using System;
using System.Text;

namespace IBatisNet.DataMapper.Test.Domain
{
    [Serializable]
    public class Simple
    {
        private string _name = string.Empty;
        private string _address = string.Empty;
        private int _count = int.MinValue;
        private DateTime _date = new DateTime(2006, 03, 06, 12, 00, 00, 00);
        private decimal _pay = 78.78M;
        private int _id = 0;


        public Simple() { }

        public void Init()
        {
            _name = "Someone with along name";
            _address = "1234 some street, some city, victoria, 3000, austaya";
            _count = 69;
            _date = DateTime.Now;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public decimal Pay
        {
            get { return _pay; }
            set { _pay = value; }
        }
    }
}
