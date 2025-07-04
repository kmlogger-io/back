using System;

namespace Domain.Records.DTOS;

public record UserDto(
    string Id,
    string Name,
    string Email,
    DateTime CreatedAt,
    List<RoleDto> Roles,
    bool Active
);
