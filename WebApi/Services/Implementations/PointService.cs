using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Implementations
{
    public class PointService(DbService dbService) : IPointService
    {
        private readonly DbService _dbService = dbService;

        public async Task<IEnumerable<Point>> GetAllPointsAsync()
        {
            return await _dbService.Points.ToListAsync();
        }

        public async Task<Point?> GetPointByIdAsync(int id)
        {
            return await _dbService.Points.FindAsync(id);
        }

        public async Task<Point> CreatePointAsync(Point point)
        {
            _dbService.Points.Add(point);
            await _dbService.SaveChangesAsync();
            return point;
        }
        public async Task<Point?> UpdatePointAsync(Point updatedPoint)
        {
            _dbService.Points.Update(updatedPoint);
            await _dbService.SaveChangesAsync();
            return updatedPoint;
        }
        public async Task<bool> DeletePointAsync(int id)
        {
            var product = await _dbService.Points.FindAsync(id);
            if (product == null)
                return false;

            _dbService.Points.Remove(product);
            await _dbService.SaveChangesAsync();
            return true;
        }
    }
}