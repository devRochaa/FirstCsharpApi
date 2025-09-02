using api.Models;

namespace api.Interfaces
{
    public interface IPortifolioRepository
    {
        Task<List<Stock>> GetUserPortifolio(AppUser user);
    }
}