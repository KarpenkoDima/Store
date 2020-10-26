using System;
using System.Collections.Generic;

namespace Store.Contractors
{
    public class Form
    {
        public string Code { get; }
        public int OrderId { get; }
        public int Step { get; }
        public bool IsFinal { get; }
        public IReadOnlyList<Field> Fields { get; }

        public Form(string code, int orderId, int step, bool isFinal, IReadOnlyList<Field> fields)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            OrderId = orderId;
            Step = step >= 1 ? step: throw new ArgumentOutOfRangeException(nameof(step));
            IsFinal = isFinal;
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
        }
    }
}