using AuthenticationAPI.Interfaces;
using AuthenticationAPI.Models;

namespace AuthenticationAPI.Services
{

    public class RegionAppService : IRegionAppService
    {
        public async Task<bool> IsUserRegionAuthinticatedAsync(int userId, int regionId)
        {
            if (userId == 0)
                throw new Exception("Invalid userId");


            var user = Consts.Users.Find(x => x.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            var userRegs = Consts.GetUserRegions(userId);

            var regien = userRegs.Find(x => x.Id == regionId);

            if (regien is null)
                return false;

            return true;

        }

        public async Task<UserModel> AddUserRegionAsync(int userId, int regionId)
        {
            if (userId == 0)
                throw new Exception("Invalid userId");


            var user = Consts.Users.Find(x => x.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            var userRegs = Consts.GetUserRegions(userId);

            var regien = userRegs.Find(x => x.Id == regionId);

            if (regien != null)
                throw new Exception("Regien already excest");

            var userRegien = new UserRegionModel { UserId = userId, RegionId = regionId };

            Consts.UserRegions.Add(userRegien);

            return user;

        }


    }
}
