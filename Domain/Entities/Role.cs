using Domain.ValueObjects;

namespace Domain.Entities;

public class Role : Entity
{
    public UniqueName Name { get; private set; }
    public string Slug { get; private set; }
    public IList<User>? Users { get;  private set; }
    private Role(){}
}