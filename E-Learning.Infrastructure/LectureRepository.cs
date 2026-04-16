using E_Learning.Application.Contract;
using E_Learning.Context;
using E_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Infrastructure
{
    public class LectureRepository : Repository<Lecture, Guid>, ILectureRepository
    {
        public LectureRepository(ELearningContext context) : base(context)
        {
        }
    }
}
