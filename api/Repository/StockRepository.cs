using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public StockRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _dbContext.Stocks.AddAsync(stockModel);
            await _dbContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.id == id);
            if (stockModel == null)
            {
                return null;
            }
            _dbContext.Stocks.Remove(stockModel);
            await _dbContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _dbContext.Stocks.Include(c => c.Comments).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _dbContext.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.id == id);
            if (existingStock == null)
            {
                return null;
            }
            _dbContext.Entry(existingStock).CurrentValues.SetValues(stockDto);
            await _dbContext.SaveChangesAsync();
            return existingStock;
        }
    }
}