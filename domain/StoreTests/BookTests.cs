using Store;
using System;
using Xunit;

namespace StoreTests
{
    public class BookTests
    {
        [Fact]
        public void IsIsbn_WithNull_ReturnFalseTest()
        {
            bool actual = Book.IsIsbn(null);
            Assert.False(actual);
        }
        [Fact]
        public void IsIsbn_WithBlankString_ReturnFalseTest()
        {
            bool actual = Book.IsIsbn(string.Empty);
            Assert.False(actual);
        }
        [Fact]
        public void IsIsbn_WithInvalidIsbn_ReturnFalseTest()
        {
            bool actual = Book.IsIsbn("ISBN 123");
            Assert.False(actual);
        }
        [Fact]
        public void IsIsbn_WithIsbn10_ReturnTrueTest()
        {
            bool actual = Book.IsIsbn("ISBN 123-456-7890");
            Assert.True(actual);
        }
        [Fact]
        public void IsIsbn_WithTrashIsbn_ReturnFalseTest()
        {
            bool actual = Book.IsIsbn("ISBN /*5123-456-7890");
            Assert.False(actual);
        }
    }
}
