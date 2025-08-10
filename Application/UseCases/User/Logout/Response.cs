using System;

namespace Application.UseCases.User.Logout;

public record Response(
    string Message,
    DateTime LogoutTime
);