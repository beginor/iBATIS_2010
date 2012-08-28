

using System.Collections;
#if dotnet2
using System.Collections.Generic;
#endif

namespace IBatisNet.DataMapper.Test.Domain
{
    public class Coupon
    {
        private int id;
        private string _code;

        public virtual int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

#if dotnet2
        private IList<int> _brandIds = new List<int>();
         
        public IList<int> BrandIds
        {
            get { return _brandIds; }
            set { _brandIds = value; }
        }
#else
        private IList _brandIds = new List();
         
        public IList BrandIds
        {
            get { return _brandIds; }
            set { _brandIds = value; }
        }
#endif
    }
}
