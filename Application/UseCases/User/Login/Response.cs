
namespace Application.UseCases.User.Login;

public record Response(
    string Token,
    string RefreshToken,
    DateTime TokenExpiry,
    DateTime RefreshTokenExpiry,
    UserInfo User
);

public record UserInfo(
    Guid Id,
    string Name,
    string Email
);