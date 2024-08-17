using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapTracking.Models;

namespace WebApi.Services.Interfaces
{
    public interface IDbService
    {
        Task<List<Point>> GetAll();
        Task<Point?> GetById(int id);
        Task<Point> Add(Point point);
        Task<Point?> UpdateById(int id, Point point);
        Task<bool> DeleteById(int id);
    }
}
















