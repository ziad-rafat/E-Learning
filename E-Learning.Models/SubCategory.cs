using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Models
{
    public class SubCategory : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Ar_Name { get; set; } = string.Empty;

        // Foreign Key
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();
    }
}
