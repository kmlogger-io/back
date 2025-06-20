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
   
    public User(FullName? fullName, Email? email, Address? address, bool active, Password? password)
    {
        AddNotificationsFromValueObjects(fullName, email, password);
        FullName = fullName;
        Email = email;
        Address = address;
        Active = active;
        Password = password;
        TokenActivate = Guid.NewGuid();
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
    
    public void AssignActivate(bool isActivate)
    {
        Active = isActivate;
        TokenActivate = Guid.Empty;
    }
    
    public void ClearToken()
        => TokenActivate = Guid.Empty;
        
    public void AssignRole(Role role)
    {
        if (Roles == null)
            Roles = new List<Role>();
        Roles.Add(role);
    }
}