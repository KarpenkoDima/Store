using Store;
using System;
using System.Collections.Generic;
using System.Text;

namespace store
{
    public class BookService
    {
        readonly IBookRepository bookRepository;

        public BookService(IBookRepository repository)
        {
            this.bookRepository = repository;
        }
        public Book[] GetAllByQuery(string query)
        {
            if (Book.IsIsbn(query))
            {
                return bookRepository.GetAllByISBN(query);
            }
            else
            {
                return bookRepository.GetAllByTittleOrAuthor(query);
            }
        }

        
    }
}
