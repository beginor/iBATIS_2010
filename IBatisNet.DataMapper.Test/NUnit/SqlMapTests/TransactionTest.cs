using System;
using System.Collections;
using System.Configuration;

using NUnit.Framework;

using IBatisNet.DataMapper.Exceptions;

using IBatisNet.DataMapper.Test;
using IBatisNet.DataMapper.Test.Domain;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for TransactionTest.
	/// </summary>
	[TestFixture] 
	public class TransactionTest: BaseTest
	{

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

		#region Transaction tests

        /// <summary>
        /// Test IsTransactionStart
        /// </summary>
        [Test]
        public void TestIsTransactionStartProperty()
        {
            Account account = NewAccount6();

            sqlMap.BeginTransaction();
            sqlMap.Insert("InsertAccountViaParameterMap", account);
            InsertNewAccount();
            sqlMap.CommitTransaction();

        }

        public void InsertNewAccount()
        {
            Account account = NewAccount6();
            account.Id = 7;
            bool existingTransaction = (sqlMap.LocalSession != null && !sqlMap.LocalSession.IsTransactionStart);

            if (existingTransaction)
            {
                 sqlMap.BeginTransaction();
            }
            sqlMap.Insert("InsertAccountViaParameterMap", account);
            if (existingTransaction)
            {
                sqlMap.CommitTransaction();
            }
        }

		/// <summary>
		/// Test BeginTransaction, CommitTransaction
		/// </summary>
		[Test] 
		public void TestBeginCommitTransaction() 
		{
			Account account = NewAccount6();

			try 
			{
				sqlMap.BeginTransaction();
				sqlMap.Insert("InsertAccountViaParameterMap", account);
				sqlMap.CommitTransaction();
			} 
			finally 
			{
			}

			// This will use autocommit...
			account = (Account) sqlMap.QueryForObject("GetAccountNullableEmail", 6);

			AssertAccount6(account);
		}

		/// <summary>
		/// Test that nested BeginTransaction throw an exception
		/// </summary>
		[Test] 
		public void TestTransactionAlreadyStarted() 
		{
			Account account = NewAccount6();
			bool exceptionThrownAsExpected = false;

			try 
			{
				sqlMap.BeginTransaction();
				sqlMap.Insert("InsertAccountViaParameterMap", account);

				try 
				{
					sqlMap.BeginTransaction(); // transaction already started
				} 
				catch (DataMapperException e) 
				{
					exceptionThrownAsExpected = true;
					Console.WriteLine ( "Test TransactionAlreadyStarted " + e.Message );
				}
				sqlMap.CommitTransaction();
			} 
			finally 
			{
			}

			// This will use autocommit...
			account = (Account) sqlMap.QueryForObject("GetAccountNullableEmail", 6);
			AssertAccount6(account);
			Assert.IsTrue(exceptionThrownAsExpected);
		}

		/// <summary>
		/// Test that CommitTransaction without BeginTransaction trow an exception
		/// </summary>
		[Test]
		public void TestNoTransactionStarted() 
		{
			Account account = NewAccount6();
			bool exceptionThrownAsExpected = false;

			sqlMap.Insert("InsertAccountViaParameterMap", account);

			try 
			{
				sqlMap.CommitTransaction(); // No transaction started
			} 
			catch (DataMapperException e) 
			{
				exceptionThrownAsExpected = true;
				Console.WriteLine ( "Test NoTransactionStarted " + e.Message );
			}

			// This will use autocommit...
			Assert.IsTrue(exceptionThrownAsExpected);
			account = (Account) sqlMap.QueryForObject("GetAccountNullableEmail", 6);
			AssertAccount6(account);
		}

		/// <summary>
		/// Test a RollBack Transaction.
		/// </summary>
		[Test] 
		public void TestBeginRollbackTransaction() 
		{
			Account account = NewAccount6();

			try 
			{
				sqlMap.BeginTransaction();
				sqlMap.Insert("InsertAccountViaParameterMap", account);
			} 
			finally 
			{
				sqlMap.RollBackTransaction();
			}

			// This will use autocommit...
			account = (Account) sqlMap.QueryForObject("GetAccountNullableEmail", 6);
			Assert.IsNull(account);
		}

		#endregion

		#region AutoCommit tests

		/// <summary>
		/// Test usage of auto commit for an insert
		/// </summary>
		[Test] 
		public void TestAutoCommitInsert() 
		{
			Account account = NewAccount6();

			sqlMap.Insert("InsertAccountViaParameterMap", account);

			account = (Account) sqlMap.QueryForObject("GetAccountNullableEmail", 6);
			AssertAccount6(account);
		}

		/// <summary>
		/// Test usage of auto commit for a query
		/// </summary>
		[Test] 
		public void TestAutoCommitQuery() 
		{
			Account account = (Account) sqlMap.QueryForObject("GetAccountNullableEmail", 1);

			AssertAccount1(account);
		}

		#endregion

	}
}
