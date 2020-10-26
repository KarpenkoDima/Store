using Store.Contractors;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace store.Contractors
{
    public class PostamateDeliveryService : IDeliveryService
    {
        private static IReadOnlyDictionary<string, string> cities = new Dictionary<string, string>
        {
            {"1", "Moscow" },
            {"2", "St. Petersberg" }

        };
        private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> postamates = new Dictionary<string, IReadOnlyDictionary<string, string>>
        {
            {
                "1",
                new Dictionary<string, string>
                {
                    { "1", "Kazan Station"},
                    { "2", "Kiev Station"},
                    { "3", "Belarusskiy Station"},                    
                }
            },
            {
                "2",
                new Dictionary<string, string>
                {
                    { "4", "Moscow Station"},
                    { "5", "Finland Station"},
                    { "6", "Vitebsk Station"},
                }
            }
        };
        public string Code => "Postamate";
        public string Title => "Delivery from Moscow to St.Petersberg";
       
        public Form CreateForm(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            return new Form(Code, order.Id, 1, false, new[]
            {
                new SelectionField("City","city","1",cities)
            }) ; 
        }      
        public Form MoveNext(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            if (step == 1)
            {
                if (values["city"].Equals("1"))
                {
                    return new Form(Code, orderId, 2, false, new Field[]
                    {
                        new HiddenField("City", "city", "1"),
                        new SelectionField("Postamate", "postamate", "1", postamates["1"])
                    });
                }
                else if (values["city"].Equals("2"))
                {
                    return new Form(Code, orderId, 2, false, new Field[]
                    {
                        new HiddenField("City", "city", "2"),
                        new SelectionField("Postamate", "postamate", "4",  postamates["2"])
                    });
                }
                else
                {
                    throw new InvalidOperationException("Invalid postamate");
                }
            }
            else
            {
                if (step == 2)
                {
                    return new Form(Code, orderId, 3, true, new Field[]
                   {
                            new HiddenField("City", "city", values["city"]),
                            new HiddenField("Postamate", "postamate", values["postamate"])
                   });
                }
                else
                {
                    throw new InvalidOperationException("Invalid postamate");
                }
            }           
        }
    }
}
