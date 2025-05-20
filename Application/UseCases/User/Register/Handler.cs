using System;
using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Flunt.Notifications;
using Flunt.Validations;
using MediatR;

namespace Application.UseCases.User.Register;

public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly IDbCommit _dbCommit;

    public Handler(IUserRepository userRepository, IDbCommit dbCommit,
        IMapper mapper, IEmailService emailService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _emailService = emailService;
        _dbCommit = dbCommit;
    }

    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<Domain.Entities.User>(request);

        user.AddNotifications(
            new Contract<Notifiable<Notification>>()
                .Requires()
                .IsFalse(await _userRepository.GetByEmail(request.Email, cancellationToken) != null, "Email", "Email already registered")
        );

        if (user.Notifications.Any())
            return new BaseResponse<Response>(404, "Request invalid", null, user.Notifications.ToList());

        var activationLink = $"{Configuration.FrontendUrl}/activate-account?email={Uri.EscapeDataString(user.Email.Address)}&token={Uri.EscapeDataString(user.TokenActivate.ToString())}";

        await _emailService.SendEmailAsync(
            new EmailMessage(
                To: user.Email.Address!,
                ToName: user.FullName.FirstName,
                Subject: "Activate your Account!",
                Body: $"<strong>Click the link to activate your account: <a href=\"{activationLink}\">{activationLink}</a></strong>",
                IsHtml: true,
                FromName: "KMLogger",
                FromEmail: Configuration.SmtpUser
            ),cancellationToken
        );

        await _userRepository.CreateAsync(user, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        var dto = new Response();
        return new BaseResponse<Response>(201, "User created successfully. Activation email sent.", dto);
    }
}