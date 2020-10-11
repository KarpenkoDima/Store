using System;

namespace Store
{
    public class Book
    {
        //public string title;
        public string Title { get; }
        public int Id { get; }
        public Book(int id, string title)
        {
            this.Id = id;
            this.Title = title;
        }
    }
}
