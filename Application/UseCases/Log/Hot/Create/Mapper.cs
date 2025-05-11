using System;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Log = Domain.Entities.LogApp;

namespace Application.UseCases.Log.Hot.Create
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Request, LogApp>()
                .ConstructUsing(request => new LogApp(
                    request.Environment,
                    ParseLevel(request.Level), 
                    new Description(request.Message),
                    new StackTrace(request.StackTrace),
                    request.AppId
                ));
        }

        private static Level ParseLevel(string level)
        {
            return Enum.TryParse<Level>(level, true, out var parsedLevel) 
                ? parsedLevel 
                : Level.Information;
        }
    }
}
