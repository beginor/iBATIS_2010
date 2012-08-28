
using System;
using System.Collections;
using IBatisNet.Common;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Test.Domain;
using NUnit.Framework;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
    /// <summary>
    /// Summary description for ParameterMapTest.
    /// </summary>
    [TestFixture]
    public class StatementTest : BaseTest
    {
        #region SetUp & TearDown

        /// <summary>
        /// SetUp
        /// </summary>
        [SetUp]
        public void Init()
        {
            InitScript(sqlMap.DataSource, ScriptDirectory + "account-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "order-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "line-item-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "enumeration-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "other-init.sql");
        }

        /// <summary>
        /// TearDown
        /// </summary>
        [TearDown]
        public void Dispose()
        { /* ... */
        }

        #endregion

        #region Object Query tests

        /// <summary>
        /// Interface mapping
        /// </summary>
        [Test]
        [Category("JIRA")]
        [Description("JIRA-283")]
        public void TestInterface()
        {
            BaseAccount account = new BaseAccount();

            sqlMap.QueryForObject<IAccount>("GetInterfaceAccount", 1, account);

            Assert.AreEqual(1, account.Id, "account.Id");
            Assert.AreEqual("Joe", account.FirstName, "account.FirstName");
            Assert.AreEqual("Dalton", account.LastName, "account.LastName");
            Assert.AreEqual("Joe.Dalton@somewhere.com", account.EmailAddress, "account.EmailAddress");
        }

        /// <summary>
        /// Test Open connection with a connection string
        /// </summary>
        [Test]
        public void TestOpenConnection()
        {
            sqlMap.OpenConnection(sqlMap.DataSource.ConnectionString);
            Account account = sqlMap.QueryForObject("SelectWithProperty", null) as Account;
            sqlMap.CloseConnection();

            AssertAccount1(account);
        }

        /// <summary>
        /// Test use a statement with property subtitution
        /// (JIRA 22)
        /// </summary>
        [Test]
        public void TestSelectWithProperty()
        {
            Account account = sqlMap.QueryForObject("SelectWithProperty", null) as Account;
            AssertAccount1(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject Via ColumnName
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectViaColumnName()
        {
            Account account = sqlMap.QueryForObject("GetAccountViaColumnName", 1) as Account;
            AssertAccount1(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject Via ColumnIndex
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectViaColumnIndex()
        {
            Account account = sqlMap.QueryForObject("GetAccountViaColumnIndex", 1) as Account;
            AssertAccount1(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject Via ResultClass
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectViaResultClass()
        {
            Account account = sqlMap.QueryForObject("GetAccountViaResultClass", 1) as Account;
            AssertAccount1(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject With simple ResultClass : string
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectWithSimpleResultClass()
        {
            string email = sqlMap.QueryForObject("GetEmailAddressViaResultClass", 1) as string;
            Assert.AreEqual("Joe.Dalton@somewhere.com", email);
        }

        /// <summary>
        /// Test ExecuteQueryForObject With simple ResultMap : string
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectWithSimpleResultMap()
        {
            string email = sqlMap.QueryForObject("GetEmailAddressViaResultMap", 1) as string;
            Assert.AreEqual("Joe.Dalton@somewhere.com", email);
        }

        /// <summary>
        /// Test Primitive ReturnValue : System.DateTime
        /// </summary>
        [Test]
        public void TestPrimitiveReturnValue()
        {
            DateTime CardExpiry = (DateTime)sqlMap.QueryForObject("GetOrderCardExpiryViaResultClass", 1);
            Assert.AreEqual(new DateTime(2003, 02, 15, 8, 15, 00), CardExpiry);
        }

        /// <summary>
        /// Test ExecuteQueryForObject with result object : Account
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectWithResultObject()
        {
            Account account = new Account();
            Account testAccount = sqlMap.QueryForObject("GetAccountViaColumnName", 1, account) as Account;
            AssertAccount1(account);
            Assert.IsTrue(account == testAccount);
        }

        /// <summary>
        /// Test ExecuteQueryForObject as Hashtable
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectAsHashtable()
        {
            Hashtable account = (Hashtable)sqlMap.QueryForObject("GetAccountAsHashtable", 1);
            AssertAccount1AsHashtable(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject as Hashtable ResultClass
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectAsHashtableResultClass()
        {
            Hashtable account = (Hashtable)sqlMap.QueryForObject("GetAccountAsHashtableResultClass", 1);
            AssertAccount1AsHashtableForResultClass(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject via Hashtable
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectViaHashtable()
        {
            Hashtable param = new Hashtable();
            param.Add("LineItem_ID", 4);
            param.Add("Order_ID", 9);

            LineItem testItem = sqlMap.QueryForObject("GetSpecificLineItem", param) as LineItem;

            Assert.IsNotNull(testItem);
            Assert.AreEqual("TSM-12", testItem.Code);
        }

        /// <summary>
        /// Test Query Dynamic Sql Element
        /// </summary>
        [Test]
        public void TestQueryDynamicSqlElement()
        {
            IList list = sqlMap.QueryForList("GetDynamicOrderedEmailAddressesViaResultMap", "Account_ID");

            Assert.AreEqual("Joe.Dalton@somewhere.com", (string)list[0]);

            list = sqlMap.QueryForList("GetDynamicOrderedEmailAddressesViaResultMap", "Account_FirstName");

            Assert.AreEqual("Averel.Dalton@somewhere.com", (string)list[0]);

        }

        /// <summary>
        /// Test Execute QueryForList With ResultMap With Dynamic Element
        /// </summary>
        [Test]
        public void TestExecuteQueryForListWithResultMapWithDynamicElement()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsViaResultMapWithDynamicElement", "LIKE");

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(4, list.Count);
            Assert.AreEqual(1, ((Account)list[0]).Id);
            Assert.AreEqual(2, ((Account)list[1]).Id);
            Assert.AreEqual(4, ((Account)list[2]).Id);

            list = sqlMap.QueryForList("GetAllAccountsViaResultMapWithDynamicElement", "=");

            Assert.AreEqual(0, list.Count);
        }

        /// <summary>
        /// Test Simple Dynamic Substitution
        /// </summary>
        [Test]
        [Ignore("No longer supported.")]
        public void TestSimpleDynamicSubstitution()
        {
            string statement = "select" + "    Account_ID          as Id," + "    Account_FirstName   as FirstName," + "    Account_LastName    as LastName," + "    Account_Email       as EmailAddress" + "  from Accounts" + "  WHERE Account_ID = #id#";

            Hashtable param = new Hashtable();
            param.Add("id", 1);
            param.Add("statement", statement);


            IList list = sqlMap.QueryForList("SimpleDynamicSubstitution", param);
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(1, list.Count);
        }

        /// <summary>
        /// Test Get Account Via Inline Parameters
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectViaInlineParameters()
        {
            Account account = new Account();
            account.Id = 1;

            Account testAccount = sqlMap.QueryForObject("GetAccountViaInlineParameters", account) as Account;

            AssertAccount1(testAccount);
        }

        /// <summary>
        /// Test ExecuteQuery For Object With Enum property
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectWithEnum()
        {
            Enumeration enumClass = sqlMap.QueryForObject("GetEnumeration", 1) as Enumeration;

            Assert.AreEqual(enumClass.Day, Days.Sat);
            Assert.AreEqual(enumClass.Color, Colors.Red);
            Assert.AreEqual(enumClass.Month, Months.August);

            enumClass = sqlMap.QueryForObject("GetEnumeration", 3) as Enumeration;

            Assert.AreEqual(enumClass.Day, Days.Mon);
            Assert.AreEqual(enumClass.Color, Colors.Blue);
            Assert.AreEqual(enumClass.Month, Months.September);
        }

        #endregion

        #region  List Query tests

        /// <summary>
        /// Test QueryForList with Hashtable ResultMap
        /// </summary>
        [Test]
        public void TestQueryForListWithHashtableResultMap()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsAsHashMapViaResultMap", null);

            AssertAccount1AsHashtable((Hashtable)list[0]);
            Assert.AreEqual(5, list.Count);

            Assert.AreEqual(1, ((Hashtable)list[0])["Id"]);
            Assert.AreEqual(2, ((Hashtable)list[1])["Id"]);
            Assert.AreEqual(3, ((Hashtable)list[2])["Id"]);
            Assert.AreEqual(4, ((Hashtable)list[3])["Id"]);
            Assert.AreEqual(5, ((Hashtable)list[4])["Id"]);
        }

        /// <summary>
        /// Test QueryForList with Hashtable ResultClass
        /// </summary>
        [Test]
        public void TestQueryForListWithHashtableResultClass()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsAsHashtableViaResultClass", null);

            AssertAccount1AsHashtableForResultClass((Hashtable)list[0]);
            Assert.AreEqual(5, list.Count);

            Assert.AreEqual(1, ((Hashtable)list[0])[BaseTest.ConvertKey("Id")]);
            Assert.AreEqual(2, ((Hashtable)list[1])[BaseTest.ConvertKey("Id")]);
            Assert.AreEqual(3, ((Hashtable)list[2])[BaseTest.ConvertKey("Id")]);
            Assert.AreEqual(4, ((Hashtable)list[3])[BaseTest.ConvertKey("Id")]);
            Assert.AreEqual(5, ((Hashtable)list[4])[BaseTest.ConvertKey("Id")]);
        }

        /// <summary>
        /// Test QueryForList with IList ResultClass
        /// </summary>
        [Test]
        public void TestQueryForListWithIListResultClass()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsAsArrayListViaResultClass", null);

            IList listAccount = (IList)list[0];
            Assert.AreEqual(1, listAccount[0]);
            Assert.AreEqual("Joe", listAccount[1]);
            Assert.AreEqual("Dalton", listAccount[2]);
            Assert.AreEqual("Joe.Dalton@somewhere.com", listAccount[3]);

            Assert.AreEqual(5, list.Count);

            listAccount = (IList)list[0];
            Assert.AreEqual(1, listAccount[0]);
            listAccount = (IList)list[1];
            Assert.AreEqual(2, listAccount[0]);
            listAccount = (IList)list[2];
            Assert.AreEqual(3, listAccount[0]);
            listAccount = (IList)list[3];
            Assert.AreEqual(4, listAccount[0]);
            listAccount = (IList)list[4];
            Assert.AreEqual(5, listAccount[0]);
        }

        /// <summary>
        /// Test QueryForList With ResultMap, result collection as ArrayList
        /// </summary>
        [Test]
        public void TestQueryForListWithResultMap()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsViaResultMap", null);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(5, list.Count);
            Assert.AreEqual(1, ((Account)list[0]).Id);
            Assert.AreEqual(2, ((Account)list[1]).Id);
            Assert.AreEqual(3, ((Account)list[2]).Id);
            Assert.AreEqual(4, ((Account)list[3]).Id);
            Assert.AreEqual(5, ((Account)list[4]).Id);
        }

        /// <summary>
        /// Test ExecuteQueryForPaginatedList
        /// </summary>
        [Test]
        public void TestExecuteQueryForPaginatedList()
        {
            // Get List of all 5
            PaginatedList list = sqlMap.QueryForPaginatedList("GetAllAccountsViaResultMap", null, 2);

            // Test initial state (page 0)
            Assert.IsFalse(list.IsPreviousPageAvailable);
            Assert.IsTrue(list.IsNextPageAvailable);
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, ((Account)list[0]).Id);
            Assert.AreEqual(2, ((Account)list[1]).Id);

            // Test illegal previous page (no effect, state should be same)
            list.PreviousPage();
            Assert.IsFalse(list.IsPreviousPageAvailable);
            Assert.IsTrue(list.IsNextPageAvailable);
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, ((Account)list[0]).Id);
            Assert.AreEqual(2, ((Account)list[1]).Id);

            // Test next (page 1)
            list.NextPage();
            Assert.IsTrue(list.IsPreviousPageAvailable);
            Assert.IsTrue(list.IsNextPageAvailable);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(3, ((Account)list[0]).Id);
            Assert.AreEqual(4, ((Account)list[1]).Id);

            // Test next (page 2 -last)
            list.NextPage();
            Assert.IsTrue(list.IsPreviousPageAvailable);
            Assert.IsFalse(list.IsNextPageAvailable);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(5, ((Account)list[0]).Id);

            // Test previous (page 1)
            list.PreviousPage();
            Assert.IsTrue(list.IsPreviousPageAvailable);
            Assert.IsTrue(list.IsNextPageAvailable);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(3, ((Account)list[0]).Id);
            Assert.AreEqual(4, ((Account)list[1]).Id);

            // Test previous (page 0 -first)
            list.PreviousPage();
            Assert.IsFalse(list.IsPreviousPageAvailable);
            Assert.IsTrue(list.IsNextPageAvailable);
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, ((Account)list[0]).Id);
            Assert.AreEqual(2, ((Account)list[1]).Id);

            // Test goto (page 0)
            list.GotoPage(0);
            Assert.IsFalse(list.IsPreviousPageAvailable);
            Assert.IsTrue(list.IsNextPageAvailable);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, ((Account)list[0]).Id);
            Assert.AreEqual(2, ((Account)list[1]).Id);

            // Test goto (page 1)
            list.GotoPage(1);
            Assert.IsTrue(list.IsPreviousPageAvailable);
            Assert.IsTrue(list.IsNextPageAvailable);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(3, ((Account)list[0]).Id);
            Assert.AreEqual(4, ((Account)list[1]).Id);

            // Test goto (page 2)
            list.GotoPage(2);
            Assert.IsTrue(list.IsPreviousPageAvailable);
            Assert.IsFalse(list.IsNextPageAvailable);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(5, ((Account)list[0]).Id);

            // Test illegal goto (page 0)
            list.GotoPage(3);
            Assert.IsTrue(list.IsPreviousPageAvailable);
            Assert.IsFalse(list.IsNextPageAvailable);
            Assert.AreEqual(0, list.Count);

            list = sqlMap.QueryForPaginatedList("GetNoAccountsViaResultMap", null, 2);

            // Test empty list
            Assert.IsFalse(list.IsPreviousPageAvailable);
            Assert.IsFalse(list.IsNextPageAvailable);
            Assert.AreEqual(0, list.Count);

            // Test next
            list.NextPage();
            Assert.IsFalse(list.IsPreviousPageAvailable);
            Assert.IsFalse(list.IsNextPageAvailable);
            Assert.AreEqual(0, list.Count);

            // Test previous
            list.PreviousPage();
            Assert.IsFalse(list.IsPreviousPageAvailable);
            Assert.IsFalse(list.IsNextPageAvailable);
            Assert.AreEqual(0, list.Count);

            // Test previous
            list.GotoPage(0);
            Assert.IsFalse(list.IsPreviousPageAvailable);
            Assert.IsFalse(list.IsNextPageAvailable);
            Assert.AreEqual(0, list.Count);

            list = sqlMap.QueryForPaginatedList("GetFewAccountsViaResultMap", null, 2);

            Assert.IsFalse(list.IsPreviousPageAvailable);
            Assert.IsFalse(list.IsNextPageAvailable);
            Assert.AreEqual(1, list.Count);

            // Test next
            list.NextPage();
            Assert.IsFalse(list.IsPreviousPageAvailable);
            Assert.IsFalse(list.IsNextPageAvailable);
            Assert.AreEqual(1, list.Count);

            // Test previous
            list.PreviousPage();
            Assert.IsFalse(list.IsPreviousPageAvailable);
            Assert.IsFalse(list.IsNextPageAvailable);
            Assert.AreEqual(1, list.Count);

            // Test previous
            list.GotoPage(0);
            Assert.IsFalse(list.IsPreviousPageAvailable);
            Assert.IsFalse(list.IsNextPageAvailable);
            Assert.AreEqual(1, list.Count);

            // Test Even - Two Pages
            try
            {
                InitScript(sqlMap.DataSource, ScriptDirectory + "more-account-records.sql");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            list = sqlMap.QueryForPaginatedList("GetAllAccountsViaResultMap", null, 5);

            Assert.AreEqual(5, list.Count);

            list.NextPage();
            Assert.AreEqual(5, list.Count);

            bool b = list.IsPreviousPageAvailable;
            list.PreviousPage();
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test QueryForList with ResultObject : 
        /// AccountCollection strongly typed collection
        /// </summary>
        [Test]
        public void TestQueryForListWithResultObject()
        {
            AccountCollection accounts = new AccountCollection();

            sqlMap.QueryForList("GetAllAccountsViaResultMap", null, accounts);

            AssertAccount1(accounts[0]);
            Assert.AreEqual(5, accounts.Count);
            Assert.AreEqual(1, accounts[0].Id);
            Assert.AreEqual(2, accounts[1].Id);
            Assert.AreEqual(3, accounts[2].Id);
            Assert.AreEqual(4, accounts[3].Id);
            Assert.AreEqual(5, accounts[4].Id);
        }

        /// <summary>
        /// Test QueryForList with ListClass : LineItemCollection
        /// </summary>
        [Test]
        public void TestQueryForListWithListClass()
        {
            LineItemCollection linesItem = sqlMap.QueryForList("GetLineItemsForOrderWithListClass", 6) as LineItemCollection;

            Assert.IsNotNull(linesItem);
            Assert.AreEqual(2, linesItem.Count);
            Assert.AreEqual("ASM-45", linesItem[0].Code);
            Assert.AreEqual("QSM-39", linesItem[1].Code);
        }

        /// <summary>
        /// Test QueryForList with no result.
        /// </summary>
        [Test]
        public void TestQueryForListWithNoResult()
        {
            IList list = sqlMap.QueryForList("GetNoAccountsViaResultMap", null);

            Assert.AreEqual(0, list.Count);
        }

        /// <summary>
        /// Test QueryForList with ResultClass : Account.
        /// </summary>
        [Test]
        public void TestQueryForListResultClass()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsViaResultClass", null);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(5, list.Count);
            Assert.AreEqual(1, ((Account)list[0]).Id);
            Assert.AreEqual(2, ((Account)list[1]).Id);
            Assert.AreEqual(3, ((Account)list[2]).Id);
            Assert.AreEqual(4, ((Account)list[3]).Id);
            Assert.AreEqual(5, ((Account)list[4]).Id);
        }

        /// <summary>
        /// Test QueryForList with simple resultClass : string
        /// </summary>
        [Test]
        public void TestQueryForListWithSimpleResultClass()
        {
            IList list = sqlMap.QueryForList("GetAllEmailAddressesViaResultClass", null);

            Assert.AreEqual("Joe.Dalton@somewhere.com", (string)list[0]);
            Assert.AreEqual("Averel.Dalton@somewhere.com", (string)list[1]);
            Assert.IsNull(list[2]);
            Assert.AreEqual("Jack.Dalton@somewhere.com", (string)list[3]);
            Assert.AreEqual("gilles.bayon@nospam.org", (string)list[4]);
        }

        /// <summary>
        /// Test  QueryForList with simple ResultMap : string
        /// </summary>
        [Test]
        public void TestQueryForListWithSimpleResultMap()
        {
            IList list = sqlMap.QueryForList("GetAllEmailAddressesViaResultMap", null);

            Assert.AreEqual("Joe.Dalton@somewhere.com", (string)list[0]);
            Assert.AreEqual("Averel.Dalton@somewhere.com", (string)list[1]);
            Assert.IsNull(list[2]);
            Assert.AreEqual("Jack.Dalton@somewhere.com", (string)list[3]);
            Assert.AreEqual("gilles.bayon@nospam.org", (string)list[4]);
        }

        /// <summary>
        /// Test QueryForListWithSkipAndMax
        /// </summary>
        [Test]
        public void TestQueryForListWithSkipAndMax()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsViaResultMap", null, 2, 2);

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(3, ((Account)list[0]).Id);
            Assert.AreEqual(4, ((Account)list[1]).Id);
        }


        [Test]
        public void TestQueryWithRowDelegate()
        {
            RowDelegate handler = new RowDelegate(this.RowHandler);

            IList list = sqlMap.QueryWithRowDelegate("GetAllAccountsViaResultMap", null, handler);

            Assert.AreEqual(5, _index);
            Assert.AreEqual(5, list.Count);
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(1, ((Account)list[0]).Id);
            Assert.AreEqual(2, ((Account)list[1]).Id);
            Assert.AreEqual(3, ((Account)list[2]).Id);
            Assert.AreEqual(4, ((Account)list[3]).Id);
            Assert.AreEqual(5, ((Account)list[4]).Id);

        }

        #endregion

        #region  Map Tests

        /// <summary>
        /// Test ExecuteQueryForMap : Hashtable.
        /// </summary>
        [Test]
        public void TestExecuteQueryForMap()
        {
            IDictionary map = sqlMap.QueryForMap("GetAllAccountsViaResultClass", null, "FirstName");

            Assert.AreEqual(5, map.Count);
            AssertAccount1(((Account)map["Joe"]));

            Assert.AreEqual(1, ((Account)map["Joe"]).Id);
            Assert.AreEqual(2, ((Account)map["Averel"]).Id);
            Assert.AreEqual(3, ((Account)map["William"]).Id);
            Assert.AreEqual(4, ((Account)map["Jack"]).Id);
            Assert.AreEqual(5, ((Account)map["Gilles"]).Id);
        }

        /// <summary>
        /// Test ExecuteQueryForMap With Cache : Hashtable.
        /// </summary>
        [Test]
        public void TestExecuteQueryForMapWithCache()
        {
            IDictionary map = sqlMap.QueryForMap("GetAllAccountsCache", null, "FirstName");

            int firstId = HashCodeProvider.GetIdentityHashCode(map);

            Assert.AreEqual(5, map.Count);
            AssertAccount1(((Account)map["Joe"]));

            Assert.AreEqual(1, ((Account)map["Joe"]).Id);
            Assert.AreEqual(2, ((Account)map["Averel"]).Id);
            Assert.AreEqual(3, ((Account)map["William"]).Id);
            Assert.AreEqual(4, ((Account)map["Jack"]).Id);
            Assert.AreEqual(5, ((Account)map["Gilles"]).Id);

            map = sqlMap.QueryForMap("GetAllAccountsCache", null, "FirstName");

            int secondId = HashCodeProvider.GetIdentityHashCode(map);

            Assert.AreEqual(firstId, secondId);
        }

        /// <summary>
        /// Test ExecuteQueryForMap : Hashtable.
        /// </summary>
        /// <remarks>
        /// If the keyProperty is an integer, you must acces the map
        /// by map[integer] and not by map["integer"]
        /// </remarks>
        [Test]
        public void TestExecuteQueryForMap2()
        {
            IDictionary map = sqlMap.QueryForMap("GetAllOrderWithLineItems", null, "PostalCode");

            Assert.AreEqual(11, map.Count);
            Order order = ((Order)map["T4H 9G4"]);

            Assert.AreEqual(2, order.LineItemsIList.Count);
        }

        /// <summary>
        /// Test ExecuteQueryForMap with value property :
        /// "FirstName" as key, "EmailAddress" as value
        /// </summary>
        [Test]
        public void TestExecuteQueryForMapWithValueProperty()
        {
            IDictionary map = sqlMap.QueryForMap("GetAllAccountsViaResultClass", null, "FirstName", "EmailAddress");

            Assert.AreEqual(5, map.Count);

            Assert.AreEqual("Joe.Dalton@somewhere.com", map["Joe"]);
            Assert.AreEqual("Averel.Dalton@somewhere.com", map["Averel"]);
            Assert.IsNull(map["William"]);
            Assert.AreEqual("Jack.Dalton@somewhere.com", map["Jack"]);
            Assert.AreEqual("gilles.bayon@nospam.org", map["Gilles"]);
        }

        /// <summary>
        /// Test ExecuteQueryForWithJoined
        /// </remarks>
        [Test]
        public void TestExecuteQueryForWithJoined()
        {
            Order order = sqlMap.QueryForObject("GetOrderJoinWithAccount", 10) as Order;

            Assert.IsNotNull(order.Account);

            order = sqlMap.QueryForObject("GetOrderJoinWithAccount", 11) as Order;

            Assert.IsNull(order.Account);
        }

        /// <summary>
        ///  Better support for nested result maps when using dictionary
        /// </remarks>
        [Test]
        [Category("JIRA-254")]
        public void Better_Support_For_Nested_Result_Maps_When_Using_Dictionary()
        {
            IDictionary order = (IDictionary)sqlMap.QueryForObject("JIRA-254", 10);

            Assert.IsNotNull(order["Account"]);

            order = (IDictionary)sqlMap.QueryForObject("JIRA-254", 11);

            Assert.IsNull(order["Account"]);
        }

        /// <summary>
        /// Test ExecuteQueryFor With Complex Joined
        /// </summary>
        /// <remarks>
        /// A->B->C
        ///  ->E
        ///  ->F
        /// </remarks>
        [Test]
        public void TestExecuteQueryForWithComplexJoined()
        {
            A a = sqlMap.QueryForObject("SelectComplexJoined", null) as A;

            Assert.IsNotNull(a);
            Assert.IsNotNull(a.B);
            Assert.IsNotNull(a.B.C);
            Assert.IsNull(a.B.D);
            Assert.IsNotNull(a.E);
            Assert.IsNull(a.F);
        }
        #endregion

        #region Extends statement

        /// <summary>
        /// Test base Extends statement
        /// </summary>
        [Test]
        public void TestExtendsGetAllAccounts()
        {
            IList list = sqlMap.QueryForList("GetAllAccounts", null);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(5, list.Count);
            Assert.AreEqual(1, ((Account)list[0]).Id);
            Assert.AreEqual(2, ((Account)list[1]).Id);
            Assert.AreEqual(3, ((Account)list[2]).Id);
            Assert.AreEqual(4, ((Account)list[3]).Id);
            Assert.AreEqual(5, ((Account)list[4]).Id);
        }

        /// <summary>
        /// Test Extends statement GetAllAccountsOrderByName extends GetAllAccounts
        /// </summary>
        [Test]
        public void TestExtendsGetAllAccountsOrderByName()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsOrderByName", null);

            AssertAccount1((Account)list[3]);
            Assert.AreEqual(5, list.Count);

            Assert.AreEqual(2, ((Account)list[0]).Id);
            Assert.AreEqual(5, ((Account)list[1]).Id);
            Assert.AreEqual(4, ((Account)list[2]).Id);
            Assert.AreEqual(1, ((Account)list[3]).Id);
            Assert.AreEqual(3, ((Account)list[4]).Id);
        }

        /// <summary>
        /// Test Extends statement GetOneAccount extends GetAllAccounts
        /// </summary>
        [Test]
        public void TestExtendsGetOneAccount()
        {
            Account account = sqlMap.QueryForObject("GetOneAccount", 1) as Account;
            AssertAccount1(account);
        }

        /// <summary>
        /// Test Extends statement GetSomeAccount extends GetAllAccounts
        /// </summary>
        [Test]
        public void TestExtendsGetSomeAccount()
        {
            Hashtable param = new Hashtable();
            param.Add("lowID", 2);
            param.Add("hightID", 4);

            IList list = sqlMap.QueryForList("GetSomeAccount", param);

            Assert.AreEqual(3, list.Count);

            Assert.AreEqual(2, ((Account)list[0]).Id);
            Assert.AreEqual(3, ((Account)list[1]).Id);
            Assert.AreEqual(4, ((Account)list[2]).Id);
        }

        [Test]
        public void TestDummy()
        {
            Hashtable param = new Hashtable();
            param.Add("?lowID", 2);
            param.Add("?hightID", 4);

            IList list = sqlMap.QueryForList("GetDummy", param);

            Assert.AreEqual(3, list.Count);

            Assert.AreEqual(2, ((Account)list[0]).Id);
            Assert.AreEqual(3, ((Account)list[1]).Id);
            Assert.AreEqual(4, ((Account)list[2]).Id);
        }

        #endregion

        #region Update tests

        /// <summary>
        /// Test Insert with post GeneratedKey
        /// </summary>
        [Test]
        public void TestInsertPostKey()
        {
            LineItem item = new LineItem();

            item.Id = 350;
            item.Code = "blah";
            item.Order = new Order();
            item.Order.Id = 9;
            item.Price = 44.00m;
            item.Quantity = 1;

            object key = sqlMap.Insert("InsertLineItemPostKey", item);

            Assert.AreEqual(99, key);
            Assert.AreEqual(99, item.Id);

            Hashtable param = new Hashtable();
            param.Add("Order_ID", 9);
            param.Add("LineItem_ID", 350);
            LineItem testItem = (LineItem)sqlMap.QueryForObject("GetSpecificLineItem", param);
            Assert.IsNotNull(testItem);
            Assert.AreEqual(350, testItem.Id);
        }

        /// <summary>
        /// Test Insert pre GeneratedKey
        /// </summary>
        [Test]
        public void TestInsertPreKey()
        {
            LineItem item = new LineItem();

            item.Id = 10;
            item.Code = "blah";
            item.Order = new Order();
            item.Order.Id = 9;
            item.Price = 44.00m;
            item.Quantity = 1;

            object key = sqlMap.Insert("InsertLineItemPreKey", item);

            Assert.AreEqual(99, key);
            Assert.AreEqual(99, item.Id);

            Hashtable param = new Hashtable();
            param.Add("Order_ID", 9);
            param.Add("LineItem_ID", 99);

            LineItem testItem = (LineItem)sqlMap.QueryForObject("GetSpecificLineItem", param);

            Assert.IsNotNull(testItem);
            Assert.AreEqual(99, testItem.Id);
        }

        /// <summary>
        /// Test Test Insert No Key
        /// </summary>
        [Test]
        public void TestInsertNoKey()
        {
            LineItem item = new LineItem();

            item.Id = 100;
            item.Code = "blah";
            item.Order = new Order();
            item.Order.Id = 9;
            item.Price = 44.00m;
            item.Quantity = 1;

            object key = sqlMap.Insert("InsertLineItemNoKey", item);

            Assert.IsNull(key);
            Assert.AreEqual(100, item.Id);

            Hashtable param = new Hashtable();
            param.Add("Order_ID", 9);
            param.Add("LineItem_ID", 100);

            LineItem testItem = (LineItem)sqlMap.QueryForObject("GetSpecificLineItem", param);

            Assert.IsNotNull(testItem);
            Assert.AreEqual(100, testItem.Id);
        }

        /// <summary>
        /// Test Insert account via public fields
        /// </summary>
        [Ignore("No more supported")]
        public void TestInsertAccountViaPublicFields()
        {
            AccountBis account = new AccountBis();

            account.Id = 10;
            account.FirstName = "Luky";
            account.LastName = "Luke";
            account.EmailAddress = "luly.luke@somewhere.com";

            sqlMap.Insert("InsertAccountViaPublicFields", account);

            Account testAccount = sqlMap.QueryForObject("GetAccountViaColumnName", 10) as Account;

            Assert.IsNotNull(testAccount);
            Assert.AreEqual(10, testAccount.Id);
        }

        public void TestInsertOrderViaProperties()
        {
            Account account = NewAccount6();

            sqlMap.Insert("InsertAccountViaParameterMap", account);

            Order order = new Order();
            order.Id = 99;
            order.CardExpiry = "09/11";
            order.Account = account;
            order.CardNumber = "154564656";
            order.CardType = "Visa";
            order.City = "Lyon";
            order.Date = DateTime.Now;
            order.PostalCode = "69004";
            order.Province = "Rhone";
            order.Street = "rue Durand";

            sqlMap.Insert("InsertOrderViaPublicFields", order);
        }

        /// <summary>
        /// Test Insert account via public fields
        /// </summary>
        public void TestInsertDynamic()
        {
            Account account = new Account();

            account.Id = 10;
            account.FirstName = "Luky";
            account.LastName = "luke";
            account.EmailAddress = null;

            sqlMap.Insert("InsertAccountDynamic", account);

            Account testAccount = sqlMap.QueryForObject("GetAccountViaColumnIndex", 10) as Account;

            Assert.IsNotNull(testAccount);
            Assert.AreEqual(10, testAccount.Id);
            Assert.AreEqual("no_email@provided.com", testAccount.EmailAddress);

            account.Id = 11;
            account.FirstName = "Luky";
            account.LastName = "luke";
            account.EmailAddress = "luly.luke@somewhere.com";

            sqlMap.Insert("InsertAccountDynamic", account);

            testAccount = sqlMap.QueryForObject("GetAccountViaColumnIndex", 11) as Account;

            Assert.IsNotNull(testAccount);
            Assert.AreEqual(11, testAccount.Id);
            Assert.AreEqual("luly.luke@somewhere.com", testAccount.EmailAddress);
        }

        /// <summary>
        /// Test Insert account via inline parameters
        /// </summary>
        [Test]
        public void TestInsertAccountViaInlineParameters()
        {
            Account account = new Account();

            account.Id = 10;
            account.FirstName = "Luky";
            account.LastName = "Luke";
            account.EmailAddress = "luly.luke@somewhere.com";

            sqlMap.Insert("InsertAccountViaInlineParameters", account);

            Account testAccount = sqlMap.QueryForObject("GetAccountViaColumnIndex", 10) as Account;

            Assert.IsNotNull(testAccount);
            Assert.AreEqual(10, testAccount.Id);
        }

        /// <summary>
        /// Test Insert account via parameterMap
        /// </summary>
        [Test]
        public void TestInsertAccountViaParameterMap()
        {
            Account account = NewAccount6();

            sqlMap.Insert("InsertAccountViaParameterMap", account);

            account = sqlMap.QueryForObject("GetAccountNullableEmail", 6) as Account;

            AssertAccount6(account);
        }

        /// <summary>
        /// Test Insert account via parameterMap
        /// </summary>
        [Test]
        public void TestInsertEnumViaParameterMap()
        {
            Enumeration enumClass = new Enumeration();
            enumClass.Id = 99;
            enumClass.Day = Days.Thu;
            enumClass.Color = Colors.Blue;
            enumClass.Month = Months.May;

            sqlMap.Insert("InsertEnumViaParameterMap", enumClass);

            enumClass = null;
            enumClass = sqlMap.QueryForObject("GetEnumeration", 99) as Enumeration;

            Assert.AreEqual(enumClass.Day, Days.Thu);
            Assert.AreEqual(enumClass.Color, Colors.Blue);
            Assert.AreEqual(enumClass.Month, Months.May);
        }

        /// <summary>
        /// Test Update via parameterMap
        /// </summary>
        [Test]
        public void TestUpdateViaParameterMap()
        {
            Account account = (Account)sqlMap.QueryForObject("GetAccountViaColumnName", 1);

            account.EmailAddress = "new@somewhere.com";
            sqlMap.Update("UpdateAccountViaParameterMap", account);

            account = sqlMap.QueryForObject("GetAccountViaColumnName", 1) as Account;

            Assert.AreEqual("new@somewhere.com", account.EmailAddress);
        }

        /// <summary>
        /// Test Update via parameterMap V2
        /// </summary>
        [Test]
        public void TestUpdateViaParameterMap2()
        {
            Account account = (Account)sqlMap.QueryForObject("GetAccountViaColumnName", 1);

            account.EmailAddress = "new@somewhere.com";
            sqlMap.Update("UpdateAccountViaParameterMap2", account);

            account = sqlMap.QueryForObject("GetAccountViaColumnName", 1) as Account;

            Assert.AreEqual("new@somewhere.com", account.EmailAddress);
        }

        /// <summary>
        /// Test Update with inline parameters
        /// </summary>
        [Test]
        public void TestUpdateWithInlineParameters()
        {
            Account account = sqlMap.QueryForObject("GetAccountViaColumnName", 1) as Account;

            account.EmailAddress = "new@somewhere.com";
            sqlMap.Update("UpdateAccountViaInlineParameters", account);

            account = (Account)sqlMap.QueryForObject("GetAccountViaColumnName", 1);

            Assert.AreEqual("new@somewhere.com", account.EmailAddress);
        }

        /// <summary>
        /// Test Execute Update With Parameter Class
        /// </summary>
        [Test]
        public void TestExecuteUpdateWithParameterClass()
        {
            Account account = NewAccount6();

            sqlMap.Insert("InsertAccountViaParameterMap", account);

            bool checkForInvalidTypeFailedAppropriately = false;

            try
            {
                sqlMap.Update("DeleteAccount", new object());
            }
            catch (IBatisNetException e)
            {
                Console.WriteLine("TestExecuteUpdateWithParameterClass :" + e.Message);
                checkForInvalidTypeFailedAppropriately = true;
            }

            sqlMap.Update("DeleteAccount", account);

            account = sqlMap.QueryForObject("GetAccountViaColumnName", 6) as Account;

            Assert.IsNull(account);
            Assert.IsTrue(checkForInvalidTypeFailedAppropriately);
        }

        /// <summary>
        /// Test Execute Delete
        /// </summary>
        [Test]
        public void TestExecuteDelete()
        {
            Account account = NewAccount6();

            sqlMap.Insert("InsertAccountViaParameterMap", account);

            account = null;
            account = sqlMap.QueryForObject("GetAccountViaColumnName", 6) as Account;

            Assert.IsTrue(account.Id == 6);

            int rowNumber = sqlMap.Delete("DeleteAccount", account);
            Assert.IsTrue(rowNumber == 1);

            account = sqlMap.QueryForObject("GetAccountViaColumnName", 6) as Account;

            Assert.IsNull(account);
        }

        /// <summary>
        /// Test Execute Delete
        /// </summary>
        [Test]
        public void TestDeleteWithComments()
        {
            int rowNumber = sqlMap.Delete("DeleteWithComments", null);

            Assert.IsTrue(rowNumber == 3);
        }

        /// <summary>
        /// Test Execute delete Via Inline Parameters
        /// </summary>
        [Test]
        public void TestDeleteViaInlineParameters()
        {
            Account account = NewAccount6();

            sqlMap.Insert("InsertAccountViaParameterMap", account);

            int rowNumber = sqlMap.Delete("DeleteAccountViaInlineParameters", 6);

            Assert.IsTrue(rowNumber == 1);
        }

        #endregion

        #region Row delegate

        private int _index = 0;

        public void RowHandler(object obj, object paramterObject, IList list)
        {
            _index++;
            Assert.AreEqual(_index, ((Account)obj).Id);
            list.Add(obj);
        }

        #endregion

        #region Tests using syntax

        /// <summary>
        /// Test Test Using syntax on sqlMap.OpenConnection
        /// </summary>
        [Test]
        public void TestUsingConnection()
        {
            using (IDalSession session = sqlMap.OpenConnection())
            {
                Account account = sqlMap.QueryForObject("GetAccountViaColumnName", 1) as Account;
                AssertAccount1(account);
            } // compiler will call Dispose on SqlMapSession
        }

        /// <summary>
        /// Test Using syntax on sqlMap.BeginTransaction
        /// </summary>
        [Test]
        public void TestUsingTransaction()
        {
            using (IDalSession session = sqlMap.BeginTransaction())
            {
                Account account = (Account)sqlMap.QueryForObject("GetAccountViaColumnName", 1);

                account.EmailAddress = "new@somewhere.com";
                sqlMap.Update("UpdateAccountViaParameterMap", account);

                account = sqlMap.QueryForObject("GetAccountViaColumnName", 1) as Account;

                Assert.AreEqual("new@somewhere.com", account.EmailAddress);

                session.Complete(); // Commit
            } // compiler will call Dispose on SqlMapSession
        }

        /// <summary>
        /// Test Using syntax on sqlMap.BeginTransaction
        /// </summary>
        [Test]
        public void TestUsing()
        {
            sqlMap.OpenConnection();
            sqlMap.BeginTransaction(false);
            Account account = (Account)sqlMap.QueryForObject("GetAccountViaColumnName", 1);

            account.EmailAddress = "new@somewhere.com";
            sqlMap.Update("UpdateAccountViaParameterMap", account);

            account = sqlMap.QueryForObject("GetAccountViaColumnName", 1) as Account;

            Assert.AreEqual("new@somewhere.com", account.EmailAddress);

            sqlMap.CommitTransaction(false);
            sqlMap.CloseConnection();
        }

        #endregion

        #region JIRA Tests

        /// <summary>
        /// Test a constructor argument with select tag.
        /// </remarks>
        [Test]
        public void TestJIRA182()
        {
            Order order = sqlMap.QueryForObject("JIRA182", 5) as Order;

            Assert.IsTrue(order.Id == 5);
            Assert.IsNotNull(order.Account);
            Assert.AreEqual(5, order.Account.Id);
        }

        /// <summary>
        /// QueryForDictionary does not process select property
        /// </summary>
        [Test]
        public void TestJIRA220()
        {
            IDictionary map = sqlMap.QueryForMap("JIAR220", null, "PostalCode");

            Assert.AreEqual(11, map.Count);
            Order order = ((Order)map["T4H 9G4"]);

            Assert.AreEqual(2, order.LineItemsIList.Count);
        }

        /// <summary>
        /// Test JIRA 30 (repeating property)
        /// </summary>
        [Test]
        public void TestJIRA30()
        {
            Account account = new Account();
            account.Id = 1;
            account.FirstName = "Joe";
            account.LastName = "Dalton";
            account.EmailAddress = "Joe.Dalton@somewhere.com";

            Account result = sqlMap.QueryForObject("GetAccountWithRepeatingProperty", account) as Account;

            AssertAccount1(result);
        }

        /// <summary>
        /// Test Bit column 
        /// </summary>
        [Test]
        public void TestJIRA42()
        {
            Other other = new Other();

            other.Int = 100;
            other.Bool = true;
            other.Long = 789456321;

            sqlMap.Insert("InsertBool", other);
        }

        /// <summary>
        /// Test for access a result map in a different namespace 
        /// </summary>
        [Test]
        public void TestJIRA45()
        {
            Account account = sqlMap.QueryForObject("GetAccountJIRA45", 1) as Account;
            AssertAccount1(account);
        }

        /// <summary>
        /// Test : Whitespace is not maintained properly when CDATA tags are used
        /// </summary>
        [Test]
        public void TestJIRA110()
        {
            Account account = sqlMap.QueryForObject("Get1Account", null) as Account;
            AssertAccount1(account);
        }

        /// <summary>
        /// Test : Whitespace is not maintained properly when CDATA tags are used
        /// </summary>
        [Test]
        public void TestJIRA110Bis()
        {
            IList list = sqlMap.QueryForList("GetAccounts", null);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test for cache stats only being calculated on CachingStatments
        /// </summary>
        [Test]
        public void TestJIRA113()
        {
            sqlMap.FlushCaches();

            // taken from TestFlushDataCache()
            // first query is not cached, second query is: 50% cache hit
            IList list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);
            int firstId = HashCodeProvider.GetIdentityHashCode(list);
            list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);
            int secondId = HashCodeProvider.GetIdentityHashCode(list);
            Assert.AreEqual(firstId, secondId);

            string cacheStats = sqlMap.GetDataCacheStats();

            Assert.IsNotNull(cacheStats);
        }

        #endregion

        #region CustomTypeHandler tests

        /// <summary>
        /// Test CustomTypeHandler 
        /// </summary>
        [Test]
        public void TestExecuteQueryWithCustomTypeHandler()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsViaCustomTypeHandler", null);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(5, list.Count);
            Assert.AreEqual(1, ((Account)list[0]).Id);
            Assert.AreEqual(2, ((Account)list[1]).Id);
            Assert.AreEqual(3, ((Account)list[2]).Id);
            Assert.AreEqual(4, ((Account)list[3]).Id);
            Assert.AreEqual(5, ((Account)list[4]).Id);

            Assert.IsFalse(((Account)list[0]).CartOption);
            Assert.IsFalse(((Account)list[1]).CartOption);
            Assert.IsTrue(((Account)list[2]).CartOption);
            Assert.IsTrue(((Account)list[3]).CartOption);
            Assert.IsTrue(((Account)list[4]).CartOption);

            Assert.IsTrue(((Account)list[0]).BannerOption);
            Assert.IsTrue(((Account)list[1]).BannerOption);
            Assert.IsFalse(((Account)list[2]).BannerOption);
            Assert.IsFalse(((Account)list[3]).BannerOption);
            Assert.IsTrue(((Account)list[4]).BannerOption);
        }

        /// <summary>
        /// Test CustomTypeHandler Oui/Non
        /// </summary>
        [Test]
        public void TestCustomTypeHandler()
        {
            Other other = new Other();
            other.Int = 99;
            other.Long = 1966;
            other.Bool = true;
            other.Bool2 = false;

            sqlMap.Insert("InsertCustomTypeHandler", other);

            Other anOther = sqlMap.QueryForObject("SelectByInt", 99) as Other;

            Assert.IsNotNull(anOther);
            Assert.AreEqual(99, anOther.Int);
            Assert.AreEqual(1966, anOther.Long);
            Assert.AreEqual(true, anOther.Bool);
            Assert.AreEqual(false, anOther.Bool2);
        }

        /// <summary>
        /// Test CustomTypeHandler Oui/Non
        /// </summary>
        [Test]
        public void TestInsertInlineCustomTypeHandlerV1()
        {
            Other other = new Other();
            other.Int = 99;
            other.Long = 1966;
            other.Bool = true;
            other.Bool2 = false;

            sqlMap.Insert("InsertInlineCustomTypeHandlerV1", other);

            Other anOther = sqlMap.QueryForObject("SelectByInt", 99) as Other;

            Assert.IsNotNull(anOther);
            Assert.AreEqual(99, anOther.Int);
            Assert.AreEqual(1966, anOther.Long);
            Assert.AreEqual(true, anOther.Bool);
            Assert.AreEqual(false, anOther.Bool2);
        }

        /// <summary>
        /// Test CustomTypeHandler Oui/Non
        /// </summary>
        [Test]
        public void TestInsertInlineCustomTypeHandlerV2()
        {
            Other other = new Other();
            other.Int = 99;
            other.Long = 1966;
            other.Bool = true;
            other.Bool2 = false;

            sqlMap.Insert("InsertInlineCustomTypeHandlerV2", other);

            Other anOther = sqlMap.QueryForObject("SelectByInt", 99) as Other;

            Assert.IsNotNull(anOther);
            Assert.AreEqual(99, anOther.Int);
            Assert.AreEqual(1966, anOther.Long);
            Assert.AreEqual(true, anOther.Bool);
            Assert.AreEqual(false, anOther.Bool2);
        }
        #endregion
    }
}
