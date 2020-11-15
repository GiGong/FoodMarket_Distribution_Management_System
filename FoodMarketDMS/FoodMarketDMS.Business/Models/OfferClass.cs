using FoodMarketDMS.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodMarketDMS.Business.Models
{
    public class OfferClass : BusinessBase, IStringArraySerializable
    {
        public static string[] PropertyNames { get; } = new string[] { "날짜", "사용자 이름", "사용자 생년월일", "제공 처리자", "품목", "서비스 품목" };

        private DateTime _date;
        private UserClass _user;
        private long _userId;
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

        public long UserId
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


        public OfferClass(DateTime date, UserClass user, string provider, List<string> products, List<string> services)
        {
            Date = date;
            User = user;
            UserId = user.Id;
            UserName = user.Name;
            Provider = provider;
            Products = products;
            Services = services;
        }

        public OfferClass(string[] data, UserClass user)
        {
            Date = new DateTime(long.Parse(data[0]));
            UserId = long.Parse(data[1]);
            UserName = data[2];
            Provider = data[3];
            Products = data[4].Split('\n').ToList();
            Services = data[5].Split('\n').ToList();

            User = user;
        }

        public string[] ToStringArray()
        {
            string productsString = string.Join("\n", Products);
            string servicesString = string.Join("\n", Services);

            return new string[] { Date.Ticks.ToString(), UserId.ToString(), UserName, Provider, productsString, servicesString };
        }

        public string[] ExportStringArray()
        {
            string productsString = string.Join(", ", Products);
            string servicesString = string.Join(", ", Services);

            return new string[] { Date.ToString("yyyy년 M월 d일 HH시 mm분"), User.Name, User.Birth, Provider, productsString, servicesString };
        }

        public static long GetUserIdFromStringData(string[] data)
        {
            return long.Parse(data[1]);
        }
    }
}
