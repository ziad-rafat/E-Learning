using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Dtos.Lecture
{
    public class CreateOrUpdateLecture
    {
        public Guid Id { get; set; }
        public string Title { get; set; } 
        public string Ar_Title { get; set; } 
        public  IFormFile? VideoUrl { get; set; } 
        public int Duration { get; set; }
        public Guid SectionId { get; set; }

    }
}
