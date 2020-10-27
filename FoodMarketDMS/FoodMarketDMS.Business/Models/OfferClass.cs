using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodMarketDMS.Business.Models
{
    public class OfferClass : BusinessBase, IStringArraySerializable
    {

        private DateTime _date;
        private uint _userId;
        private string _userName;
        private string _provider;
        private List<string> _products;
        private List<string> _services;


        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        public uint UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        public string Provider
        {
            get { return _provider; }
            set { SetProperty(ref _provider, value); }
        }

        public List<string> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        public List<string> Services
        {
            get { return _services; }
            set { SetProperty(ref _services, value); }
        }


        public OfferClass(DateTime date, uint userId, string userName, string provider, List<string> products, List<string> services)
        {
            Date = date;
            UserId = userId;
            UserName = userName;
            Provider = provider;
            Products = products;
            Services = services;
        }

        public OfferClass(string[] data)
        {
            Date = new DateTime(long.Parse(data[0]));
            UserId = uint.Parse(data[1]);
            UserName = data[2];
            Provider = data[3];
            Products = data[4].Split('\n').ToList();
            Services = data[5].Split('\n').ToList();
        }

        public string[] ToStringArray()
        {
            string productsString = string.Join("\n", Products);
            string servicesString = string.Join("\n", Services);

            return new string[] { Date.Ticks.ToString(), UserId.ToString(), UserName, Provider, productsString, servicesString };
        }

    }
}
