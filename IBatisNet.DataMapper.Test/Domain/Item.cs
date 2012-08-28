namespace IBatisNet.DataMapper.Test.Domain
{
    public class Item
    {
        private int _id;
        private string _status;
        private decimal _unitCost;
        private int _quantity;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public decimal UnitCost
        {
            get { return _unitCost; }
            set { _unitCost = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
    }
}
