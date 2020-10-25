using System.Collections.Generic;

namespace Store.Web.Model
{
    public class ConfirmatiopnModel
    {
        public int OrderId { get; set; }
        public string CellPhone { get; set; }
        public IDictionary<string, string> Errors { get; internal set; } = new Dictionary<string, string>();
    }
}