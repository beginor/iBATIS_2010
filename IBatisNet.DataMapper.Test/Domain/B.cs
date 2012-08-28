using System;

namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// Summary description for B.
	/// </summary>
	public class B 
	{
		private C _c;
		private D _d;
		private string _id;
		private string _libelle;

		public string Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public string Libelle
		{
			get { return _libelle; }
			set { _libelle = value; }
		}

		public C C
		{
			get { return _c; }
			set { _c = value; }
		}


		public D D
		{
			get { return _d; }
			set { _d = value; }
		}
	}
}
