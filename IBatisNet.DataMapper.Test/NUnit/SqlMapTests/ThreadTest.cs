using System;
using System.Collections;
using System.Threading;
using System.Configuration;

using NUnit.Framework;

using IBatisNet.DataMapper.Exceptions;

using IBatisNet.DataMapper.Test;
using IBatisNet.DataMapper.Test.Domain;

using log4net;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for TransactionTest.
	/// </summary>
	[TestFixture] 
	public class ThreadTest: BaseTest
	{
		private static readonly ILog _logger = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private static int _numberOfThreads = 10;
		private ManualResetEvent  _startEvent = new ManualResetEvent(false);
		private ManualResetEvent  _stopEvent = new ManualResetEvent(false);

		#region SetUp & TearDown

		/// <summary>
		/// SetUp 
		/// </summary>
		[SetUp] 
		public void Init() 
		{
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-init.sql" );
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Thread test

		[Test]
		public void TestCommonUsageMultiThread()
		{
			const int threadCount = 10;

			Thread[] threads = new Thread[threadCount];
			
			for(int i = 0; i < threadCount; i++)
			{
				threads[i] = new Thread(new ThreadStart(ExecuteMethodUntilSignal));
				threads[i].Start();
			}

			_startEvent.Set();

			Thread.CurrentThread.Join(1 * 2000);

			_stopEvent.Set();
		}

		public void ExecuteMethodUntilSignal()
		{
			_startEvent.WaitOne(int.MaxValue, false);

			while (!_stopEvent.WaitOne(1, false))
			{
				Assert.IsFalse(sqlMap.IsSessionStarted);

				Console.WriteLine("Begin Thread : " + Thread.CurrentThread.GetHashCode());

				Account account = (Account) sqlMap.QueryForObject("GetAccountViaColumnIndex", 1);
				
				Assert.IsFalse(sqlMap.IsSessionStarted);
				
				Assert.AreEqual(1, account.Id, "account.Id");
				Assert.AreEqual("Joe", account.FirstName, "account.FirstName");
				Assert.AreEqual("Dalton", account.LastName, "account.LastName");

				Console.WriteLine("End Thread : " + Thread.CurrentThread.GetHashCode());
			}
		}

		/// <summary>
		/// Test BeginTransaction, CommitTransaction
		/// </summary>
		[Test] 
		public void TestThread() 
		{
			Account account = NewAccount6();

			try 
			{
				Thread[] threads = new Thread[_numberOfThreads];

				AccessTest accessTest = new AccessTest();

				for (int i = 0; i < _numberOfThreads; i++) 
				{
					Thread thread = new Thread(new ThreadStart(accessTest.GetAccount));
					threads[i] = thread;
				}
				for (int i = 0; i < _numberOfThreads; i++) 
				{
					threads[i].Start();
				}
			} 
			finally 
			{
			}

		}

		#endregion

		/// <summary>
		/// Summary description for AccessTest.
		/// </summary>
		private class AccessTest
		{
		
			/// <summary>
			/// Get an account
			/// </summary>
			public void GetAccount()
			{
				Assert.IsFalse(sqlMap.IsSessionStarted);
				
				Account account = (Account) sqlMap.QueryForObject("GetAccountViaColumnIndex", 1);
				
				Assert.IsFalse(sqlMap.IsSessionStarted);
				
				Assert.AreEqual(1, account.Id, "account.Id");
				Assert.AreEqual("Joe", account.FirstName, "account.FirstName");
				Assert.AreEqual("Dalton", account.LastName, "account.LastName");

			}
		}	
	}


}
