using AuthenticationAPI.Interfaces;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationAPI.Services
{
    public class TokenAppService: ITokenAppService
    {
        private readonly JWT _jwt;

        public TokenAppService(IOptions<JWT>  jwtOptions)
        {
            _jwt = jwtOptions.Value;
        }

        public async Task<string> CreateTokenAsync(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = Consts.GetUserRoles(user.Id);
            var regions = Consts.GetUserRegions(user.Id);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Roles", string.Join(",",roles)),
                new Claim("Regions", string.Join(",",regions))
            };


            var token = new JwtSecurityToken(_jwt.Issuer,
              _jwt.Audience,
              claims,
              expires: DateTime.Now.AddDays(_jwt.Duration),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
