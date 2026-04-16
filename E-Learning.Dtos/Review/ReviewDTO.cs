using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Dtos.Review
{
    public class ReviewDTO
    {
        public Guid id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public int HelpfulVotes { get; set; } = 0;
    }
}
