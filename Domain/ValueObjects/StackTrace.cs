using System;
using Flunt.Br;

namespace Domain.ValueObjects;

public class StackTrace : BaseValueObject
{
    public string? Body { get; private set; }
    public StackTrace(string text)
    {
        if(text is not null)
        {
            AddNotifications(
                new Contract()
                .IsLowerThan(text.Length, 5000, Key, "Body cannot be longer than 5000 characters")
            );
            Body = text;
        }
    }
    private StackTrace(){}
}
