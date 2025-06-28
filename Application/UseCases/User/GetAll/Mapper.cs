using AutoMapper;

namespace Application.UseCases.User.GetAll;

public class Mapper : Profile
{
    public Mapper()
    {
        _ = CreateMap<Domain.Entities.User, Domain.Records.DTOS.UserDto>()
            .ConstructUsing(src => new Domain.Records.DTOS.UserDto(
                src.Id.ToString(),
                $"{src.FullName.FirstName} {src.FullName.LastName}",
                src.Email.Address, 
                src.CreatedDate,
                src.Roles != null 
                    ? src.Roles.Select(role => new Domain.Records.DTOS.RoleDto(
                        role.Id.ToString(),
                        role.Name.Name, 
                        role.Slug
                    )).ToList() 
                    : new List<Domain.Records.DTOS.RoleDto>(),
                src.Active
            ));
            
        CreateMap<Domain.Entities.Role, Domain.Records.DTOS.RoleDto>()
            .ConstructUsing(src => new Domain.Records.DTOS.RoleDto(
                src.Id.ToString(),
                src.Name.Name,
                src.Slug
            ));
    }
}