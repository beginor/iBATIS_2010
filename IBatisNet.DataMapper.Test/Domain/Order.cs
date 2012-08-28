using System;
using System.Collections;
#if dotnet2
using System.Collections.Generic;
#endif

namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// Description résumée de Order.
	/// </summary>
	[Serializable]
	public class Order
	{
		private int _id;
		private Account _account;
		private DateTime _date;
		private string _cardType;
		private string _cardNumber;
		private string _cardExpiry;
		private string _street;
		private string _city;
		private string _province;
		private string _postalCode;
		private IList _lineItemsIList ;//= new ArrayList();
        private LineItemCollection _collection;
		private LineItem[] _lineItemsArray;
		private LineItem _favouriteLineItem;

		public Order()
		{}

		public Order(int id, Account account)
		{
			_id = id;
			_account = account;
		}

		public Order(int id, Account account, IList lineItems)
		{
			_id = id;
			_account = account;
			_lineItemsIList = lineItems;
		}

		public Order(int id, Account account, LineItem[] lineItemsArray)
		{
			_id = id;
			_account = account;
			_lineItemsArray = lineItemsArray;
		}

		public Order(int id, Account account, LineItemCollection collection)
		{
			_id = id;
			_account = account;
            _collection = collection;
		}


#if dotnet2
        private IList<LineItem> _genericList;
        public IList<LineItem> LineItemsGenericList
        {
            get { return _genericList; }
            set { _genericList = value; }
        }

        private LineItemCollection2 _genericCollection = null;
        public LineItemCollection2 LineItemsCollection2
        {
            get { return _genericCollection; }
            set { _genericCollection = value; }
        }

        public Order(IList<LineItem> lineItems)
        {
            _genericList = lineItems;
        }

        public Order(LineItemCollection2 collection)
        {
            _genericCollection = collection;
        }
#else
        private IList _genericList;
        public IList LineItemsGenericList
        {
            get { return _genericList; }
            set { _genericList = value; }
        }

		public LineItemCollection LineItemsCollection2
		{
			get { return _collection; }
			set { _collection = value; }
		}

        public Order(IList lineItems)
        {
            _genericList = lineItems;
        }

        public Order(LineItemCollection collection)
        {
            _collection = collection;
        }
#endif

        public LineItem FavouriteLineItem
		{
			get { return _favouriteLineItem; }
			set { _favouriteLineItem = value; }
		}

		public IList LineItemsIList
		{
			get { return _lineItemsIList; }
			set { _lineItemsIList = value; }
		}


        public LineItemCollection LineItemsCollection
		{
            get { return _collection; }
            set { _collection = value; }
		}

		public LineItem[] LineItemsArray
		{
			get { return _lineItemsArray; }
			set { _lineItemsArray = value; }
		}

		public string PostalCode
		{
			get { return _postalCode; }
			set { _postalCode = value; }
		}

		public string Province
		{
			get { return _province; }
			set { _province = value; }
		}

		public string City
		{
			get { return _city; }
			set { _city = value; }
		}

		public string Street
		{
			get { return _street; }
			set { _street = value; }
		}

		public string CardExpiry
		{
			get
			{
				return _cardExpiry; 
			}
			set
			{ 
				_cardExpiry = value; 
			}
		}

		public string CardNumber
		{
			get
			{
				return _cardNumber; 
			}
			set
			{ 
				_cardNumber = value; 
			}
		}

		public string CardType
		{
			get
			{
				return _cardType; 
			}
			set
			{ 
				_cardType = value; 
			}
		}

		public virtual Account Account
		{
			get { return _account; }
			set { _account = value; }
		}

		public int Id
		{
			get
			{
				return _id; 
			}
			set
			{ 
				_id = value; 
			}
		}

		public DateTime Date
		{
			get
			{
				return _date; 
			}
			set
			{ 
				_date = value; 
			}
		}

		public DateTime OrderDate {
			get {
				return _date; 
			}
			set { 
				_date = value; 
			}
		}
	}
}
