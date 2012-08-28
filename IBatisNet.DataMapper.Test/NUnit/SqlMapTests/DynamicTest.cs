using System;
using System.Collections;
using System.Configuration;

using NUnit.Framework;

using IBatisNet.DataMapper.Test;
using IBatisNet.DataMapper.Test.Domain;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
    /// <summary>
    /// Summary description for DynamicTest.
    /// </summary>
    [TestFixture]
    public class DynamicTest : BaseTest
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
            InitScript(sqlMap.DataSource, ScriptDirectory + "order-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "line-item-init.sql");
        }

        /// <summary>
        /// TearDown
        /// </summary>
        [TearDown]
        public void Dispose()
        { /* ... */ }

        #endregion

        #region Dynamic tests

        /// <summary>
        /// Test Dynamic Sql On Column Selection
        /// JIRA IBATISNET-114
        /// </summary>
        [Test]
        public void TestDynamicSqlOnColumnSelection()
        {
            Account paramAccount = new Account();
            Account resultAccount = new Account();
            IList list = null;

            paramAccount.LastName = "Dalton";
            list = sqlMap.QueryForList("DynamicSqlOnColumnSelection", paramAccount);
            resultAccount = (Account)list[0];
            AssertAccount1(resultAccount);
            Assert.AreEqual(5, list.Count);

            paramAccount.LastName = "Bayon";
            list = sqlMap.QueryForList("DynamicSqlOnColumnSelection", paramAccount);
            resultAccount = (Account)list[0];
            Assert.IsNull(resultAccount.FirstName);
            Assert.IsNull(resultAccount.LastName);
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test IsNotEmpty True
        /// </summary>
        [Test]
        public void TestIsNotEmptyTrue()
        {
            IList list = sqlMap.QueryForList("DynamicIsNotEmpty", "Joe");
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(1, list.Count);
        }

        /// <summary>
        /// Test IsNotEmpty False
        /// </summary>
        [Test]
        public void TestIsNotEmptyFalse()
        {
            IList list = sqlMap.QueryForList("DynamicIsNotEmpty", "");
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test IsEqual true
        /// </summary>
        [Test]
        public void TestIsEqualTrue()
        {
            IList list = sqlMap.QueryForList("DynamicIsEqual", "Joe");
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(1, list.Count);
        }

        /// <summary>
        /// Test IsEqual False
        /// </summary>
        [Test]
        public void TestIsEqualFalse()
        {
            IList list = sqlMap.QueryForList("DynamicIsEqual", "BLAH!");
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test IsGreater true
        /// </summary>
        [Test]
        public void TestIsGreaterTrue()
        {
            IList list = sqlMap.QueryForList("DynamicIsGreater", 5);
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(1, list.Count);
        }

        /// <summary>
        /// Test IsGreater false
        /// </summary>
        [Test]
        public void TestIsGreaterFalse()
        {
            IList list = sqlMap.QueryForList("DynamicIsGreater", 1);
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test IsGreaterEqual true
        /// </summary>
        [Test]
        public void TestIsGreaterEqualTrue()
        {
            IList list = sqlMap.QueryForList("DynamicIsGreaterEqual", 3);
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(1, list.Count);
        }

        /// <summary>
        /// Test IsGreaterEqual false
        /// </summary>
        [Test]
        public void TestIsGreaterEqualFalse()
        {
            IList list = sqlMap.QueryForList("DynamicIsGreaterEqual", 1);
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test IsLess true
        /// </summary>
        [Test]
        public void TestIsLessTrue()
        {
            IList list = sqlMap.QueryForList("DynamicIsLess", 1);
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(1, list.Count);
        }

        /// <summary>
        /// Test IsLess false
        /// </summary>
        [Test]
        public void TestIsLessFalse()
        {
            IList list = sqlMap.QueryForList("DynamicIsLess", 5);
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test IsLessEqual true
        /// </summary>
        [Test]
        public void TestIsLessEqualTrue()
        {
            IList list = sqlMap.QueryForList("DynamicIsLessEqual", 3);
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(1, list.Count);
        }

        /// <summary>
        /// Test IsLessEqual false
        /// </summary>
        [Test]
        public void TestIsLessEqualFalse()
        {
            IList list = sqlMap.QueryForList("DynamicIsLessEqual", 5);
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test IsNotNull true
        /// </summary>
        [Test]
        public void TestIsNotNullTrue()
        {
            IList list = sqlMap.QueryForList("DynamicIsNotNull", "");
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(1, list.Count);
        }

        /// <summary>
        /// Test IsNotNull false
        /// </summary>
        [Test]
        public void TestIsNotNullFalse()
        {
            IList list = sqlMap.QueryForList("DynamicIsNotNull", null);
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test IsPropertyAvailable true
        /// </summary>
        [Test]
        public void TestIsPropertyAvailableTrue()
        {
            Account account = new Account();
            account.Id = 1;

            IList list = sqlMap.QueryForList("DynamicIsPropertyAvailable", account);
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(1, list.Count);
        }

        /// <summary>
        /// Test IsPropertyAvailable false
        /// </summary>
        [Test]
        public void TestIsPropertyAvailableFalse()
        {
            string parameter = "1";

            IList list = sqlMap.QueryForList("DynamicIsPropertyAvailable", parameter);
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test IsParameterPresent true
        /// </summary>
        [Test]
        public void TestIsParameterPresentTrue()
        {
            IList list = sqlMap.QueryForList("DynamicIsParameterPresent", 1);
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(1, list.Count);
        }

        /// <summary>
        /// Test IsParameterPresent false
        /// </summary>
        [Test]
        public void TestIsParameterPresentFalse()
        {
            IList list = sqlMap.QueryForList("DynamicIsParameterPresent", null);
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test Iterate 
        /// </summary>
        [Test]
        public void TestIterate()
        {
            IList parameters = new ArrayList();
            parameters.Add(1);
            parameters.Add(2);
            parameters.Add(3);

            IList list = sqlMap.QueryForList("DynamicIterate", parameters);
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(3, list.Count);
        }

        /// <summary>
        /// Test Iterate 2
        /// </summary>
        [Test]
        public void TestIterate2()
        {
            Account account = new Account();
            account.Ids = new int[3] { 1, 2, 3 };

            IList list = sqlMap.QueryForList("DynamicIterate2", account);
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(3, list.Count);
        }

        /// <summary>
        /// Test Empty Parameter Object
        /// </summary>
        [Test]
        public void TestEmptyParameterObject()
        {
            Account account = new Account();
            account.Id = -1;

            IList list = sqlMap.QueryForList("DynamicQueryByExample", account);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test Dynamic With Extend
        /// </summary>
        [Test]
        public void TestDynamicWithExtend()
        {
            Account account = new Account();
            account.Id = -1;

            IList list = sqlMap.QueryForList("DynamicWithExtend", account);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// Test Multiple Iterate
        /// </summary>
        [Test]
        public void TestMultiIterate()
        {
            IList parameters = new ArrayList();
            parameters.Add(1);
            parameters.Add(2);
            parameters.Add(3);

            IList list = sqlMap.QueryForList("MultiDynamicIterate", parameters);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(3, list.Count);
        }

        /// <summary>
        /// Test Array Property Iterate
        /// </summary>
        [Test]
        public void TestArrayPropertyIterate()
        {
            Account account = new Account();
            account.Ids = new int[3] { 1, 2, 3 };

            IList list = sqlMap.QueryForList("DynamicQueryByExample", account);

            AssertAccount1((Account)list[0]);
            Assert.AreEqual(3, list.Count);
        }

        /// <summary>
        /// Test Complete Statement Substitution
        /// </summary>
        [Test]
        [Ignore("No longer supported.")]
        public void TestCompleteStatementSubst()
        {
            string statement = "select" +
            "    Account_ID			as Id," +
            "    Account_FirstName	as FirstName," +
            "    Account_LastName	as LastName," +
            "    Account_Email		as EmailAddress" +
            "  from Accounts" +
            "  where Account_ID = #id#";
            int id = 1;

            Hashtable parameters = new Hashtable();
            parameters.Add("id", id);
            parameters.Add("statement", statement);

            IList list = sqlMap.QueryForList("DynamicSubst", parameters);
            AssertAccount1((Account)list[0]);
            Assert.AreEqual(1, list.Count);
        }

        /// <summary>
        /// Test Query By Example
        /// </summary>
        [Test]
        public void TestQueryByExample()
        {
            Account account;

            account = new Account();

            account.Id = 5;
            account = sqlMap.QueryForObject("DynamicQueryByExample", account) as Account;
            AssertGilles(account);

            account = new Account();
            account.FirstName = "Gilles";
            account = sqlMap.QueryForObject("DynamicQueryByExample", account) as Account;
            AssertGilles(account);

            account = new Account();
            account.LastName = "Bayon";
            account = sqlMap.QueryForObject("DynamicQueryByExample", account) as Account;
            AssertGilles(account);

            account = new Account();
            account.EmailAddress = "gilles";
            account = (Account)sqlMap.QueryForObject("DynamicQueryByExample", account) as Account;
            AssertGilles(account);

            account = new Account();
            account.Id = 5;
            account.FirstName = "Gilles";
            account.LastName = "Bayon";
            account.EmailAddress = "gilles.bayon@nospam.org";
            account = sqlMap.QueryForObject("DynamicQueryByExample", account) as Account;
            AssertGilles(account);
        }

        /// <summary>
        /// Test Query By Example via private field
        /// </summary>
        [Test]
        public void TestQueryByExampleViaField()
        {
            Account account;

            account = new Account();

            account.Id = 5;
            account = sqlMap.QueryForObject("DynamicQueryByExampleViaPrivateField", account) as Account;
            AssertGilles(account);

            account = new Account();
            account.FirstName = "Gilles";
            account = sqlMap.QueryForObject("DynamicQueryByExampleViaPrivateField", account) as Account;
            AssertGilles(account);

            account = new Account();
            account.LastName = "Bayon";
            account = sqlMap.QueryForObject("DynamicQueryByExampleViaPrivateField", account) as Account;
            AssertGilles(account);

            account = new Account();
            account.EmailAddress = "gilles";
            account = (Account)sqlMap.QueryForObject("DynamicQueryByExampleViaPrivateField", account) as Account;
            AssertGilles(account);

            account = new Account();
            account.Id = 5;
            account.FirstName = "Gilles";
            account.LastName = "Bayon";
            account.EmailAddress = "gilles.bayon@nospam.org";
            account = sqlMap.QueryForObject("DynamicQueryByExampleViaPrivateField", account) as Account;
            AssertGilles(account);
        }
        #endregion
    }
}
