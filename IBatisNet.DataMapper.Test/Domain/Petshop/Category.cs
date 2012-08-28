using System;
using System.Collections;

#if dotnet2
using System.Collections.Generic;
#endif

namespace IBatisNet.DataMapper.Test.Domain.Petshop
{
    /// <summary>
    /// Business entity used to model category
    /// </summary>
    [Serializable]
    public class Category
    {
        private string _Id;
        private string _name;
        private string _description;
        private IList _products = new ArrayList();
        
#if dotnet2
        private IList<Product> _genericList;
        public IList<Product> GenericProducts
        {
            get { return _genericList; }
            set { _genericList = value; }
        }
#else
        private IList _genericList;
        public IList GenericProducts
        {
            get { return _genericList; }
            set { _genericList = value; }
        }
#endif

        #region Properties
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public IList Products
        {
            get { return _products; }
            set { _products = value; }
        }
        #endregion
    }
}