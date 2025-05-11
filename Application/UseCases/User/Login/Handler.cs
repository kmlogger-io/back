using System;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;
using Flunt.Notifications;

namespace Application.UseCases.User.Login;

public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public Handler(IUserRepository userRepository, ITokenService tokenService, IMapper mapper)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<Domain.Entities.User>(request);
        var isAuthenticated = await _userRepository.Authenticate(user, cancellationToken);

        if (!isAuthenticated || user.Notifications.Any())
            return new BaseResponse<Response>(403, "Invalid password or user", null, user.Notifications.ToList());

        var token = _tokenService.GenerateToken(user);
        user.AssignToken(token);

        var dto = new Response(token);
        return new BaseResponse<Response>(200, "Login successful", dto);
    }
}