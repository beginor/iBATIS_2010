using System;

namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// Summary for Document.
	/// </summary>
    [Serializable]
	public class Document
	{
		private int _id = -1;
		private string _title = string.Empty;

		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}
		
		public string Test
		{
			set { _title = value; }
		}
		
	}
}
