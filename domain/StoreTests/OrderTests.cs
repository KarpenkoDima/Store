using store;
using System;
using System.Collections.Generic;
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
    }
}
