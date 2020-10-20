using System.Collections.Generic;

namespace FoodMarketDMS.Business.Models
{
    public class Offer : BusinessBase
    {
        private Person _user;
        private List<string> _products;
        private List<Service> _services;

        public Person User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public List<string> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        public List<Service> Services
        {
            get { return _services; }
            set { SetProperty(ref _services, value); }
        }
    }
}
