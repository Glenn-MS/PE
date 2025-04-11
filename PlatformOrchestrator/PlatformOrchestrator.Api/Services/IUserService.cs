using PlatformOrchestrator.Api.Models;

namespace PlatformOrchestrator.Api.Services;

public interface IUserService
{
    IEnumerable<User> GetAllUsers();
    void AddUser(User user);
    void UpdateUser(int userId, User user);
    void DeleteUser(int userId);
}
