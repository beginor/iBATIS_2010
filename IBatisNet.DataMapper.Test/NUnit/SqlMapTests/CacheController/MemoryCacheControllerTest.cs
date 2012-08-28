using IBatisNet.DataMapper.Configuration.Cache;
using IBatisNet.DataMapper.Configuration.Cache.Memory;
using NUnit.Framework;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests.CacheController
{
	/// <summary>
	/// Description résumée de MemoryCacheControllerTest.
	/// </summary>
	[TestFixture]
	public class MemoryCacheControllerTest: LruCacheControllerTest
	{

		protected override ICacheController GetController() 
		{
			return new MemoryCacheControler();
		}

		[Test]
		public override void TestSizeOne() 
		{
			// This is not relevant for this model
		}
	}
}
