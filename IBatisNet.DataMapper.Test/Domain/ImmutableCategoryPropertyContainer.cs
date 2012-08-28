
namespace IBatisNet.DataMapper.Test.Domain {
	
	/// <summary>
	/// A Container object that stores an immutable category as a property.
	/// 
	/// This container will be used to test constructor injection on an immutable category, when it's
	/// resultmapped through a another object's constructor.
	/// </summary>
	public class ImmutableCategoryPropertyContainer 
	{
		private ImmutableCategory _immutableCategory;

		public ImmutableCategory ImmutableCategory {
			get { return _immutableCategory; }
			set { _immutableCategory = value; }
		}
	}
}
