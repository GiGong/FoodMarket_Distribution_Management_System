namespace FoodMarketDMS.Business.Models
{
    public class UserClass : BusinessBase
    {
        private string _name;
        private string _phoneNumber;
        private string _brith;
        private string _address;

        #region Constructor

        public UserClass() { }

        public UserClass(string name, string phoneNumber, string birth, string address)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Birth = birth;
            Address = address;
        }

        #endregion Constructor

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
    }
}
