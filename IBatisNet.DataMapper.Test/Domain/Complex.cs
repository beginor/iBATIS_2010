using System;
using System.Collections;

namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// Description résumée de Complex.
	/// </summary>
	public class Complex
	{
		private Hashtable _map = null;

		public Hashtable Map 
		{
			get
			{
				return _map;
			}
			set
			{
				_map = value;
			}
		}

	}
}
