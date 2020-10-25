using System;
using System.Collections.Generic;
using System.Text;

namespace store
{
    public class OrderItem
    {
        public int BookId { get; }
        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            set { ThrowIfInvalidCount(value); count = value; }
        }
        public decimal Price { get; }
        public OrderItem(int bookId, int count, decimal price)
        {
            ThrowIfInvalidCount(count);
            this.BookId = bookId;
            this.Count = count;
            this.Price = price;
        }
        void ThrowIfInvalidCount(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("Count must be greater then 0");
            }
        }
        
    }
}
