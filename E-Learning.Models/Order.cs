using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Models
{
    public enum OrderStatus
    {
        Pending = 0,
        Shipped = 1,
        Delivered = 2,

    }
    public class Order : BaseEntity
    {
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeleviryDate { get; set; }
        public OrderStatus? orderStatus { get; set; } = OrderStatus.Pending;
        public string PaymentMethod { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
        public string? PaymentProcessor { get; set; }

        // Foreign Key
        public string UserId { get; set; }
        public User? User { get; set; }

        public ICollection<OrderCourse> OrderCourses { get; set; } = new List<OrderCourse>();
    }
}
