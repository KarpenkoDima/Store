using Moq;
using store;
using Store;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreTests
{
    public class BookServiceTests
    {
        [Fact]
        public void GetAllByQuery_WithIsbn_CallsGetAllByIsbn()
        {
            var bookRepository = new Mock<IBookRepository>();
            bookRepository.Setup(x => x.GetAllByISBN(It.IsAny<string>()))
                .Returns(new[] { new Book(1,"","","","",0.0m)});
            bookRepository.Setup(x => x.GetAllByTittleOrAuthor(It.IsAny<string>()))
               .Returns(new[] { new Book(2, "", "", "", "", 0.0m) });

            var bookService = new BookService(bookRepository.Object);
            var validISBN = "ISBN 12345-67890";
            var actual = bookService.GetAllByQuery(validISBN);
            Assert.Collection(actual, book=>Assert.Equal(1, book.Id));
        }
        [Fact]
        public void GetAllByQuery_WithAuthor_CallsGetAllByTitleOrAuthors()
        {
            var bookRepository = new Mock<IBookRepository>();
            bookRepository.Setup(x => x.GetAllByISBN(It.IsAny<string>()))
                .Returns(new[] { new Book(1, "", "", "","",0.0m) });
            bookRepository.Setup(x => x.GetAllByTittleOrAuthor(It.IsAny<string>()))
               .Returns(new[] { new Book(2, "", "", "","", 0.0m) });

            var bookService = new BookService(bookRepository.Object);
            var invalidISBN = "12345-67890";
            var actual = bookService.GetAllByQuery(invalidISBN);
            Assert.Collection(actual, book => Assert.Equal(2, book.Id));
        }
    }
}
