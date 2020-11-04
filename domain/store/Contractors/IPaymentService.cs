using store;
using store.store;
using System.Collections.Generic;

namespace Store.Contractors
{
    public interface IPaymentService
    {
        string Code { get; }
        string Title { get; }
        Form CreateForm(Order order);
        Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> value);
        OrderPayment GetPayment(Form formDelivery);
    }
}
