﻿using store;
using Store;
using System;
using System.Linq;

namespace StoreMemory
{
    public class BookRepository : IBookRepository
    {
        readonly Book[] books = new[] { new Book(1, "Art of Programming"), new Book(2, "Refactoring"), new Book(3, "C Programming Language") };

        public Book[] GetByTitle(string titlePart)
        {
            return books
                .Where(book => book.Title.Contains(titlePart))
                .ToArray();
        }
    }
}
