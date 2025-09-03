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
        private readonly IFMPService _fmpService;
        public PortifolioController(UserManager<AppUser> userManager,
                                    IStockRepository stockRepo,
                                    IPortifolioRepository portifolioRepo,
                                    IFMPService fmpService)
        {
            _portifolioRepo = portifolioRepo;
            _userManager = userManager;
            _stockRepo = stockRepo;
            _fmpService = fmpService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortifolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortifolio = await _portifolioRepo.GetUserPortifolio(appUser!);
            return Ok(userPortifolio);
        }

        [HttpPost("{symbol:alpha}")]
        [Authorize]
        public async Task<IActionResult> AddPortifolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                {
                    return BadRequest("This stock does not exists");
                }
                else
                {
                    await _stockRepo.CreateAsync(stock);
                }
            }

            var userPortifolio = await _portifolioRepo.GetUserPortifolio(appUser!);
            if (userPortifolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Cannot add same stock to portifolio");

            var portifolioModel = new Portfolio
            {
                StockId = stock.id,
                AppUserId = appUser!.Id
            };

            await _portifolioRepo.CreateAsync(portifolioModel);

            if (portifolioModel == null) return StatusCode(500, "Could not create it");
            return Created();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Remove(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortifolio = await _portifolioRepo.GetUserPortifolio(appUser!, symbol);
            if (userPortifolio == null) return BadRequest("Stock is not in your portifolio");

            await _portifolioRepo.DeleteAsync(symbol: symbol, appUser: appUser!);
            return Ok();
        }
    }

}