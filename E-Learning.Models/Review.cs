using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Models
{
    public class Review : BaseEntity
    {
          
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public int HelpfulVotes { get; set; } = 0;

      

        // Foreign Keys
        public Guid CourseId { get; set; }
        public Course? Course { get; set; }

        public string UserId { get; set; }
        public User? User { get; set; }
    }
}
