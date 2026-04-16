using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Models
{
    public class Section : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Ar_Title { get; set; } = string.Empty;

        // Foreign Key
        public Guid CourseId { get; set; }
        public Course? Course { get; set; }

        public ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
       // public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
