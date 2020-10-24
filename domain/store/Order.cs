using Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace store
{
    public class Order
    {
        private List<OrderItem> items;
        public int Id { get; }
        public IReadOnlyCollection<OrderItem> Items
        {
            get
            {
                return items;
            }
        }
        public int TotalCount
        {
            get
            {
                return items.Sum(item=> item.Count);
            }
        }
        public decimal TotalPrice
        {
            get
            {
                return items.Sum(item => item.Price * item.Count);
            }
        }

        public Order(int id, IEnumerable<OrderItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(this.items));
            }
            this.Id = id;
            this.items = new List<OrderItem>(items);
        }
        // Need Tests
        public void AddItem(Book book, int count)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            var item = items.SingleOrDefault(x => x.BookId == book.Id);
            if (item == null)
            {
                items.Add(new OrderItem(book.Id, count, book.Price));
            }
            else
            {
                items.Remove(item);
                items.Add(new OrderItem(book.Id, item.Count +count, book.Price));
            }
        }
    }
}
