

namespace IBatisNet.DataMapper.Test.Domain
{
    public class Child
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

        private int _parentId;
        public int ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}
