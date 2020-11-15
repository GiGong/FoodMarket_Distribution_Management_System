using FoodMarketDMS.Business.Interfaces;
using System;

namespace FoodMarketDMS.Business.Models
{
    public class ServiceClass : BusinessBase, IStringArraySerializable
    {
        public static string[] PropertyNames { get; } = new string[] { "날짜", "품목", "비고" };

        private DateTime _date;
        private string _products;
        private string _note;

        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        public string Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        public string Note
        {
            get { return _note; }
            set { SetProperty(ref _note, value); }
        }

        public ServiceClass(DateTime date, string products, string note)
        {
            Date = date;
            Products = products;
            Note = note;
        }

        public ServiceClass(string[] data)
        {
            Date = new DateTime(long.Parse(data[0]));
            Products = data[1];
            Note = data[2];
        }

        public string[] ToStringArray()
        {
            return new string[] { Date.Ticks.ToString(), Products, Note };
        }

        public string[] ExportStringArray()
        {
            return new string[] { Date.Date.ToString("yyyy년 M월 d일"), Products, Note };
        }

    }
}
