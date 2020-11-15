using FoodMarketDMS.Business.Interfaces;
using System;
using System.Collections.Generic;

namespace FoodMarketDMS.Business.Models
{
    public class UserClass : BusinessBase, IStringArraySerializable
    {
        public static string[] PropertyNames { get; } = new string[] { "이름", "생년월일", "핸드폰 번호" };

        private readonly static HashSet<long> ID_CHECK = new HashSet<long>();
        private static long INDEX = 0;

        private string _name;
        private string _brith;
        private string _phoneNumber;

        #region Property

        public long Id { get; }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Birth
        {
            get { return _brith; }
            set { SetProperty(ref _brith, value); }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
        }

        #endregion Property

        #region Constructor

        /// <summary>
        /// Constructor for Add User
        /// </summary>
        public UserClass(string name, string birth, string phoneNumber)
        {
            while (ID_CHECK.Contains(INDEX))
            {
                INDEX++;
            }
            Id = INDEX++;

            if (Id > -1)
            {
                ID_CHECK.Add(Id);
            }

            Name = name;
            Birth = birth;
            PhoneNumber = phoneNumber;
        }

        /// <summary>
        /// Constructor for Load data from string[]
        /// </summary>
        public UserClass(string[] data)
        {
            INDEX++;
            Id = long.Parse(data[0]);
            if (!ID_CHECK.Add(Id))
            {
                throw new Exception("Identifier overlap exception!");
            }

            Name = data[1];
            Birth = data[2];
            PhoneNumber = data[3];
        }

        #endregion Constructor

        #region Method

        public string[] ToStringArray()
        {
            return new string[] { Id.ToString(), Name, Birth, PhoneNumber };
        }

        public string[] ExportStringArray()
        {
            return new string[] { Name, Birth, PhoneNumber };
        }

        #endregion Method
    }
}
