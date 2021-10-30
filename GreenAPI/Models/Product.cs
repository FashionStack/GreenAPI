using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenAPI.Models
{
    public class Product
    {
        public long ProductId { get; set; }
        public Category Category { get; set; }
        //public int CategoryId { get; set; }
       
        [Required]
        [DisplayName("Nome")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Tamanho")]
        public string Size { get; set; }
        [Required]
        [DisplayName("SKU")]
        public string SKU { get; set; }
        [Required]
        [DisplayName("Código Referencia")]
        public string ReferenceCode { get; set; }
        [Required]
        [DisplayName("Quantidade")]
        public int Amount { get; set; }
        public string ImageUrl { get; set; }
    }
}
