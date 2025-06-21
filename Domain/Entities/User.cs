using System.ComponentModel.DataAnnotations.Schema;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User : Entity
{
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public Address Address { get; private set; }
    public Password Password { get; private set; }
    public bool Active { get; private set; }
    public IList<Role> Roles { get; private set; }
    public Guid? TokenActivate { get; private set; }
   
    [NotMapped]
    public string Token { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiryTime { get; private set; }
   
    protected User(){}

    //Construtor para Register.
    public User(FullName? fullName, Email? email, List<Role> roles)
    {
        AddNotificationsFromValueObjects(fullName, email);
        FullName = fullName;
        Email = email;
        Roles = roles;
        Active = false;
    }
   
    public User(Email email, Password password)
    {
        AddNotificationsFromValueObjects(email, password);
        Password = password;
        Email = email;
    }
    
    public void GenerateNewToken()
        => TokenActivate = Guid.NewGuid();

    public void UpdatePassword(Password password)
    {
        AddNotificationsFromValueObjects(password);
        Password = password;
        Active = true;
    }

    public void ResetPassword()
    {
        Active = false;
    }

    public void AssignToken(string token) => Token = token;
    
    public void AssignRefreshToken(string refreshToken, DateTime expiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
    }
    
    public void ClearRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiryTime = null;
    }
    
    public bool IsRefreshTokenValid()
        => !string.IsNullOrEmpty(RefreshToken) && 
           RefreshTokenExpiryTime.HasValue && 
           RefreshTokenExpiryTime.Value > DateTime.UtcNow;
}