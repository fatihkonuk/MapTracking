using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapTracking.Models;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Services
{
    public class PointService : IPointService
    {
        private static readonly List<Point> _points = [];
        private static int _count = 1;

        public List<Point> GetAll()
        {
            return _points;
        }

        public Point? GetById(int id)
        {
            Point? _point = _points.FirstOrDefault(e => e.Id == id);
            if (_point == null)
                return null;

            return _point;
        }
        public Point Add(Point point)
        {
            point.Id = _count++;
            _points.Add(point);
            return point;
        }

        public bool DeleteById(int id)
        {
            Point? point = _points.FirstOrDefault(e => e.Id == id);
            if (point == null)
                return false;

            _points.Remove(point);
            return true;
        }

        public Point? UpdateById(int id, Point point)
        {
            Point? existingPoint = _points.FirstOrDefault(e => e.Id == id);
            if (existingPoint == null)
                return null;

            int index = _points.IndexOf(existingPoint);
            _points[index] = point;
            _points[index].Id = id;

            return _points[index];
        }
    }
}