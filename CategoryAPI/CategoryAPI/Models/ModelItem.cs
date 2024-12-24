using CategoryAPI.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CategoryAPI.Models
{
    public class ModelItem
    {
            [MaxLength(100)]
            public string Name { get; set; }
            public double Price { get; set; }
            public string? Notes { get; set; }
            public IFormFile Image { get; set; }
            public int CategoryId { get; set; }
        }
    }
