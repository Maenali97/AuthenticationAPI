using AuthenticationAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionAppService _regionAppService;
        private readonly ILogger<RegionController> _logger;
        public RegionController(IRegionAppService regionAppService, ILogger<RegionController> logger)
        {
            _regionAppService = regionAppService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("getregions")]
        public async Task<IActionResult> GetRegions()
        {
            return Ok(Consts.Regions);
        }

        [Authorize]
        [HttpGet("isuserregionauthinticated")]
        public async Task<IActionResult> IsUserRegionAuthinticated(int userId, int regionId)
        {
            var isAuth = await _regionAppService.IsUserRegionAuthinticatedAsync(userId, regionId);
            return Ok(isAuth);
        }

        [Authorize]
        [HttpPost("adduserregion")]
        public async Task<IActionResult> AddUserRegion(int userId, int regionId)
        {
            var user = await _regionAppService.AddUserRegionAsync(userId, regionId);
            return Ok(user);
        }
    }
}
