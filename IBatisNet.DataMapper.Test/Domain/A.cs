using System;

namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// Summary description for A.
	/// </summary>
	public class A
	{
		private string _id;
		private string _libelle;
		private B _b;
		private E _e;
		private F _f;

		public B B
		{
			get { return _b; }
			set { _b = value; }
		}

		public E E
		{
			get { return _e; }
			set { _e = value; }
		}

		public F F
		{
			get { return _f; }
			set { _f = value; }
		}

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
	}
}
