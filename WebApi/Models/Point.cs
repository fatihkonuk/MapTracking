using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Point : BaseEntity
    {
        [Required]
        public required string Name { get; set; }
        public double PointX { get; set; }
        public double PointY { get; set; }
    }
}