using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Implementations
{
    public class DbService(DbContextOptions<DbService> options) : DbContext(options)
    {
        public DbSet<Point> Points { get; set; }
    }
}
