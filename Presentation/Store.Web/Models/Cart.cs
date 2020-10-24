using System;
using System.Collections.Generic;

namespace Store.Web.Models
{
    public class Cart
    {
        public int OrderId { get; }
        public int TotalCount { get; set; } = 0;
        public decimal TotalPrice { get; set; } = 0m;
        public Cart(int orderId)
        {
            this.OrderId = orderId;
        }
    }
}
