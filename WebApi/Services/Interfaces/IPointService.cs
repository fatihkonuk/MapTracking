using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface IPointService
    {
        Task<IEnumerable<Point>> GetAllPointsAsync();
        Task<Point?> GetPointByIdAsync(int id);
        Task<Point> CreatePointAsync(Point point);
        Task<Point?> UpdatePointAsync(Point point);
        Task<bool> DeletePointAsync(int id);
    }
}