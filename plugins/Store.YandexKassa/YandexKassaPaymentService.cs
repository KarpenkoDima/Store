using store;
using store.store;
using Store.Contractors;
using Store.Web.Contractors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.YandexKassa
{
    public class YandexKassaPaymentService : IPaymentService, IWebContractorService
    {
        public string Code => "YandexKassa";

        public string Title => "Yandex Kassa: Payment bank card";

        public string GetUri => "/YandexKassa/";

        public Form CreateForm(Order order)
        {
            return new Form(Code, order.Id, 1, true, new Field[0]);
        }

        public OrderPayment GetPayment(Form formDelivery)
        {
            return new OrderPayment(Code, "Payment bank card", new Dictionary<string, string>());
        }

        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> value)
        {
            return new Form(Code, orderId, 2, true, new Field[0]);
        }
    }
}
