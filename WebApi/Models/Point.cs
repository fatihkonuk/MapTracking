using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapTracking.Models
{
    public class Point
    {
        public int Id { get; set; }
        public int PointX { get; set; }
        public int PointY { get; set; }
        public string? PointName { get; set; }
    }
}