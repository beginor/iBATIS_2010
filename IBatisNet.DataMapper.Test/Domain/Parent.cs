
using System.Collections.Generic;

namespace IBatisNet.DataMapper.Test.Domain

{
    public class Parent
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private byte[] _rowVersion;
        public byte[] RowVersion
        {
            get { return _rowVersion; }
            set { _rowVersion = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private IList<Child> _children;
        public IList<Child> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}
