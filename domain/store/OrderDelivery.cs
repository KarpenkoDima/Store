using System;
using System.Collections.Generic;
using System.Text;

namespace store
{
    public class OrderDelivery
    {
        public string Code { get; }
        public string Description { get; }
        public decimal Amount { get; set; }
        public IReadOnlyDictionary<string, string> parametrs { get; }

        public OrderDelivery(string code, string description, IReadOnlyDictionary<string, string> parametrs, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException(nameof(code));
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException(nameof(description));
            }
            if (parametrs == null)
            {
                throw new ArgumentNullException(nameof(parametrs));
            }
            Code = code;
            Description = description;
            this.parametrs = parametrs;
            Amount = amount;
        }
    }
}
