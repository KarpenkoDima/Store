using store;
using Store;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreMemory
{
    public class BookRepository : IBookRepository
    {
        readonly Book[] books = new[] 
        { new Book(1,"ISBN 12345-67891", "Donald Knut", "Art of Programming", "", 10.0m),
            new Book(2, "ISBN 11111-11111", "M. Fowler", "Refactoring", "", 12.12m), 
            new Book(3, "ISBN 09876-54321", "B. Kernigan", "C Programming Language", "", 21.34m) 
        };

        public Book[] GetAllByIds(IEnumerable<int> bookIds)
        {
            var foundBooks = (from book in books
                             join bookId in bookIds on book.Id equals bookId
                             select book).ToArray();

            return books.Where(book => bookIds.Contains(book.Id)).ToArray();
        }

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

        public Book GetById(int id)
        {
            return books.Single(book => book.Id == id);
        }
    }
}
