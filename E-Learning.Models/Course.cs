using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Models
{
    public class Course : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Ar_Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Ar_Description { get; set; }
        public decimal Price { get; set; }
        public string? CourseImage { get; set; }
        public string? PromotionalVideo { get; set; }
        public decimal RatingAverage { get; set; } = 0;
        public int RatingCount { get; set; } = 0;
        public int EnrollmentsCount { get; set; } = 0;

        // Foreign Keys
        public string UserId { get; set; } = default!;
        public virtual User Instructor { get; set; } = default!;

        public Guid TopicId { get; set; }
        public virtual Topic Topic { get; set; } = default!;

        // Relationships
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public virtual ICollection<OrderCourse> OrderCourses { get; set; } = new List<OrderCourse>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
