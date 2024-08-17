using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapTracking.Models;

namespace WebApi.Services
{
    public interface IPointService
    {
        List<Point> GetAll();
        Point? GetById(int id);
        Point Add(Point point);
        Point? UpdateById(int id, Point point);
        bool DeleteById(int id);
    }
}