using System.Collections;

namespace IBatisNet.DataMapper.Test.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class Product
    {
        private int _id;
        private string _name;
        private IList _items = new ArrayList();

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
        
        public IList Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }
}
