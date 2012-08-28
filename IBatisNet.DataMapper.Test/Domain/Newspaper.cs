using System;

namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// Description résumée de Newspaper.
	/// </summary>
	public class Newspaper : Document
	{
		private string _city = string.Empty;

		public string City
		{
			get { return _city; }
			set { _city = value; }
		}
	}
}
