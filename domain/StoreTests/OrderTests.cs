using store;
using Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace StoreTests
{
    public class OrderTests
    {
        [Fact]
        public void Order_WithNullItems_ThrowArgumentNullExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Order(1, null);
            });
        }

        [Fact]
        public void Order_TotaCount_WithmptyItems_ReturnZeroTest()
        {
            var order = new Order(1, new OrderItem[0]);
            Assert.Equal(0m, order.TotalCount);
        }

        [Fact]
        public void Order_TotalCount_WithNonEmptyItems_CalculateTotalCountItems()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,3,10m),
                new OrderItem(2,5,100m)
            });

            Assert.Equal(3 + 5, order.TotalCount);
        }

        [Fact]
        public void Order_TotalPrice_WithNonEmptyItems_CalculateTotalPriceItems()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,3,10m),
                new OrderItem(2,5,100m)
            });

            Assert.Equal(3*10m + 5*100m, order.TotalPrice);
        }

        [Fact]
        public void Order_Get_WithEsxsistingItem_ReturnItemTest()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,3, 10m),
                new OrderItem(2,5,20m)
            });
            var orderItem = order.Get(1);
            Assert.Equal(3, orderItem.Count);
        }

        [Fact]
        public void Order_Get_WithNonEsxsistingItem_ThrowInvalidExceptiontest()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,3, 10m),
                new OrderItem(2,5,20m)
            });            
            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Get(0);
         
            });
        }
        [Fact]
        public void Order_AddUpdateItem_WithExistingItem_UpdateItem()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,3, 10m),
                new OrderItem(2,5,20m)
            });
            var book = new Book(1, "","","","",0m);
            order.AddOrUpdateItem(book, 10);
            Assert.Equal(13, order.Get(1).Count);
        }
        [Fact]
        public void Order_AddUpdateItem_WithNoneExistingItem_AddItem()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,3, 10m),
                new OrderItem(2,5,20m)
            });
            var book = new Book(3, "", "", "", "", 0m);
            order.AddOrUpdateItem(book, 10);
            Assert.Equal(10, order.Get(3).Count);
        }
        [Fact]
        public void Order_RemoveItem_WithExistingItem_RemoveItemTest()
        {
            var order = new Order(1, new[]
           {
                new OrderItem(1,3, 10m),
                new OrderItem(2,5,20m)
            });
            order.RemoveItem(1);
            Assert.Equal(1, order.Items.Count);
        }
        [Fact]
        public void Order_RemoveItem_WithNonExistingItem_ThrowExceptionTest()
        {
            var order = new Order(1, new[]
           {
                new OrderItem(1,3, 10m),
                new OrderItem(2,5,20m)
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                order.RemoveItem(10);
            });
        }
    }
}
