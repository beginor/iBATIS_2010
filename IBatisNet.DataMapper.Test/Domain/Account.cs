using System;
using System.Collections;
#if dotnet2
using System.Collections.Generic;
#endif

namespace IBatisNet.DataMapper.Test.Domain
{
    public interface IAccount
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string EmailAddress { get; set; }
    }

    public class BaseAccount : IAccount
    {
        private int id;
        private string firstName;
        private string lastName;
        private string emailAddress;

        #region IAccount Members

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        #endregion
    }

	/// <summary>
	/// Description résumée de Account.
	/// </summary>
	[Serializable]
	public class Account
	{
		private int id;
		private string _firstName;
		private string _lastName;
		private string _emailAddress;
		private int[] _ids = null;
		private bool _bannerOption = false;
		private bool _cartOption = false;
	    private Document _document = null;

        #if dotnet2
        protected IList<Document> documents = new List<Document>();

        public IList<Document> Documents
        {
            get { return documents; }
        }
        #else
        protected IList documents = new ArrayList();

        public IList Documents
        {
            get { return documents; }
        }
        #endif

        public Account()
		{}

        public Account(int identifiant, string firstName, string lastName)
		{
            id = identifiant;
			_firstName = firstName;
			_lastName = lastName;
		}

        public Account(int identifiant, string firstName, string lastName, Document document)
        {
            id = identifiant;
            _firstName = firstName;
            _lastName = lastName;
            _document = document;
        }

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		public string FirstName
		{
			get { return _firstName; }
			set { _firstName = value; }
		}

		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; }
		}

		public string EmailAddress
		{
			get { return _emailAddress; }
			set { _emailAddress = value; }
		}

		public int[] Ids
		{
			get { return _ids; }
			set { _ids = value; }
		}

		public bool BannerOption
		{
			get { return _bannerOption; }
			set { _bannerOption = value; }
		}

		public bool CartOption
		{
			get { return _cartOption; }
			set { _cartOption = value; }
		}

        public Document Document
        {
            get { return _document; }
            set { _document = value; }
        }
	}
}
