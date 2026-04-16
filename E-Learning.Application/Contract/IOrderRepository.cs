using E_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Application.Contract
{
    public interface IOrderRepository:IRepository<Order,Guid>
    {
    }
}
