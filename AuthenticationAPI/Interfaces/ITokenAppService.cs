using AuthenticationAPI.Models;

namespace AuthenticationAPI.Interfaces
{
    public interface ITokenAppService
    {
        Task<string> CreateTokenAsync(UserModel user);
    }
}
