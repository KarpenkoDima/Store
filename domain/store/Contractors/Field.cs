using System.Collections.Generic;
using System.Diagnostics;

namespace Store.Contractors
{
    public abstract class Field
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        protected Field(string label, string name, string value)
        {
            Label = label;
            Name = name;
            Value = value;
        }
    }

    public class HiddenField : Field
    {
        public HiddenField(string label, string name, string value)
            :base(label, name, value)
        {

        }
    }
    public class SelectionField : Field
    {
        public IReadOnlyDictionary<string, string> Items { get; }
        public SelectionField(string label, string name, string value, IReadOnlyDictionary<string, string> items)
            : base(label, name, value)
        {
            Items = items;
        }
    }
}