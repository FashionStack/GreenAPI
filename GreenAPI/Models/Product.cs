using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GreenAPI.Models
{
    public class Product
    {
        public long ProductId { get; set; }
        [JsonIgnore]
        [ForeignKey("CategoryId")]
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