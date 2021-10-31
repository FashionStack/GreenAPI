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
        [DisplayName("Medida numrica ou letra ? ")]
        public char SizeType { get; set; }
        [Required]
        [DisplayName("Essa roupa e sustentavel ? ")]
        public bool Status { get; set; }
    }
}
