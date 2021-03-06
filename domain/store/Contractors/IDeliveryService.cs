﻿using store;
using System.Collections.Generic;

namespace Store.Contractors
{
    public interface IDeliveryService
    {
        string Code { get; }
        string Title { get; }
        Form CreateForm(Order order);
        Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> value);
        OrderDelivery CreateDelivery(Form formDelivery);
    }
}
