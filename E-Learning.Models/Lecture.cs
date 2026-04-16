using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Models
{
    public class Lecture : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Ar_Title { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public int Duration { get; set; }

        // Foreign Key
        public Guid SectionId { get; set; }
        public Section? Section { get; set; }
    }
}
