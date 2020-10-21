using System;
using System.Collections.Generic;

namespace FoodMarketDMS.Business.Models
{
    public class OfferClass : BusinessBase
    {
        
        private DateTime _date;
        private UserClass _user;
        private string _userName;
        private string _provider;
        private List<string> _products;
        private List<string> _services;


        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        public UserClass User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
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

        public OfferClass() { }

        public OfferClass(DateTime date, UserClass user, string userName, string provider, List<string> products, List<string> services)
        {
            Date = date;
            User = user;
            UserName = userName;
            Provider = provider;
            Products = products;
            Services = services;
        }

    }
}
