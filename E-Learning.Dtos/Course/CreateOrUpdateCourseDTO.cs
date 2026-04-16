using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Dtos.Course
{
    public class CreateOrUpdateCourseDTO
    {
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; } = default!;

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Ar_Title { get; set; } = default!;

        [StringLength(2000)]
        public string? Description { get; set; }

        [StringLength(2000)]
        public string? Ar_Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public IFormFile? Image { get; set; }
        public string? CourseImage { get; set; }

        public IFormFile? Video { get; set; }
        public string? PromotionalVideo { get; set; }

        [Required]
        public Guid TopicId { get; set; }
    }
}
