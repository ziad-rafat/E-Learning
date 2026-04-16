using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Models
{
    public class Assignment:BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } 
        public DateTime? DueDate { get; set; }

        // Foreign Key
        public Guid SectionId { get; set; }
        public Section? Section { get; set; }
    }
}
