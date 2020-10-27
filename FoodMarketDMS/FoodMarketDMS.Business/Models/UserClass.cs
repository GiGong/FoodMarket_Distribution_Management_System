using System;
using System.Collections.Generic;

namespace FoodMarketDMS.Business.Models
{
    public class UserClass : BusinessBase, IStringArraySerializable
    {
        private readonly static HashSet<uint> ID_CHECK = new HashSet<uint>();
        private static uint INDEX = 0;

        private string _name;
        private string _phoneNumber;
        private string _brith;
        private string _address;

        #region Property

        public uint Id { get; }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
        }

        public string Birth
        {
            get { return _brith; }
            set { SetProperty(ref _brith, value); }
        }

        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        #endregion Property

        #region Constructor

        /// <summary>
        /// Constructor for Add User
        /// </summary>
        public UserClass(string name, string phoneNumber, string birth, string address)
        {
            while (ID_CHECK.Contains(INDEX))
            {
                INDEX++;
            }
            Id = INDEX++;
            ID_CHECK.Add(Id);

            Name = name;
            PhoneNumber = phoneNumber;
            Birth = birth;
            Address = address;
        }

        /// <summary>
        /// Constructor for Load data from string[]
        /// </summary>
        public UserClass(string[] data)
        {
            INDEX++;
            Id = uint.Parse(data[0]);
            if (!ID_CHECK.Add(Id))
            {
                throw new Exception("Identifier overlap exception!");
            }

            Name = data[1];
            PhoneNumber = data[2];
            Birth = data[3];
            Address = data[4];
        }

        #endregion Constructor

        #region Method

        public string[] ToStringArray()
        {
            return new string[] { Id.ToString(), Name, PhoneNumber, Birth, Address };
        }

        #endregion Method
    }
}
