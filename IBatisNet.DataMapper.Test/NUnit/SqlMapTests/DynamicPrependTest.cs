using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Threading;

using NUnit.Framework;

using IBatisNet.DataMapper.Test;
using IBatisNet.DataMapper.Test.Domain;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
    /// <summary>
    /// Summary description for DynamicPrependTest.
    /// </summary>
    [TestFixture]
    public class DynamicPrependTest : BaseTest
    {
        #region SetUp & TearDown

        /// <summary>
        /// SetUp
        /// </summary>
        [SetUp]
        public void Init()
        {
            InitScript(sqlMap.DataSource, ScriptDirectory + "account-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "account-procedure.sql", false);
            InitScript(sqlMap.DataSource, ScriptDirectory + "category-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "order-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "line-item-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "other-init.sql");
        }

        /// <summary>
        /// TearDown
        /// </summary>
        [TearDown]
        public void Dispose()
        { /* ... */ }

        #endregion

        #region Dynamic Prepend tests

        /// <summary>
        /// Test Dynamic With Prepend (1)
        /// </summary>
        [Test]
        public void TestDynamicJIRA168()
        {
            Query query = new Query();
            Account account = new Account();
            account.Id = 1;
            query.DataObject = account;

            account = (Account)sqlMap.QueryForObject("DynamicJIRA168", query);

            AssertAccount1(account);
        }

        /// <summary>
        /// Test Iterate With Prepend (1)
        /// </summary>
        [Test]
        public void TestIterateWithPrepend1()
        {
            IList parameters = new ArrayList();
            parameters.Add(1);
            parameters.Add(2);
            parameters.Add(3);

            IList list = sqlMap.QueryForList("DynamicIterateWithPrepend1", parameters);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(3, list.Count);
        }

        /// <summary>
        /// Test Iterate With Prepend (2)
        /// </summary>
        [Test]
        public void TestIterateWithPrepend2()
        {
            IList parameters = new ArrayList();
            parameters.Add(1);
            parameters.Add(2);
            parameters.Add(3);

            IList list = sqlMap.QueryForList("DynamicIterateWithPrepend2", parameters);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(3, list.Count);
        }

        /// <summary>
        /// Test Iterate With Prepend (3)
        /// </summary>
        [Test]
        public void TestIterateWithPrepend3()
        {
            IList parameters = new ArrayList();
            parameters.Add(1);
            parameters.Add(2);
            parameters.Add(3);

            IList list = sqlMap.QueryForList("DynamicIterateWithPrepend3", parameters);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(3, list.Count);
        }

        /// <summary>
        /// Test Dynamic With Prepend (1)
        /// </summary>
        [Test]
        public void TestDynamicWithPrepend1()
        {
            Account account = new Account();
            account.Id = 1;

            account = (Account)sqlMap.QueryForObject("DynamicWithPrepend", account);

            AssertAccount1(account);
        }

        /// <summary>
        /// Test Dynamic With Prepend (2)
        /// </summary>
        [Test]
        public void TestDynamicWithPrepend2()
        {
            Account account = new Account();
            account.Id = 1;
            account.FirstName = "Joe";

            account = (Account)sqlMap.QueryForObject("DynamicWithPrepend", account);
            AssertAccount1(account);

        }

        /// <summary>
        /// Test Dynamic With Prepend (3)
        /// </summary>
        [Test]
        public void TestDynamicWithPrepend3()
        {
            Account account = new Account();
            account.Id = 1;
            account.FirstName = "Joe";
            account.LastName = "Dalton";

            account = (Account)sqlMap.QueryForObject("DynamicWithPrepend", account);
            AssertAccount1(account);
        }

        /// <summary>
        /// Test Dynamic With Prepend (4)
        /// </summary>
        [Test]
        public void TestDynamicWithPrepend4()
        {
            IList list = sqlMap.QueryForList("DynamicWithPrepend", null);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test Iterate With Two Prepends
        /// </summary>
        [Test]
        public void TestIterateWithTwoPrepends()
        {
            Account account = new Account();
            account.Id = 1;
            account.FirstName = "Joe";

            account = sqlMap.QueryForObject("DynamicWithPrepend", account) as Account;
            Assert.IsNotNull(account);
            AssertAccount1(account);

            IList list = sqlMap.QueryForList("DynamicWithTwoDynamicElements", account);
            AssertAccount1((Account)list[0]);
        }

        /// <summary>
        /// Test Complex Dynamic
        /// </summary>
        [Test]
        public void TestComplexDynamic()
        {
            Account account = new Account();
            account.Id = 1;
            account.FirstName = "Joe";
            account.LastName = "Dalton";

            IList list = sqlMap.QueryForList("ComplexDynamicStatement", account);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(1, list.Count);
        }

        /// <summary>
        /// Test GetAccounts Dynamic
        /// </summary>
        /// <remarks>
        /// Bug Fix http://sourceforge.net/forum/message.php?msg_id=2646964
        /// </remarks>
        [Test]
        public void TestGetAccountsDynamic()
        {
            Hashtable map = new Hashtable();
            map.Add("MaximumAllowed", 100);

            map.Add("FirstName", "Joe");
            map.Add("LastName", "Dalton");
            map.Add("EmailAddress", "Joe.Dalton@somewhere.com");

            IList list = sqlMap.QueryForList("GetAccountsDynamic", map);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(1, list.Count);
        }

        /// <summary>
        /// Test IsEqual with HashTable
        /// </summary>
        /// <remarks>
        /// Bug Fix https://sourceforge.net/forum/message.php?msg_id=2840259
        /// </remarks>
        [Test]
        public void TestDynamicSelectByIntLong()
        {
            Hashtable search = new Hashtable();
            search.Add("year", 0);
            search.Add("areaid", 0);

            IList list = sqlMap.QueryForList("DynamicSelectByIntLong", search);

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, (list[0] as Other).Int);
            Assert.AreEqual(8888888, (list[0] as Other).Long);
            Assert.AreEqual(false, (list[0] as Other).Bool);

            Assert.AreEqual(2, (list[1] as Other).Int);
            Assert.AreEqual(9999999999, (list[1] as Other).Long);
            Assert.AreEqual(true, (list[1] as Other).Bool);

            //----------------------
            search.Clear();
            search.Add("year", 1);
            search.Add("areaid", 0);

            list = null;
            list = sqlMap.QueryForList("DynamicSelectByIntLong", search);

            Assert.AreEqual(1, list.Count);
            //----------------------
            search.Clear();
            search.Add("year", 0);
            search.Add("areaid", 9999999999);

            list = null;
            list = sqlMap.QueryForList("DynamicSelectByIntLong", search);

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(2, (list[0] as Other).Int);
            //----------------------
            search.Clear();
            search.Add("year", 2);
            search.Add("areaid", 9999999999);

            list = null;
            list = sqlMap.QueryForList("DynamicSelectByIntLong", search);

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(2, (list[0] as Other).Int);
            Assert.AreEqual(9999999999, (list[0] as Other).Long);
            Assert.AreEqual(true, (list[0] as Other).Bool);
        }

        /// <summary>
        /// Test Dynamic With GUID
        /// </summary>
        [Test]
        public void TestDynamicWithGUID()
        {
            Category category = new Category();
            category.Name = "toto";
            category.Guid = Guid.Empty;

            int key = (int)sqlMap.Insert("InsertCategory", category);

            category = new Category();
            category.Name = "titi";
            category.Guid = Guid.NewGuid();

            Category categoryTest = (Category)sqlMap.QueryForObject("DynamicGuid", category);
            Assert.IsNull(categoryTest);

            category = new Category();
            category.Name = "titi";
            category.Guid = Guid.Empty;

            categoryTest = (Category)sqlMap.QueryForObject("DynamicGuid", category);
            Assert.IsNotNull(categoryTest);
        }

        /// <summary>
        /// Test JIRA 11
        /// </summary>
        /// <remarks>
        /// To test only for MSSQL with .NET SqlClient provider
        /// </remarks>
        [Test]
        [Category("MSSQL")]
        public void TestJIRA11()
        {
            Search search = new Search();
            search.NumberSearch = 123;
            search.StartDate = new DateTime(2004, 12, 25);
            search.Operande = "like";
            search.StartDateAnd = true;

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");

            IList list = sqlMap.QueryForList("Jira-IBATISNET-11", search);

            Thread.CurrentThread.CurrentCulture = currentCulture;

            Assert.AreEqual(0, list.Count);
        }

        /// <summary>
        /// We've been using the stable version of Ibatis for a while and recently
        /// upgraded to the latest alpha version 1.1.0.458. The old version was
        /// able to handle mapping the .Net bool type to SqlServer's Bit column.
        /// When we run sql maps that contain a bool property, we now get an
        /// exception.
        /// </summary>
        /// <remarks>
        /// No problems !!
        /// </remarks>
        [Test]
        public void TestDynamicSelectByBool()
        {
            Other other = new Other();
            other.Bool = true;

            Other anOther = sqlMap.QueryForObject("DynamicSelectByBool", other) as Other;

            Assert.IsNotNull(anOther);
            Assert.AreEqual(2, anOther.Int);
            Assert.AreEqual(9999999999, anOther.Long);
            Assert.AreEqual(true, anOther.Bool);

            other.Bool = false;
            anOther = sqlMap.QueryForObject("DynamicSelectByBool", other) as Other;

            Assert.IsNotNull(anOther);
            Assert.AreEqual(1, anOther.Int);
            Assert.AreEqual(8888888, anOther.Long);
            Assert.AreEqual(false, anOther.Bool);

        }

        /// <summary>
        /// Test JIRA 29
        /// </summary>
        [Test]
        [Category("MSSQL")]
        public void TestJIRA29()
        {
            Hashtable param = new Hashtable();
            param["Foo"] = new DateTime(2003, 2, 17, 8, 15, 0);

            Order order = sqlMap.QueryForObject("SelectOrderByDate", param) as Order;

            Assert.IsNotNull(order);

            Assert.AreEqual(11, order.Id);

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            order = sqlMap.QueryForObject("SelectOrderByDateDynamic", param) as Order;

            Thread.CurrentThread.CurrentCulture = currentCulture;

            Assert.IsNotNull(order);

            Assert.AreEqual(11, order.Id);
        }

        /// <summary>
        /// Test JIRA 29
        /// </summary>
        [Test]
        public void Test2ForJIRA29()
        {
            // init
            Account account = new Account();

            account.Id = 1234;
            account.FirstName = "#The Pound Signs#";
            account.LastName = "Gilles";
            account.EmailAddress = "a.a@somewhere.com";

            sqlMap.Insert("InsertAccountViaInlineParameters", account);

            // test
            Hashtable param = new Hashtable();
            param["AccountName"] = "The Pound Signs";

            Account testAccount = sqlMap.QueryForObject("SelectAccountJIRA29", param) as Account;

            Assert.IsNotNull(testAccount);
            Assert.AreEqual(1234, testAccount.Id);
            Assert.AreEqual("#The Pound Signs#", testAccount.FirstName);
        }

        /// <summary>
        /// Test JIRA 29
        /// </summary>
        [Test]
        public void Test3ForJIRA29()
        {
            // init
            Account account = new Account();

            account.Id = 1234;
            account.FirstName = "#The Pound Signs#";
            account.LastName = "Gilles";
            account.EmailAddress = "a.a@somewhere.com";

            sqlMap.Insert("InsertAccountViaInlineParameters", account);

            // test
            Hashtable param = new Hashtable();
            param["Foo"] = "The Pound Signs";

            Account testAccount = sqlMap.QueryForObject("SelectAccountJIRA29-2", param) as Account;

            Assert.IsNotNull(testAccount);
            Assert.AreEqual(1234, testAccount.Id);
            Assert.AreEqual("#The Pound Signs#", testAccount.FirstName);
        }

        [Test]
        public void TestSelectKeyWithDynamicSql()
        {
            Account account = new Account();
            account.Id = 99998;
            account.FirstName = "R";
            account.LastName = "G";

            Hashtable param = new Hashtable(2);
            param["Account"] = account;
            param["AccountsTableName"] = "Accounts";
            object selectKeyValue = sqlMap.Insert("SelectKeyWithDynamicSql", param);

            Assert.IsNotNull(selectKeyValue);
            Assert.AreEqual(99998, Convert.ToInt32(selectKeyValue));

            Assert.IsTrue(param.ContainsKey("AccountId"));
            Assert.AreEqual(99998, (int)param["AccountId"]);
        }
        #endregion

    }
}
