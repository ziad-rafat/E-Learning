using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Models
{
    public class Enrollment : BaseEntity
    {
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public string UserId { get; set; }
        public User? User { get; set; }

        public Guid CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
