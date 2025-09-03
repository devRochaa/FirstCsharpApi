using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortifolioRepository : IPortifolioRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PortifolioRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Stock>> GetUserPortifolio(AppUser user, string? filter = null)
        {
            return string.IsNullOrWhiteSpace(filter) ? await _dbContext.Portfolios.Where(u => u.AppUserId == user.Id)
                .Select(stock => new Stock
                {
                    id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarkCap = stock.Stock.MarkCap,
                }).ToListAsync()
                :
                await _dbContext.Portfolios.Where(u => u.AppUserId == user.Id && u.Stock.Symbol.ToLower() == filter.ToLower())
                .Select(stock => new Stock
                {
                    id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarkCap = stock.Stock.MarkCap,
                }).ToListAsync();
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _dbContext.Portfolios.AddAsync(portfolio);
            await _dbContext.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio?> DeleteAsync(string symbol, AppUser appUser)
        {
            var portfolioModel = await _dbContext.Portfolios.FirstOrDefaultAsync(p => p.AppUserId == appUser.Id && p.Stock.Symbol.ToLower() == symbol.ToLower());
            if (portfolioModel == null) return null;

            _dbContext.Portfolios.Remove(portfolioModel);
            await _dbContext.SaveChangesAsync();
            return portfolioModel;
        }
    }
}