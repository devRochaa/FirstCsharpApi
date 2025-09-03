using System.Linq.Expressions;
using api.Models;

namespace api.Interfaces
{
    public interface IPortifolioRepository
    {
        Task<List<Stock>> GetUserPortifolio(AppUser user, string? filter = null);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        Task<Portfolio> DeleteAsync(string symbol, AppUser appUser);
    }
}