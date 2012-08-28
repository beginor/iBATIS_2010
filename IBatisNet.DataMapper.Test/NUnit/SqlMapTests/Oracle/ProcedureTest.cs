using System;
using System.Collections;

using NUnit.Framework;

using IBatisNet.DataMapper.Test.NUnit;
using IBatisNet.DataMapper.Test.Domain;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests.Oracle
{
	/// <summary>
	/// Summary description for ProcedureTest.
	/// </summary>
	[TestFixture] 
	[Category("Oracle")]
	public class ProcedureTest : BaseTest
	{
		
		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
			InitScript( sqlMap.DataSource, ScriptDirectory + "category-init.sql" );
			InitScript( sqlMap.DataSource, ScriptDirectory + "category-procedure.sql", false );		
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-init.sql" );	
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-procedure.sql", false );
			InitScript( sqlMap.DataSource, ScriptDirectory + "swap-procedure.sql", false );	
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-refcursor-package-spec.sql", false );	
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-refcursor-package-body.sql", false );	
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Specific statement store procedure tests for oracle

		/// <summary>
		/// Test an insert with sequence key via a store procedure.
		/// </summary>
		[Test] 
		public void InsertTestSequenceViaProcedure()
		{
			Category category = new Category();
			category.Name = "Mapping object relational";

			sqlMap.Insert("InsertCategoryViaStoreProcedure", category);
			Assert.AreEqual(1, category.Id );

			category = new Category();
			category.Name = "Nausicaa";

			sqlMap.Insert("InsertCategoryViaStoreProcedure", category);
			Assert.AreEqual(2, category.Id );
		}

		/// <summary>
		/// Test store procedure with output parameters
		/// </summary>
		[Test]
		public void TestProcedureWithOutputParameters() 
		{
			string first = "Joe.Dalton@somewhere.com";
			string second = "Averel.Dalton@somewhere.com";

			Hashtable map = new Hashtable();
			map.Add("email1", first);
			map.Add("email2", second);

			sqlMap.QueryForObject("SwapEmailAddresses", map);

			Assert.AreEqual(first, map["email2"].ToString());
			Assert.AreEqual(second, map["email1"].ToString());
		}

		/// <summary>
		/// Test store procedure with input parameters
		/// passe via Hashtable
		/// </summary>
		[Test]
		public void TestProcedureWithInputParametersViaHashtable() 
		{
			Hashtable map = new Hashtable();
			map.Add("Id", 0);
			map.Add("Name", "Toto");
			map.Add("GuidString", Guid.NewGuid().ToString());

			sqlMap.Insert("InsertCategoryViaStoreProcedure", map);
			Assert.AreEqual(1, map["Id"] );

		}

		/// <summary>
		/// Test Insert Account via store procedure
		/// </summary>
		[Test] 
		public void TestInsertAccountViaStoreProcedure() {
			Account account = new Account();

			account.Id = 99;
			account.FirstName = "Achille";
			account.LastName = "Talon";
			account.EmailAddress = "Achille.Talon@somewhere.com";

			sqlMap.Insert("InsertAccountViaStoreProcedure", account);

			Account testAccount = sqlMap.QueryForObject("GetAccountViaColumnName", 99) as Account;

			Assert.IsNotNull(testAccount);
			Assert.AreEqual(99, testAccount.Id);
		}

		/// <summary>
		/// Test QueryForList with Ref Cursor.
		/// </summary>
		[Test]
		public void QueryForListWithRefCursor()
		{
			Hashtable param = new Hashtable();
			param.Add("P_ACCOUNTS",null);

			IList list = sqlMap.QueryForList("GetAllAccountsViaStoredProcRefCursor", param);

			Assert.AreEqual(5, list.Count);
			AssertAccount1((Account) list[0]);
			Assert.AreEqual(2, ((Account) list[1]).Id);
			Assert.AreEqual("Averel", ((Account) list[1]).FirstName);
			Assert.AreEqual(3, ((Account) list[2]).Id);
			Assert.AreEqual("William", ((Account) list[2]).FirstName);
			Assert.AreEqual(4, ((Account) list[3]).Id);
			Assert.AreEqual("Jack", ((Account) list[3]).FirstName);
			Assert.AreEqual(5, ((Account) list[4]).Id);
			Assert.AreEqual("Gilles", ((Account) list[4]).FirstName);
		}

		/// <summary>
		/// Test QueryForList with Ref Cursor and Input.
		/// </summary>
		[Test]
		public void QueryForListWithRefCursorAndInput()
		{
			Hashtable param = new Hashtable();
			param.Add("P_ACCOUNTS",null);
			param.Add("P_ACCOUNT_ID",1);

			IList list = sqlMap.QueryForList("GetAccountViaStoredProcRefCursor", param);

			Assert.AreEqual(1, list.Count);
			AssertAccount1((Account) list[0]);
		}		
		#endregion
	}
}
