using store.store;
using Store.Contractors;
using System;
using System.Collections.Generic;

namespace store.Contractors
{
    public class CashPaymentService : IPaymentService
    {
        public string Code => "Cash";

        public string Title => "Cash Payment";

        public Form CreateForm(Order order)
        {
            return new Form(Code, order.Id, 1, false, new Field[0]);
        }

        public OrderPayment GetPayment(Form formDelivery)
        {
            if (formDelivery.Code != Code || !formDelivery.IsFinal)
            {
                throw new InvalidOperationException("Invalid payment form");

            }
            return new OrderPayment(Code, "Cash payment", new Dictionary<string, string>());
        }

        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> value)
        {
            if (step!=1)
            {
                throw new InvalidOperationException("Invalid step cash");
            }

            return new Form(Code, orderId, 2, true, new Field[0]);
        }
    }
}
