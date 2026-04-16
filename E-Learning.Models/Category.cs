using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Models
{
    public class Category: BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string Ar_Name { get; set; } = string.Empty;

        // Relationships
        public ICollection<SubCategory> Subcategories { get; set; } = new List<SubCategory>();
    }
}
