using GreenAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenAPI.Context.Configurations
{
    /// <summary>
    /// Configuracoes referente a entidade na base de dados, como tamanho da coluna e not nulls.
    /// </summary>
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        /// <summary>
        /// Método usado para alterar as propriedades basicas da conversão do objeto de entidade para usar na criação de tabela do banco de dados.
        /// </summary>
        /// <param name="builder">Classe usada para alterar os valores padrões da conversão EF.</param>
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Size).HasMaxLength(5).IsRequired();
            builder.Property(c => c.SKU).HasMaxLength(50).IsRequired();
            builder.Property(c => c.ReferenceCode).HasMaxLength(50).IsRequired();
            builder.Property(c => c.Amount).IsRequired();
            builder.Property(c => c.Sustainable).IsRequired();
            builder.Property(c => c.ImageUrl).HasMaxLength(255);
        }
    }
}
