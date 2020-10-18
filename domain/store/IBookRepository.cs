using Store;
using System;
using System.Collections.Generic;
using System.Text;

namespace store
{
    public interface IBookRepository
    {
        Book[] GetByTitle(string titlePart);
    }
}
