using AuthenticationAPI.Models;

namespace AuthenticationAPI.Interfaces
{
    public interface IUserAppService
    {
        Task<string> LoginAsync(LoginModel user);
        Task<UserModel> AddRoleAsync(AddRoleModel model);
        Task<string> GetUserTokenAsync(string email, string password);


    }
}
