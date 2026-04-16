using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Dtos.Course
{
    public class CourseDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Ar_Title { get; set; } = default!;
        public string? Description { get; set; }
        public string? Ar_Description { get; set; }
        public decimal Price { get; set; }
        public string? CourseImage { get; set; }
        public string? PromotionalVideo { get; set; }
        public decimal RatingAverage { get; set; }
        public int RatingCount { get; set; }
        public int EnrollmentsCount { get; set; }
        public string InstructorId { get; set; } = default!;
        public string InstructorName { get; set; } = default!;
        public Guid TopicId { get; set; }
        public string TopicName { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
