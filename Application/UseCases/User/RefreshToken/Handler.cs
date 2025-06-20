using System;
using System.Security.Claims;
using Application.UseCases.User.Login;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.User.RefreshToken;

public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IDbCommit _dbCommit;

    public Handler(IUserRepository userRepository, ITokenService tokenService, IDbCommit dbCommit)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _dbCommit = dbCommit;
    }

    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal is null)
            return new BaseResponse<Response>(401, "Invalid access token", null);

        var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(userEmail))
            return new BaseResponse<Response>(401, "Invalid token claims", null);

        var user = await _userRepository.GetByEmail(userEmail, cancellationToken);
        if (user is null)
            return new BaseResponse<Response>(404, "User not found", null);

        if (user.RefreshToken != request.RefreshToken || !user.IsRefreshTokenValid())
            return new BaseResponse<Response>(401, "Invalid refresh token", null);

        var newAccessToken = _tokenService.GenerateToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        
        var accessTokenExpiry = DateTime.UtcNow.AddHours(8);
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        user.AssignToken(newAccessToken);
        user.AssignRefreshToken(newRefreshToken, refreshTokenExpiry);
        
        await _userRepository.UpdateRefreshToken(user, cancellationToken);
        await _dbCommit.Commit(cancellationToken);       

        var response = new Response(newAccessToken, newRefreshToken, accessTokenExpiry, refreshTokenExpiry,
            new UserInfo(user.Id, user?.FullName?.FirstName, user?.Email?.Address)
        );
        return new BaseResponse<Response>(200, "Token refreshed successfully", response);
    }
}