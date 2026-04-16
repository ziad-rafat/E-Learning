using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Dtos.Course
{
    public class CourseListDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Ar_Title { get; set; } = default!;
        public decimal Price { get; set; }
        public string? CourseImage { get; set; }
        public decimal RatingAverage { get; set; }
        public int RatingCount { get; set; }
        public int EnrollmentsCount { get; set; }
        public string? InstructorFirstName { get; set; } = default!;
        public string? InstructorLastName { get; set; } = default!;

        public string TopicName { get; set; } = default!;
    }
}
