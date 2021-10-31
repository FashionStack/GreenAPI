using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenAPI.Models
{
    public class ProductViewModel
    {
        public long ProductId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string SKU { get; set; }
        public string ReferenceCode { get; set; }
        public int Amount { get; set; }
        public bool Sustainable { get; set; }
        public string ImageUrl { get; set; }
    }
}