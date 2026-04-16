using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Dtos.Topic
{
    public class GetAllTopicDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SubCategoryName { get; set; }
        public int  NumberOfCourses {get; set; }
        public int NumberOfStudents { get; set; }
    }
}
