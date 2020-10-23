using System;
using System.Text.RegularExpressions;

namespace Store
{
    public class Book
    {
        public string ISBN { get; }
        public string Author { get; }

        //public string title;
        public string Title { get; }
        public int Id { get; }
        public string Desscription { get; }
        public decimal Price { get; }
        public Book(int id, string isbn, string authors, string title, string description, decimal price)
        {
            this.Id = id;
            this.ISBN = isbn;
            this.Author = authors;
            this.Title = title;
            this.Desscription = description;
            this.Price = price;
        }
        internal static bool IsIsbn(string query)
        {
            if (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query))
            {
                return false;
            }

            query = query.Replace("-", "")
                        .Replace(" ", "")
                        .ToUpper();
            return Regex.IsMatch(query, "^ISBN\\d{10}(\\d{3})?$");
        }
    }
}
