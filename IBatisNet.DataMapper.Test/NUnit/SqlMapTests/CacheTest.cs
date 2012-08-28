
using System;
using System.Collections;
using System.Threading;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.Configuration.Cache;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Test.Domain;
using NUnit.Framework;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for ParameterMapTest.
	/// </summary>
	[TestFixture] 
	public class CacheTest : BaseTest
	{
		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void SetUp() 
		{
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-init.sql" );
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-procedure.sql", false );
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void TearDown()
		{ /* ... */ } 

		#endregion

		#region Test cache

        [Test]
        public void LRU_cache_should_work()
        {
            IList list = sqlMap.QueryForList("GetLruCachedAccountsViaResultMap", null);

            int firstId = HashCodeProvider.GetIdentityHashCode(list);

            list = sqlMap.QueryForList("GetLruCachedAccountsViaResultMap", null);

            int secondId = HashCodeProvider.GetIdentityHashCode(list);

            Assert.AreEqual(firstId, secondId);

            list = sqlMap.QueryForList("GetLruCachedAccountsViaResultMap", null);

            int thirdId = HashCodeProvider.GetIdentityHashCode(list);

            Assert.AreEqual(firstId, thirdId);
        }

		/// <summary>
		/// Test for JIRA 29
		/// </summary>
		[Test] 
		public void TestJIRA28()
		{
			Account account = sqlMap.QueryForObject("GetNoAccountWithCache",-99) as Account;

			Assert.IsNull(account);
		}

        /// <summary>
        /// Cache error with QueryForObject<T>
        /// </summary>
        [Test]
        public void TestJIRA242WithNoCache()
        {
            Account account = sqlMap.QueryForObject<Account>("GetNoAccountWithCache", -99);
            account = sqlMap.QueryForObject<Account>("GetNoAccountWithCache", -99);

            Assert.IsNull(account);
        }

        /// <summary>
        /// Cache error with QueryForObject<T> with object in cache
        /// </summary>
        [Test]
        public void TestJIRA242WithCache()
        {
            Account account1 = sqlMap.QueryForObject<Account>("GetNoAccountWithCache", 1);
            AssertAccount1(account1);
            int firstId = HashCodeProvider.GetIdentityHashCode(account1);

            Account account2 = sqlMap.QueryForObject<Account>("GetNoAccountWithCache", 1);
            AssertAccount1(account2);

            int secondId = HashCodeProvider.GetIdentityHashCode(account2);

            Assert.AreEqual(firstId, secondId);
        }

        /// <summary>
        /// Cache error with QueryForObjectwith object in cache
        /// </summary>
        [Test]
        public void TestJIRA242_WithoutGeneric_WithCache()
        {
            Account account1 = sqlMap.QueryForObject("GetNoAccountWithCache", 1) as Account;
            AssertAccount1(account1);
            int firstId = HashCodeProvider.GetIdentityHashCode(account1);

            Account account2 = sqlMap.QueryForObject("GetNoAccountWithCache", 1) as Account;
            AssertAccount1(account2);

            int secondId = HashCodeProvider.GetIdentityHashCode(account2);

            Assert.AreEqual(firstId, secondId);
        }

	    /// <summary>
		/// Test Cache query
		/// </summary>
		/// <remarks>
		/// Used trace to see that the second query don't open an new connection
		/// </remarks>
        [Test]
        public void TestJIRA104()
		{
		    IList list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

		    int firstId = HashCodeProvider.GetIdentityHashCode(list);

		    list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

		    int secondId = HashCodeProvider.GetIdentityHashCode(list);

		    Assert.AreEqual(firstId, secondId);
		}
	    
		/// <summary>
		/// Test Cache query
		/// </summary>
		[Test] 
		public void TestQueryWithCache() 
		{
			IList list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			int firstId = HashCodeProvider.GetIdentityHashCode(list);

			list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			//Console.WriteLine(sqlMap.GetDataCacheStats());

			int secondId = HashCodeProvider.GetIdentityHashCode(list);

			Assert.AreEqual(firstId, secondId);

			Account account = (Account) list[1];
			account.EmailAddress  = "somebody@cache.com";
			sqlMap.Update("UpdateAccountViaInlineParameters", account);

			list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			int thirdId = HashCodeProvider.GetIdentityHashCode(list);

			Assert.IsTrue(firstId != thirdId);

			//Console.WriteLine(sqlMap.GetDataCacheStats());
		}


		/// <summary>
		/// Test flush Cache
		/// </summary>
		[Test] 
		public void TestFlushDataCache() 
		{
			IList list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			int firstId = HashCodeProvider.GetIdentityHashCode(list);

			list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			int secondId = HashCodeProvider.GetIdentityHashCode(list);

			Assert.AreEqual(firstId, secondId);

			sqlMap.FlushCaches();

			list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			int thirdId = HashCodeProvider.GetIdentityHashCode(list);

            Assert.AreNotEqual(firstId, thirdId);
		}

		[Test]
		public void TestFlushDataCacheOnExecute()
		{
			IList list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);
			int firstId = HashCodeProvider.GetIdentityHashCode(list);
			
		    list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);
			int secondId = HashCodeProvider.GetIdentityHashCode(list);
			Assert.AreEqual(firstId, secondId);
		    
			sqlMap.Update("UpdateAccountViaInlineParameters", list[0]);
			list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);
			int thirdId = HashCodeProvider.GetIdentityHashCode(list);
            Assert.AreNotEqual(firstId ,thirdId);
		}

		/// <summary>
		/// Test MappedStatement Query With Threaded Cache
		/// </summary>
		[Test]
		public void TestMappedStatementQueryWithThreadedCache() 
		{
			Hashtable results = new Hashtable();

			TestCacheThread.StartThread(sqlMap, results, "GetCachedAccountsViaResultMap");
			int firstId = (int) results["id"];

			TestCacheThread.StartThread(sqlMap, results, "GetCachedAccountsViaResultMap");
			int secondId = (int) results["id"];

			Assert.AreEqual(firstId, secondId);

			IList list = (IList) results["list"];

			Account account = (Account) list[1];
			account.EmailAddress = "new.toto@somewhere.com";
			sqlMap.Update("UpdateAccountViaInlineParameters", account);

			list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			int thirdId = HashCodeProvider.GetIdentityHashCode(list);

            Assert.AreNotEqual(firstId , thirdId);
		}

		/// <summary>
		/// Test MappedStatement Query With Threaded Read Write Cache
		/// </summary>
		[Test]
		public void TestMappedStatementQueryWithThreadedReadWriteCache()
		{
			Hashtable results = new Hashtable();

			TestCacheThread.StartThread(sqlMap, results, "GetRWCachedAccountsViaResultMap");
			int firstId = (int) results["id"];

			TestCacheThread.StartThread(sqlMap, results, "GetRWCachedAccountsViaResultMap");
			int secondId = (int) results["id"];

			Assert.AreNotEqual(firstId, secondId);

			IList list = (IList) results["list"];

			Account account = (Account) list[1];
			account.EmailAddress = "new.toto@somewhere.com";
			sqlMap.Update("UpdateAccountViaInlineParameters", account);

			list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			int thirdId = HashCodeProvider.GetIdentityHashCode(list);

			Assert.AreNotEqual(firstId, thirdId);
		}

		/// <summary>
		/// Test Cache Null Object
		/// </summary>
		[Test]
		public void TestCacheNullObject()
		{
			CacheModel cache = GetCacheModel();
			CacheKey key = new CacheKey();
			key.Update("testKey");

			cache[key] = null;

			object returnedObject = cache[key];
			Assert.AreEqual(CacheModel.NULL_OBJECT, returnedObject);
			Assert.AreEqual(HashCodeProvider.GetIdentityHashCode(CacheModel.NULL_OBJECT), HashCodeProvider.GetIdentityHashCode(returnedObject));
			Assert.AreEqual(1, cache.HitRatio);
		}


		/// <summary>
		/// Test CacheHit
		/// </summary>
		[Test]
		public void TestCacheHit() 
		{
			CacheModel cache = GetCacheModel();
			CacheKey key = new CacheKey();
			key.Update("testKey");

			string value = "testValue";
			cache[key] = value;

			object returnedObject = cache[key];
			Assert.AreEqual(value, returnedObject);
			Assert.AreEqual(HashCodeProvider.GetIdentityHashCode(value), HashCodeProvider.GetIdentityHashCode(returnedObject));
			Assert.AreEqual(1, cache.HitRatio);
		}

		/// <summary>
		/// Test CacheMiss
		/// </summary>
		[Test]
		public void TestCacheMiss() 
		{
			CacheModel cache = GetCacheModel();
			CacheKey key = new CacheKey();
			key.Update("testKey");

			string value = "testValue";
			cache[key] = value;

			CacheKey wrongKey = new CacheKey();
			wrongKey.Update("wrongKey");

			object returnedObject = cache[wrongKey];
			Assert.IsTrue(!value.Equals(returnedObject));
			Assert.IsNull(returnedObject) ;
			Assert.AreEqual(0, cache.HitRatio);
		}
		
		/// <summary>
		/// Test CacheHitMiss
		/// </summary>
		[Test]
		public void TestCacheHitMiss() 
		{
			CacheModel cache = GetCacheModel();
			CacheKey key = new CacheKey();
			key.Update("testKey");

			string value = "testValue";
			cache[key] = value;

			object returnedObject = cache[key];
			Assert.AreEqual(value, returnedObject);
			Assert.AreEqual(HashCodeProvider.GetIdentityHashCode(value), HashCodeProvider.GetIdentityHashCode(returnedObject));

			CacheKey wrongKey = new CacheKey();
			wrongKey.Update("wrongKey");

			returnedObject = cache[wrongKey];
			Assert.IsTrue(!value.Equals(returnedObject));
			Assert.IsNull(returnedObject) ;
			Assert.AreEqual(0.5, cache.HitRatio);
		}


		/// <summary>
		/// Test Duplicate Add to Cache
		/// </summary>
		/// <remarks>IBATISNET-134</remarks>
		[Test]
		public void TestDuplicateAddCache() 
		{
			CacheModel cache = GetCacheModel();
			CacheKey key = new CacheKey();
			key.Update("testKey");
			string value = "testValue";

			object obj = null;
			obj = cache[key];
			Assert.IsNull(obj);
			obj = cache[key];
			Assert.IsNull(obj);

			cache[key] = value;
			cache[key] = value;

			object returnedObject = cache[key];
			Assert.AreEqual(value, returnedObject);
			Assert.AreEqual(HashCodeProvider.GetIdentityHashCode(value), HashCodeProvider.GetIdentityHashCode(returnedObject));
		}

		private CacheModel GetCacheModel() 
		{
			CacheModel cache = new CacheModel();
			cache.FlushInterval = new FlushInterval();
			cache.FlushInterval.Minutes = 5;
			cache.Implementation = "IBatisNet.DataMapper.Configuration.Cache.Lru.LruCacheController, IBatisNet.DataMapper";
			cache.Initialize();

			return cache;
		}

		#endregion


		private class TestCacheThread
		{
			private ISqlMapper _sqlMap = null;
			private Hashtable _results = null;
			private string _statementName = string.Empty;

			public TestCacheThread(ISqlMapper sqlMap, Hashtable results, string statementName) 
			{
				_sqlMap = sqlMap;
				_results = results;
				_statementName = statementName;
			}

			public void Run() 
			{
				try 
				{
					IMappedStatement statement = _sqlMap.GetMappedStatement( _statementName );
                    ISqlMapSession session = new SqlMapSession(sqlMap);
					session.OpenConnection();
					IList list = statement.ExecuteQueryForList(session, null);

					//int firstId = HashCodeProvider.GetIdentityHashCode(list);

					list = statement.ExecuteQueryForList(session, null);
					int secondId = HashCodeProvider.GetIdentityHashCode(list);

					_results["id"] = secondId ;
					_results["list"] = list;
					session.CloseConnection();
				} 
				catch (Exception e) 
				{
					throw e;
				}
			}

			public static void StartThread(ISqlMapper sqlMap, Hashtable results, string statementName) 
			{
				TestCacheThread tct = new TestCacheThread(sqlMap, results, statementName);
				Thread thread = new Thread( new ThreadStart(tct.Run) );
				thread.Start();
				try 
				{
					thread.Join();
				} 
				catch (Exception e) 
				{
					throw e;
				}
			}
		}
	}
}
