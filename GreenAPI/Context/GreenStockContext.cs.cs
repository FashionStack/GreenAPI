using GreenAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenAPI.Context
{
    public class GreenStockContext : DbContext
    {
        public GreenStockContext(DbContextOptions<GreenStockContext> options) : base(options)
        {

        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}
