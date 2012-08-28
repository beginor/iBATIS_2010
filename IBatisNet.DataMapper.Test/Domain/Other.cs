namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// Other.
	/// </summary>
	public class Other
	{
		private int _int;
		private long _long;
		private bool _bool = false;
		private bool _bool2 = false;

		public bool Bool2
		{
			get { return _bool2; }
			set { _bool2 = value; }
		}

		public bool Bool
		{
			get { return _bool; }
			set { _bool = value; }
		}

		public int Int
		{
			get { return _int; }
			set { _int = value; }
		}

		public long Long
		{
			get { return _long; }
			set { _long = value; }
		}
	}
}
