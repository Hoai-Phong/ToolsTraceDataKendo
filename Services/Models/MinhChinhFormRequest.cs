using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vietlott.Services.Models
{
    public class MinhChinhFormRequest
    {
        public DateTime Date { get; set; } = default!;
        public string Ky { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public int Page { get; set; }

        public IList<KeyValuePair<string, string>> ToHtmlFormData()
        {
            var list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("date", Date.ToString("dd-MM-yyyy")));
            list.Add(new KeyValuePair<string, string>("ky", Ky));
            list.Add(new KeyValuePair<string, string>("number", Number));
            list.Add(new KeyValuePair<string, string>("page", Page.ToString()));
            return list;
        }
            

    }
}
