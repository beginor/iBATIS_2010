using System;

namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// Description résumée de Book.
	/// </summary>
	public class Book : Document
	{
		private int _pageNumber = -1;

		public int PageNumber
		{
			get { return _pageNumber; }
			set { _pageNumber = value; }
		}
	}
}
