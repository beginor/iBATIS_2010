using System;
using System.Collections;

#if dotnet2
using System.Collections.Generic;
#endif

namespace IBatisNet.DataMapper.Test.Domain.Petshop
{
    /// <summary>
    /// Business entity used to model a product
    /// </summary>
    [Serializable]
    public class Product
    {

        #region Private Fields
        private string _id;
        private string _name;
        private string _description;
        private Category _category;
        private IList _items = new ArrayList();

        #endregion

#if dotnet2
        private IList<Item> _genericList = new List<Item>();
        public IList<Item> GenericItems
        {
            get { return _genericList; }
            set { _genericList = value; }
        }
#else
        private IList _genericList;
        public IList GenericItems
        {
            get { return _genericList; }
            set { _genericList = value; }
        }        
#endif

        #region Properties

        public IList Items
        {
            get { return _items; }
            set { _items = value; }
        }
        
        public string Id
        {
            set { _id = value; }
            get { return _id; }
        }

        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }

        public Category Category
        {
            set { _category = value; }
            get { return _category; }
        }
        #endregion

    }
}