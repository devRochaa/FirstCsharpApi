using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/portifolio")]
    public class PortifolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortifolioRepository _portifolioRepo;
        public PortifolioController(UserManager<AppUser> userManager,
                                    IStockRepository stockRepo,
                                    IPortifolioRepository portifolioRepo)
        {
            _portifolioRepo = portifolioRepo;
            _userManager = userManager;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortifolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortifolio = await _portifolioRepo.GetUserPortifolio(appUser);
            return Ok(userPortifolio);
        } 
    }
}