using Application.UseCases.User.Login;

namespace Application.UseCases.User.RefreshToken;

public record Response(
    string Token,
    string RefreshToken,
    DateTime TokenExpiry,
    DateTime RefreshTokenExpiry,
    UserInfo? User
);