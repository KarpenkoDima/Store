using store;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreTests
{
    public class OrderItemTest
    {
        [Fact]
        public void OrederItem_WithZeroCount_ThrowArgumetOutOfRangeExceptionTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = 0;
                new OrderItem(1, count, 0m);
            });
        }
        [Fact]
        public void OrederItem_WithNegativeCount_ThrowArgumetOutOfRangeExceptionTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = -1;
                new OrderItem(1, count, 0m);
            });
        }
        [Fact]
        public void OrederItem_WithPositiveCount_SetsTest()
        {
            var orderItem = new OrderItem(1, 2, 3m);
            Assert.Equal(1, orderItem.BookId);
            Assert.Equal(2, orderItem.Count);
            Assert.Equal(3, orderItem.Price);
        }
    }
}
