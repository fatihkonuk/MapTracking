using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Feature : BaseEntity
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string WKT { get; set; }
        [Required]
        public required int UserId { get; set; }
    }
}