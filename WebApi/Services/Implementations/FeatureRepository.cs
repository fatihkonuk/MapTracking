using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services.Implementations
{
    public class FeatureRepository(AppDbContext context) : GenericRepository<Feature>(context)
    {
        public async Task<IEnumerable<Feature>> GetByUserIdAsync(int id)
        {
            return await _dbSet.Where(e => e.UserId == id).ToListAsync();
        }
    }
}