using AutoMapper;

namespace Application.Mappers;

public class ValueObjectsMapper : Profile
{
    public ValueObjectsMapper()
    {
        CreateMap<Domain.ValueObjects.Email, string>()
            .ConvertUsing(email => email.Address);

        CreateMap<string, Domain.ValueObjects.Email>()
            .ConstructUsing(address => new Domain.ValueObjects.Email(address));

        CreateMap<Domain.ValueObjects.Password, string>()
            .ConvertUsing(password => password.Hash);

        CreateMap<string, Domain.ValueObjects.Password>()
            .ConstructUsing(hash => new Domain.ValueObjects.Password(hash, true));

        CreateMap<Domain.ValueObjects.UniqueName, string>()
            .ConvertUsing(src => src.Name);
            
        CreateMap<string, Domain.ValueObjects.UniqueName>()
            .ConstructUsing(name => new Domain.ValueObjects.UniqueName(name));

        CreateMap<Domain.ValueObjects.FullName, string>()
            .ConvertUsing(fullName => $"{fullName.FirstName} {fullName.LastName}");
    }
}
