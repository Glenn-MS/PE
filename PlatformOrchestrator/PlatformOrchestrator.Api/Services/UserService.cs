using PlatformOrchestrator.Api.Models;

namespace PlatformOrchestrator.Api.Services;

public class UserService : IUserService
{
    private readonly List<User> _users = new();

    public IEnumerable<User> GetAllUsers()
    {
        return _users;
    }

    public void AddUser(User user)
    {
        user.Id = _users.Count + 1;
        _users.Add(user);
    }

    public void UpdateUser(int userId, User user)
    {
        var existingUser = _users.FirstOrDefault(u => u.Id == userId);
        if (existingUser != null)
        {
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;
        }
    }

    public void DeleteUser(int userId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            _users.Remove(user);
        }
    }

    public void AssignRole(int userId, string role)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user != null && !user.Roles.Contains(role))
        {
            user.Roles.Add(role);
        }
    }

    public void RemoveRole(int userId, string role)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user != null && user.Roles.Contains(role))
        {
            user.Roles.Remove(role);
        }
    }
}
