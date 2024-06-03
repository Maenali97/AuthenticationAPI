#  JWT Token ASP.NET Core Simple Project Authentication API

This project demonstrates how to implement JSON Web Tokens (JWT) authentication in an ASP.NET Core application.


- Visual Studio 2022
- .NET Core 6.0

## Features
- User authentication with JWT
- Token generation
- Protected routes

## Nugget Packages used
- Microsoft.AspNetCore.Authentication.JwtBearer (version 6.0.0)
- Swashbuckle.AspNetCore (version 6.6.2)
- System.IdentityModel.Tokens.Jwt (version 7.6.0)


## Project Structure
- Controllers/ - Contains API controllers
- Models/ - Contains data models
- Services/ - Contains service classes
- Interfaces/ - Contains service interfaces
- appsettings.json - Configuration file

## Steps are made to the project
- ### Step 1: Add Nuget Packeges
```csharp
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="6.0.0" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.6.0" />
  </ItemGroup>
```
- ### Step 2: Add The Following in appsettings.json
```json
  "JWT": {
    "Key": "sz8eI7OdHBrjrIo8j9nTW/rQyO1OvY0pAQ2wDKQZw/0=",
    "Issuer": "MyAuth",
    "Audience": "UserAuth",
    "Duration": 1
  }
```
 - ### Step 3: Create User Model
```csharp
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
```
 - ### Step 4: Create Consts For Static Data
```csharp
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
```
 - ### Step 5: Create JWT Helper Class
```csharp
    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double Duration { get; set; }
    }
```
 - ### Step 6: Add The Folowing In Program
```csharp
var configration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestApiJWT", Version = "v1" });
});

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = configration["JWT:Issuer"],
        ValidAudience = configration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configration["JWT:Key"]))
    };
});

builder.Services.Configure<JWT>(configration.GetSection("JWT"));
```
 - ### Step 7: Create Token Service With Interface
```csharp
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

    public interface ITokenAppService
    {
        Task<string> CreateTokenAsync(UserModel user);
    }
```
- ### Step 8: Create User Service With Interface
```csharp
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
    public interface IUserAppService
    {
        Task<string> LoginAsync(LoginModel user);
        Task<UserModel> AddRoleAsync(AddRoleModel model);
        Task<string> GetUserTokenAsync(string email, string password);


    }
```
- ### Step 9: Create User Controller 
```csharp
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserAppService userAppService, ILogger<UserController> logger)
        {
            _userAppService = userAppService;
            _logger = logger;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel user)
        {
            var result = await _userAppService.LoginAsync(user);
            _logger.LogInformation("Login Successfully");
            return Ok(new { token  = result });
        }
        [Authorize]
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRole(AddRoleModel model)
        {

            var result = await _userAppService.AddRoleAsync(model);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("getusertoken")]
        public async Task<IActionResult> GetUserToken(string email, string password)
        {

            var result = await _userAppService.GetUserTokenAsync(email, password);

            return Ok(result);
        }
    }
```

- ### Step 10: Test All The API's 
