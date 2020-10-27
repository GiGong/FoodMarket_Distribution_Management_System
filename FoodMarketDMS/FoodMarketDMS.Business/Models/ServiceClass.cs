using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodMarketDMS.Business.Models
{
    public class ServiceClass : BusinessBase, IStringArraySerializable
    {
        private DateTime _date;
        private List<string> _products;

        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        public List<string> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        public ServiceClass(DateTime date, List<string> products)
        {
            Date = date;
            Products = products;
        }

        public ServiceClass(string[] data)
        {
            Date = new DateTime(long.Parse(data[0]));
            Products = new List<string>(data.Skip(1));
        }

        public string[] ToStringArray()
        {
            var result = new string[Products.Count + 1];
            result[0] = Date.Ticks.ToString();
            Products.CopyTo(result, 1);
            return result.ToArray();
        }
    }
}
