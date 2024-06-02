using AuthenticationAPI.Models;

namespace AuthenticationAPI.Interfaces
{
    public interface IRegionAppService
    {
        Task<bool> IsUserRegionAuthinticatedAsync(int userId, int regionId);
        Task<UserModel> AddUserRegionAsync(int userId, int regionId);
    }
}
