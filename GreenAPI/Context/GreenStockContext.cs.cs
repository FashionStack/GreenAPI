using GreenAPI.Context.Configurations;
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

        /// <summary>
        /// Equivalente a tabela 'Product' no banco de dados
        /// </summary>
        public DbSet<Product> Product { get; set; }

        /// <summary>
        /// Equivalente a tabela 'Category' no banco de dados
        /// </summary>
        public DbSet<Category> Category { get; set; }

        /// <summary>
        /// Método para aplicar as configurações das tabelas, como tamanho da coluna e se é not null.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
