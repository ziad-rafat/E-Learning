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
    public class SectionRepository : Repository<Section, Guid>, ISectionRepository
    {
        public SectionRepository(ELearningContext context) : base(context)
        {
        }
    }
}
