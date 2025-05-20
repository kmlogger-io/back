using System;
using Domain;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.User.ForgotPassword;

/// <summary>
/// Handles the generation of a password reset token and sends an activation email to the user.
/// </summary>
public class Handler(
    IUserRepository userRepository,
    IDbCommit dbCommit,
    IEmailService emailService) : IRequestHandler<Request, BaseResponse<Response>>
{
    /// <summary>
    /// Processes the password reset request: generates a token and sends a reset email.
    /// </summary>
    /// <param name="request">The email of the user requesting a password reset</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="BaseResponse{Response}"/> indicating success or failure</returns>
    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var userFromDb = await userRepository.GetByEmail(request.Email, cancellationToken);
        if (userFromDb is null)
        {
            return new BaseResponse<Response>(404, "User not found");
        }

        userFromDb.GenerateNewToken();
        userRepository.Update(userFromDb);

        var resetLink = $"{Configuration.PublicUrlFrontEnd}/auth/forgot-password-activate/?token={userFromDb.TokenActivate}";

        var commitTask = dbCommit.Commit(cancellationToken);
        var emailTask = emailService.SendEmailAsync(
            new EmailMessage(
                To: userFromDb.Email.Address!,
                ToName: userFromDb.FullName.FirstName,
                Subject: "Reset your password!",
                Body: $"<strong>Click the link to reset your password: <a href=\"{resetLink}\">{resetLink}</a></strong>",
                IsHtml: true,
                FromName: "KMLogger",
                FromEmail: Configuration.SmtpUser
            ),
            cancellationToken
        );

        await Task.WhenAll(commitTask, emailTask);
        return new BaseResponse<Response>(201, "Password reset email sent", new Response());
    }
}