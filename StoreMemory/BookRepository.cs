using store;
using Store;
using System;
using System.Linq;

namespace StoreMemory
{
    public class BookRepository : IBookRepository
    {
        readonly Book[] books = new[] 
        { new Book(1,"ISBN 12345-67891", "Donald Knut", "Art of Programming"),
            new Book(2, "ISBN 11111-11111", "M. Fowler", "Refactoring"), 
            new Book(3, "ISBN 09876-54321", "B. Kernigan", "C Programming Language") 
        };

        public Book[] GetAllByISBN(string isbn)
        {
            return books
                .Where(book => book.ISBN == isbn)
                .ToArray();
        }

        public Book[] GetAllByTittleOrAuthor(string titlePart)
        {
            return books
                .Where(book => book.Title.Contains(titlePart) || book.Author.Contains(titlePart))
                .ToArray();
        }
    }
}
