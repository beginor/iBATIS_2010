using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Text;

namespace IBatisNet.DataMapper.Test.Domain
{
    [Serializable]
    public class LineItemCollection2 : Collection<LineItem>
    {
        public virtual new int Count
        {
            get { return base.Count; }
        }

        public virtual new int IndexOf(LineItem item)
        {
            return base.IndexOf(item);
        }
    }
}
