using AuthenticationAPI.Models;

namespace AuthenticationAPI
{
    public static class Consts
    {
        private static readonly List<UserModel> _users = new List<UserModel>
    {
        new UserModel {Id = 1, UserName = "AliAhmad", Password = "qqq111", Email = "aliahmad@example.com" },
        new UserModel {Id = 2, UserName = "OmerKhaled", Password = "zzz222", Email = "omerkhaled@example.com" },
        new UserModel {Id = 3, UserName = "RedaAli", Password = "ccc444", Email = "redaali@example.com" }
    };

        private static readonly List<RegionModel> _regions = new List<RegionModel>
    {
        new RegionModel { Id = 1, Name =  "b_game" },
        new RegionModel { Id = 2, Name =  "vip_chararacter_personalize" },
    };

        private static readonly List<RoleModel> _roles = new List<RoleModel>
    {
        new RoleModel { Id = 1, Name =  "Admin" },
        new RoleModel { Id = 2, Name =  "Player" },
    };

        private static readonly List<UserRoleModel> _userRoles = new List<UserRoleModel>
    {
        new UserRoleModel { UserId = 1, RoleId =  1 },
        new UserRoleModel { UserId = 1, RoleId =  2 },
    };

        private static readonly List<UserRegionModel> _userRegions = new List<UserRegionModel>
    {
        new UserRegionModel { UserId = 1, RegionId =  1 },
        new UserRegionModel { UserId = 1, RegionId =  2 },
    };

        public static List<UserModel> Users => _users;
        public static List<RegionModel> Regions => _regions;
        public static List<RoleModel> Roles => _roles;
        public static List<UserRoleModel> UserRoles => _userRoles;
        public static List<UserRegionModel> UserRegions => _userRegions;


        public static List<RoleModel> GetUserRoles(int userId)
        {
            return _userRoles
                .Where(ur => ur.UserId == userId)
                .Join(_roles, ur => ur.RoleId, r => r.Id, (ur, r) => r)
                .ToList();
        }
        public static List<RegionModel> GetUserRegions(int userId)
        {
            return _userRegions
                .Where(ur => ur.UserId == userId)
                .Join(_regions, ur => ur.RegionId, r => r.Id, (ur, r) => r)
                .ToList();
        }
    }
}
