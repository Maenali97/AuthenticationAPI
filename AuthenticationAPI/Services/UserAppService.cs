using AuthenticationAPI.Interfaces;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationAPI.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly ITokenAppService _tokenAppService;

        public UserAppService(ITokenAppService tokenAppService)
        {
            _tokenAppService = tokenAppService;
        }

        public async Task<string> LoginAsync(LoginModel user)
        {

            if (string.IsNullOrEmpty(user.Email))
                throw new Exception("Email Is Requred");

            if (string.IsNullOrEmpty(user.Password))
                throw new Exception("Password Is Requred");

            var currentUser = Consts.Users.FirstOrDefault(o => o.Email.ToLower() == user.Email.ToLower() && o.Password == user.Password);

            if (currentUser == null)
                throw new Exception("Email Or Password Not Correct");

            var token = await _tokenAppService.CreateTokenAsync(currentUser);

            return token;

        }
        public async Task<UserModel> AddRoleAsync(AddRoleModel model)
        {
            var user = Consts.Users.Find(x => x.Id == model.UserId);

            if (user is null ||( Consts.Roles.Find(x => x.Name.ToLower() == model.Role.ToLower()) == null))
                throw new Exception("Invalid USer Id or Role");

            var userRoles = Consts.GetUserRoles(model.UserId);

            if (userRoles.Find(x => x.Name == model.Role) != null)
                throw new Exception("User Already Asighn On This Role");

            var roleModel = Consts.Roles.Find(x => x.Name.ToLower() == model.Role.ToLower());

            if (roleModel is null)
                throw new Exception("Role Not Found");

            var userRole = new UserRoleModel { UserId = model.UserId, RoleId = roleModel.Id };

            Consts.UserRoles.Add(userRole);

            return user;

        }
        public async Task<string> GetUserTokenAsync(string email, string password)
        {
            var user = Consts.Users.Find(x => x.Email == email);

            if (user is null || user.Password == password)
                throw new Exception("Email Or Pawssowrd is Incorrect");

            return await _tokenAppService.CreateTokenAsync(user);
        }
    }
}
