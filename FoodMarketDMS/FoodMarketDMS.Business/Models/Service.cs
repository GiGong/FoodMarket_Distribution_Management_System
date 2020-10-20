using System;
using System.Collections.Generic;

namespace FoodMarketDMS.Business.Models
{
    public class Service : BusinessBase
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
    }
}
