
using System;
using System.Collections;
#if dotnet2
using System.Collections.Generic;
#endif
using IBatisNet.DataMapper.Test.Domain;

using NUnit.Framework;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
    /// <summary>
    /// Summary description for ParameterMapTest.
    /// </summary>
    [TestFixture]
    public class MultipleResultTest : BaseTest
    {
        #region SetUp & TearDown

        /// <summary>
        /// SetUp
        /// </summary>
        [SetUp]
        public void Init()
        {
            InitScript(sqlMap.DataSource, ScriptDirectory + "account-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "category-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "documents-init.sql");
        }

        /// <summary>
        /// TearDown
        /// </summary>
        [TearDown]
        public void Dispose()
        { /* ... */
        }

        #endregion


        /// <summary>
        /// Test Multiple ResultMaps
        /// </summary>
        [Test]
        public void TestMultipleResultMapsWithInList()
        {
            IList accounts = new ArrayList();
            sqlMap.QueryForList("GetMultipleResultMapAccount", null, accounts);
            Assert.AreEqual(2, accounts.Count);
        }

        /// <summary>
        /// Test Multiple ResultMaps
        /// </summary>
        [Test]
        public void TestMultipleAccountResultMap()
        {
            Assert.AreEqual(2, sqlMap.QueryForList("GetMultipleResultMapAccount", null).Count);
        }

        /// <summary>
        /// Test Multiple ResultMaps
        /// </summary>
        [Test]
        public void TestMultipleResultClassWithInList()
        {
            IList accounts = new ArrayList();
            sqlMap.QueryForList("GetMultipleResultClassAccount", null, accounts);
            Assert.AreEqual(2, accounts.Count);
        }

        /// <summary>
        /// Test Multiple Result class
        /// </summary>
        [Test]
        public void TestMultipleAccountResultClass()
        {
            Assert.AreEqual(2, sqlMap.QueryForList("GetMultipleResultClassAccount", null).Count);
        }

        /// <summary>
        /// Test Multiple ResultMaps
        /// </summary>
        [Test]
        public void TestMultipleResultMap()
        {
            Category category = new Category();
            category.Name = "toto";
            category.Guid = Guid.NewGuid();

            int key = (int)sqlMap.Insert("InsertCategory", category);
            IList list = sqlMap.QueryForList("GetMultipleResultMap", null);
            
            Assert.AreEqual(2, list.Count);
            
            Account account = list[0] as Account;
            Category saveCategory = list[01] as Category;
            AssertAccount1(account);
            Assert.AreEqual(key, saveCategory.Id);
            Assert.AreEqual(category.Name, saveCategory.Name);
            Assert.AreEqual(category.Guid, saveCategory.Guid);
        }

        /// <summary>
        /// Test Multiple Result class
        /// </summary>
        [Test]
        public void TestMultipleResultClass()
        {
            Category category = new Category();
            category.Name = "toto";
            category.Guid = Guid.NewGuid();

            int key = (int)sqlMap.Insert("InsertCategory", category);

            IList list = sqlMap.QueryForList("GetMultipleResultClass", null);
            Assert.AreEqual(2, list.Count);
        }

#if dotnet2
        
        /// <summary>
        /// Test Multiple Document
        /// </summary>
        [Test]
        public void TestMultipleDocument()
        {
            IList<Document> list = sqlMap.QueryForList<Document>("GetMultipleDocument", null);

            Assert.AreEqual(3, list.Count);
        }
#endif        
    }
}
