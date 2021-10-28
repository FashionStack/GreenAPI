using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenAPI.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        [DisplayName("Nome")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Tamanho do Tipo")]
        public char SizeType { get; set; }
        [Required]
        [DisplayName("Status")]
        public bool Status { get; set; }
        public List<Product> Product { get; set; }
    }
}
