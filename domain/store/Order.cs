using store.store;
using Store;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace store
{
    public class Order
    {
        private List<OrderItem> items;

        public string CellPhone { get; set; }

        public int Id { get; }
        public IReadOnlyCollection<OrderItem> Items
        {
            get
            {
                return items;
            }
        }
        public OrderDelivery Delivery { get; set; }
        public OrderPayment Payment { get; set; }
        public int TotalCount =>  items.Sum(item=> item.Count);
        public decimal TotalPrice => items.Sum(item => item.Price * item.Count) + (Delivery?.Amount ?? 0m);
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
        public void AddOrUpdateItem(Book book, int count)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            int index = items.FindIndex(item=>item.BookId == book.Id);
            if (index == -1)
            {
                items.Add(new OrderItem(book.Id, count, book.Price));
            }
            else
            {
                items[index].Count += count;
            }           
        }
        public void RemoveItem(int bookId)
        {            
            int index = items.FindIndex(b => b.BookId == bookId);
            if (index == -1)
            {
                ThrowBookException($"Cart does not contains that book ", bookId);                
            }
            /* if (items.Count == 0)
             {
                 throw new InvalidOperationException("Car must conatin books");
             }*/
            items.RemoveAt(index);
        }
       
        private void ThrowBookException(string message, int bookId)
        {
            var exceprion = new InvalidOperationException(message);
            exceprion.Data[nameof(bookId)] = bookId;
            
            throw exceprion;
        }

        public OrderItem Get(int id)
        {
            int index = items.FindIndex(item => item.BookId == id);
            if (index == -1)
            {
                throw new InvalidOperationException("Book not found" + id);
            }
            return items[index];
        }

        public bool ContainsItem(int bookId)
        {
            return items.Any(items => items.BookId == bookId);
        }
    }
}
