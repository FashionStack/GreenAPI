using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GreenAPI.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public char SizeType { get; set; }
        public bool Status { get; set; }
    }
}