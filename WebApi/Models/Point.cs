using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapTracking.Models
{
    public class Point
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public double PointX { get; set; }
        public double PointY { get; set; }
    }
}