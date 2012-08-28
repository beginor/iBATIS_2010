using System;

namespace IBatisNet.DataMapper.Test.Domain 
{
	public class ImmutableCategory 
	{

		private int _id;
		private string _name;
		private Guid _guid;

		public ImmutableCategory(int id, string name, Guid guid) {

			_name = name;
			_id = id;
			_guid = guid;
		}
		
		public string Name {
			get { return _name; }
		}

		
		public int Id {
			get { return _id; }
		}

		
		public Guid Guid {
			get { return _guid; }
		}

	}
}
