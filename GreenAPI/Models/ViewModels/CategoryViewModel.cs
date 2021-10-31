using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenAPI.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public char SizeType { get; set; }
        public bool Status { get; set; }
    }
}
