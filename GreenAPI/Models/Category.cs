using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GreenAPI.Models
{
    public class Category
    {
        [JsonIgnore]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public char SizeType { get; set; }
        public bool Status { get; set; }
    }
}