using System;
using AutoMapper;
using Domain.ValueObjects;

namespace Application.UseCases.User.Register;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<string, Password>()
            .ConstructUsing(password => new Password(password, false));

        CreateMap<Request, Domain.Entities.User>()
            .ConstructUsing(request => new Domain.Entities.User(
                new FullName(request.FirstName, request.LastName),
                new Email(request.Email),
                new Address(request.Number, request.NeighBordHood, request.Road, request.Complement),
                false,
                new Password(request.Password, false)
            ));
    }
}
