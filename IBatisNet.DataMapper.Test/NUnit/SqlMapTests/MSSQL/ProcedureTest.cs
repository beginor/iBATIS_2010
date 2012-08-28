using System;
using System.Collections;
using IBatisNet.DataMapper.Test.Domain;
using NUnit.Framework;
//using NUnit.Framework.SyntaxHelpers;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests.MSSQL
{
	/// <summary>
	/// Summary description for ProcedureTest.
	/// </summary>
	[TestFixture] 
	[Category("MSSQL")]
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
			InitScript( sqlMap.DataSource, ScriptDirectory + "category-procedure.sql" );		
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-init.sql" );	
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-procedure.sql", false );
            InitScript( sqlMap.DataSource, ScriptDirectory + "category-procedureWithReturn.sql", false);
			InitScript( sqlMap.DataSource, ScriptDirectory + "ps_SelectAccount.sql", false );
            InitScript( sqlMap.DataSource, ScriptDirectory + "ps_SelectAllAccount.sql", false);			    
			InitScript( sqlMap.DataSource, ScriptDirectory + "swap-procedure.sql" );
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Specific statement store procedure tests for sql server

#if dotnet2
	    /// <summary>
        /// Test an insert with via a store procedure and getting the generatedKey from a t-sql return statement
        /// </summary>
        [Test]
        public void InsertTestIdentityViaProcedureWithReturn ( )
        {
            Category category = new Category ( );
            category.Name = "Mapping object relational";

            int categoryID = ( int ) sqlMap.Insert ( "InsertCategoryViaStoreProcedureWithReturn", category );
            Assert.That(categoryID, Is.EqualTo(1));
            Assert.That(category.Id, Is.EqualTo(1));

            Category category2 = new Category ( );
            category2.Name = "Nausicaa";

            int categoryID2 = ( int ) sqlMap.Insert ( "InsertCategoryViaStoreProcedureWithReturn", category2 );
            Assert.That(categoryID2, Is.EqualTo(2));
            Assert.That(category2.Id, Is.EqualTo(2));

            Category category3 = sqlMap.QueryForObject<Category> ( "GetCategory", categoryID2 ) ;
            Category category4 = sqlMap.QueryForObject<Category> ( "GetCategory", categoryID );
            
            Assert.AreEqual ( categoryID2, category3.Id );
            Assert.AreEqual ( category2.Name, category3.Name );

            Assert.AreEqual ( categoryID, category4.Id );
            Assert.AreEqual ( category.Name, category4.Name );
        }
#endif

        /// <summary>
        /// Test XML parameter.
        /// </summary>
        [Test]
        [Category("MSSQL.2005")]
        public void TestXMLParameter()
        {
            InitScript(sqlMap.DataSource, ScriptDirectory + "ps_SelectByIdList.sql");	

            string accountIds = "<Accounts><id>3</id><id>4</id></Accounts>";

            IList accounts = sqlMap.QueryForList("SelectAccountViaXML", accountIds);
            Assert.IsTrue(accounts.Count == 2);
        }

        /// <summary>
        /// Test get an account via a store procedure.
        /// </summary>
        [Test]
        public void GetAllAccountViaProcedure()
        {
            IList accounts = sqlMap.QueryForList("SelectAllAccountViaSP", null);
            Assert.IsTrue( accounts.Count==5);
        }
	    
		/// <summary>
		/// Test get an account via a store procedure.
		/// </summary>
		[Test] 
		public void GetAccountViaProcedure0()
		{
			Account account = sqlMap.QueryForObject("GetAccountViaSP0", 1) as Account;
			Assert.AreEqual(1, account.Id );
		}
		
		/// <summary>
		/// Test get an account via a store procedure.
		/// </summary>
		[Test] 
		public void GetAccountViaProcedure1()
		{
			Account account = sqlMap.QueryForObject("GetAccountViaSP1", 1) as Account;
			Assert.AreEqual(1, account.Id );
		}
		
		/// <summary>
		/// Test get an account via a store procedure.
		/// </summary>
		[Test] 
		public void GetAccountViaProcedure2()
		{
			Hashtable hash = new Hashtable();
			hash.Add("Account_ID",1);
			Account account = sqlMap.QueryForObject("GetAccountViaSP2", hash) as Account;
			Assert.AreEqual(1, account.Id );
		}
		
		/// <summary>
		/// Test an insert with identity key via a store procedure.
		/// </summary>
		[Test] 
		public void InsertTestIdentityViaProcedure()
		{
			Category category = new Category();
			category.Name = "Mapping object relational";

			sqlMap.Insert("InsertCategoryViaStoreProcedure", category);
			Assert.AreEqual(1, category.Id );

			category = new Category();
			category.Name = "Nausicaa";

			sqlMap.QueryForObject("InsertCategoryViaStoreProcedure", category);
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

			Assert.AreEqual(first, map["email2"]);
			Assert.AreEqual(second, map["email1"]);
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
			map.Add("Guid", Guid.NewGuid());

			sqlMap.Insert("InsertCategoryViaStoreProcedureWithMap", map);
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
        /// Test DBHelperParameterCache in transaction
        /// </summary>
        [Test]
        public void TestDBHelperParameterCache()
        {
            Account account = new Account();

            account.Id = 99;
            account.FirstName = "Achille";
            account.LastName = "Talon";
            account.EmailAddress = "Achille.Talon@somewhere.com";

            sqlMap.BeginTransaction();
            sqlMap.Insert("InsertAccountViaStoreProcedure", account);

            Hashtable map = new Hashtable();
            map.Add("Id", 0);
            map.Add("Name", "Toto");
            map.Add("Guid", Guid.NewGuid());

            sqlMap.Insert("InsertCategoryViaStoreProcedureWithMap", map);
            Assert.AreEqual(1, map["Id"]);

            sqlMap.CommitTransaction();

        }
		#endregion
	}
}
