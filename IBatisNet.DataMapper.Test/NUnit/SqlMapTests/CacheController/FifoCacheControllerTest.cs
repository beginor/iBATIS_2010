using IBatisNet.DataMapper.Configuration.Cache;
using IBatisNet.DataMapper.Configuration.Cache.Fifo;
using NUnit.Framework;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests.CacheController
{
	/// <summary>
	/// Description résumée de FifoCacheControllerTest.
	/// </summary>
	[TestFixture]
	public class FifoCacheControllerTest : LruCacheControllerTest
	{

		protected override ICacheController GetController() 
		{
			return new FifoCacheController();
		}
	}
}
