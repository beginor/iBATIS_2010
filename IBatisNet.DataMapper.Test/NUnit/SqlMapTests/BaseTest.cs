
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using System.Configuration;
using IBatisNet.DataMapper.Configuration;
using log4net;

using NUnit.Framework;

using IBatisNet.Common; // DataSource definition
using IBatisNet.Common.Utilities; // ScriptRunner definition
using IBatisNet.DataMapper; // SqlMap API
using IBatisNet.DataMapper.Test.Domain;
using System.Collections.Specialized;


[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
    public delegate string KeyConvert(string key);

    /// <summary>
    /// Summary description for BaseTest.
    /// </summary>
    [TestFixture]
    public abstract class BaseTest
    {
        /// <summary>
        /// The sqlMap
        /// </summary>
        protected static ISqlMapper sqlMap = null;
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected static string ScriptDirectory = null;

        protected static KeyConvert ConvertKey = null;

        /// <summary>
        /// Constructor
        /// </summary>
        static BaseTest()
        {
#if dotnet2
            ScriptDirectory = Path.Combine(Path.Combine(Path.Combine(Path.Combine(Resources.ApplicationBase, ".."), ".."), "Scripts"),
                ConfigurationManager.AppSettings["database"]) + Path.DirectorySeparatorChar;
#else
            ScriptDirectory = Path.Combine(Path.Combine(Path.Combine(Path.Combine(Resources.ApplicationBase, ".."), ".."), "Scripts"), 
                ConfigurationSettings.AppSettings["database"]) + Path.DirectorySeparatorChar;
#endif
        }

        /// <summary>
        /// Initialize an sqlMap
        /// </summary>
        [TestFixtureSetUp]
        protected virtual void SetUpFixture()
        {
            //DateTime start = DateTime.Now;

            DomSqlMapBuilder builder = new DomSqlMapBuilder();
#if dotnet2
            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection2, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int?");
            builder.Properties = properties;

            string fileName = "sqlmap" + "_" + ConfigurationManager.AppSettings["database"] + "_" + ConfigurationManager.AppSettings["providerType"] + ".config";
#else
            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace","IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");
            builder.Properties = properties;

			string fileName = "sqlmap" + "_" + ConfigurationSettings.AppSettings["database"] + "_" + ConfigurationSettings.AppSettings["providerType"] + ".config";
#endif
            try
            {
                sqlMap = builder.Configure(fileName);
            }
            catch (Exception ex)
            {
                Exception e = ex;
                while (e != null)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace.ToString());
                    e = e.InnerException;

                }
                throw;
            }

            if (sqlMap.DataSource.DbProvider.Name.IndexOf("PostgreSql") >= 0)
            {
                BaseTest.ConvertKey = new KeyConvert(Lower);
            }
            else if (sqlMap.DataSource.DbProvider.Name.IndexOf("oracle") >= 0)
            {
                BaseTest.ConvertKey = new KeyConvert(Upper);
            }
            else
            {
                BaseTest.ConvertKey = new KeyConvert(Normal);
            }

            //			string loadTime = DateTime.Now.Subtract(start).ToString();
            //			Console.WriteLine("Loading configuration time :"+loadTime);
        }

        /// <summary>
        /// Dispose the SqlMap
        /// </summary>
        [TestFixtureTearDown]
        protected virtual void TearDownFixture()
        {
            sqlMap = null;
        }

        protected static string Normal(string key)
        {
            return key;
        }

        protected static string Upper(string key)
        {
            return key.ToUpper();
        }

        protected static string Lower(string key)
        {
            return key.ToLower();
        }

        /// <summary>
        /// Configure the SqlMap
        /// </summary>
        /// <remarks>
        /// Must verify ConfigureHandler signature.
        /// </remarks>
        /// <param name="obj">
        /// The reconfigured sqlMap.
        /// </param>
        protected static void Configure(object obj)
        {
            sqlMap = null;//(SqlMapper) obj;
        }

        /// <summary>
        /// Run a sql batch for the datasource.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="script">The sql batch</param>
        protected static void InitScript(IDataSource datasource, string script)
        {
            InitScript(datasource, script, true);
        }

        /// <summary>
        /// Run a sql batch for the datasource.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="script">The sql batch</param>
        /// <param name="doParse">parse out the statements in the sql script file.</param>
        protected static void InitScript(IDataSource datasource, string script, bool doParse)
        {
            ScriptRunner runner = new ScriptRunner();

            runner.RunScript(datasource, script, doParse);
        }

        /// <summary>
        /// Create a new account with id = 6
        /// </summary>
        /// <returns>An account</returns>
        protected Account NewAccount6()
        {
            Account account = new Account();
            account.Id = 6;
            account.FirstName = "Calamity";
            account.LastName = "Jane";
            account.EmailAddress = "no_email@provided.com";
            return account;
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=1).
        /// </summary>
        /// <param name="account">An account object</param>
        protected void AssertGilles(Account account)
        {
            Assert.AreEqual(5, account.Id, "account.Id");
            Assert.AreEqual("Gilles", account.FirstName, "account.FirstName");
            Assert.AreEqual("Bayon", account.LastName, "account.LastName");
            Assert.AreEqual("gilles.bayon@nospam.org", account.EmailAddress, "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=1).
        /// </summary>
        /// <param name="account">An account object</param>
        protected void AssertAccount1(Account account)
        {
            Assert.AreEqual(1, account.Id, "account.Id");
            Assert.AreEqual("Joe", account.FirstName, "account.FirstName");
            Assert.AreEqual("Dalton", account.LastName, "account.LastName");
            Assert.AreEqual("Joe.Dalton@somewhere.com", account.EmailAddress, "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=1).
        /// </summary>
        /// <param name="account">An account as hashtable</param>
        protected void AssertAccount1AsHashtable(Hashtable account)
        {
            Assert.AreEqual(1, (int)account["Id"], "account.Id");
            Assert.AreEqual("Joe", (string)account["FirstName"], "account.FirstName");
            Assert.AreEqual("Dalton", (string)account["LastName"], "account.LastName");
            Assert.AreEqual("Joe.Dalton@somewhere.com", (string)account["EmailAddress"], "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=1).
        /// </summary>
        /// <param name="account">An account as hashtable</param>
        protected void AssertAccount1AsHashtableForResultClass(Hashtable account)
        {
            Assert.AreEqual(1, (int)account[BaseTest.ConvertKey("Id")], "account.Id");
            Assert.AreEqual("Joe", (string)account[BaseTest.ConvertKey("FirstName")], "account.FirstName");
            Assert.AreEqual("Dalton", (string)account[BaseTest.ConvertKey("LastName")], "account.LastName");
            Assert.AreEqual("Joe.Dalton@somewhere.com", (string)account[BaseTest.ConvertKey("EmailAddress")], "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=6).
        /// </summary>
        /// <param name="account">An account object</param>
        protected void AssertAccount6(Account account)
        {
            Assert.AreEqual(6, account.Id, "account.Id");
            Assert.AreEqual("Calamity", account.FirstName, "account.FirstName");
            Assert.AreEqual("Jane", account.LastName, "account.LastName");
            Assert.IsNull(account.EmailAddress, "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input order is equal to the order(id=1).
        /// </summary>
        /// <param name="order">An order object.</param>
        protected void AssertOrder1(Order order)
        {
            DateTime date = new DateTime(2003, 2, 15, 8, 15, 00);

            Assert.AreEqual(1, order.Id, "order.Id");
            Assert.AreEqual(date.ToString(), order.Date.ToString(), "order.Date");
            Assert.AreEqual("VISA", order.CardType, "order.CardType");
            Assert.AreEqual("999999999999", order.CardNumber, "order.CardNumber");
            Assert.AreEqual("05/03", order.CardExpiry, "order.CardExpiry");
            Assert.AreEqual("11 This Street", order.Street, "order.Street");
            Assert.AreEqual("Victoria", order.City, "order.City");
            Assert.AreEqual("BC", order.Province, "order.Id");
            Assert.AreEqual("C4B 4F4", order.PostalCode, "order.PostalCode");
        }

        /// <summary>
        /// Verify that the input order is equal to the order(id=1).
        /// </summary>
        /// <param name="order">An order as hashtable.</param>
        protected void AssertOrder1AsHashtable(Hashtable order)
        {
            DateTime date = new DateTime(2003, 2, 15, 8, 15, 00);

            Assert.AreEqual(1, (int)order["Id"], "order.Id");
            Assert.AreEqual(date.ToString(), ((DateTime)order["Date"]).ToString(), "order.Date");
            Assert.AreEqual("VISA", (string)order["CardType"], "order.CardType");
            Assert.AreEqual("999999999999", (string)order["CardNumber"], "order.CardNumber");
            Assert.AreEqual("05/03", (string)order["CardExpiry"], "order.CardExpiry");
            Assert.AreEqual("11 This Street", (string)order["Street"], "order.Street");
            Assert.AreEqual("Victoria", (string)order["City"], "order.City");
            Assert.AreEqual("BC", (string)order["Province"], "order.Id");
            Assert.AreEqual("C4B 4F4", (string)order["PostalCode"], "order.PostalCode");
        }
    }
}
