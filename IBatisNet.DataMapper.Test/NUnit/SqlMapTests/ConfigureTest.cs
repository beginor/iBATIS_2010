using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper; // SqlMap API
using IBatisNet.DataMapper.Configuration;
using IBatisNet.DataMapper.SessionStore;
using IBatisNet.DataMapper.Test.Domain;
using NUnit.Framework;
using System.Collections.Specialized;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Description résumée de ConfigureTest.
	/// </summary>
	[TestFixture] 
	public class ConfigureTest : BaseTest 
	{
		private string _fileName = string.Empty;

		#region SetUp

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
#if dotnet2
            _fileName = "sqlmap" + "_" + ConfigurationManager.AppSettings["database"] + "_" + ConfigurationManager.AppSettings["providerType"] + ".config";
#else
			_fileName = "sqlmap" + "_" + ConfigurationSettings.AppSettings["database"] + "_" + ConfigurationSettings.AppSettings["providerType"] + ".config";		
#endif

        }
		#endregion 
	    
	    /// <summary>
        /// Test HybridWebThreadSessionStore
        /// </summary>
        [Test]
        public void HybridWebThreadSessionStoreTest()
	    {
            sqlMap.SessionStore = new HybridWebThreadSessionStore(sqlMap.Id);
	        
            Account account = sqlMap.QueryForObject("SelectWithProperty", null) as Account;
            AssertAccount1(account);
	    }
	    

		#region Relatives Path tests

		/// <summary>
		/// Test Configure via relative path
		/// </summary>
		[Test] 
		public void TestConfigureRelativePath()
		{
			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");
            builder.Properties = properties;

            ISqlMapper mapper = builder.Configure(_fileName);

			Assert.IsNotNull(mapper);
		}

		/// <summary>
		/// Test ConfigureAndWatch via relative path
		/// </summary>
		[Test] 
		public void TestConfigureAndWatchRelativePath()
		{
			ConfigureHandler handler = new ConfigureHandler(Configure);


			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.ConfigureAndWatch(_fileName, handler);

			Assert.IsNotNull(mapper);
		}

		/// <summary>
		/// Test Configure via relative path
		/// </summary>
		[Test] 
		public void TestConfigureRelativePathViaBuilder()
		{
			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.Configure(_fileName);

			Assert.IsNotNull(mapper);
		}

		/// <summary>
		/// Test ConfigureAndWatch via relative path
		/// </summary>
		[Test] 
		public void TestConfigureAndWatchRelativePathViaBuilder()
		{
			ConfigureHandler handler = new ConfigureHandler(Configure);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.ConfigureAndWatch(_fileName, handler);

			Assert.IsNotNull(mapper);
		}
		#endregion 

		#region Absolute Paths

		/// <summary>
		/// Test Configure via absolute path
		/// </summary>
		[Test] 
		public void TestConfigureAbsolutePath()
		{
			_fileName = Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;

			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");
            builder.Properties = properties;

            ISqlMapper mapper = builder.Configure(_fileName);

			Assert.IsNotNull(mapper);
		}

		/// <summary>
		/// Test Configure via absolute path
		/// </summary>
		[Test] 
		public void TestConfigureAbsolutePathViaBuilder()
		{
			_fileName = Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;

			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.Configure(_fileName);

			Assert.IsNotNull(mapper);
		}

		/// <summary>
		/// Test Configure via absolute path with file suffix
		/// </summary>
		[Test] 
		public void TestConfigureAbsolutePathWithFileSuffix()
		{
			_fileName = "file://"+Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;

			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.Configure(_fileName);

			Assert.IsNotNull(mapper);
		}

		/// <summary>
		/// Test Configure via absolute path with file suffix
		/// </summary>
		[Test] 
		public void TestConfigureAbsolutePathWithFileSuffixViaBuilder()
		{
			_fileName = "file://"+Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;

			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.Configure(_fileName);

			Assert.IsNotNull(mapper);
		}

		/// <summary>
		/// Test Configure via absolute path via FileIfno
		/// </summary>
		[Test] 
		public void TestConfigureAbsolutePathViaFileInfo()
		{
			_fileName = Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;
			FileInfo fileInfo = new FileInfo(_fileName);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.Configure(fileInfo);

			Assert.IsNotNull(mapper);
		}

		/// <summary>
		/// Test Configure via absolute path via Uri
		/// </summary>
		[Test] 
		public void TestConfigureAbsolutePathViaUri()
		{
			_fileName = Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;
			Uri uri = new Uri(_fileName);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.Configure(uri);

			Assert.IsNotNull(mapper);
		}

		/// <summary>
		/// Test Configure via absolute path
		/// </summary>
		[Test] 
		public void TestConfigureAndWatchAbsolutePath()
		{
			_fileName = Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;
			ConfigureHandler handler = new ConfigureHandler(Configure);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.ConfigureAndWatch(_fileName, handler);

			Assert.IsNotNull(mapper);
		}

		/// <summary>
		/// Test Configure via absolute path
		/// </summary>
		[Test] 
		public void TestConfigureAndWatchAbsolutePathViaBuilder()
		{
			_fileName = Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;
			ConfigureHandler handler = new ConfigureHandler(Configure);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.ConfigureAndWatch(_fileName, handler);

			Assert.IsNotNull(mapper);
		}

		/// <summary>
		/// Test Configure via absolute path 
		/// </summary>
		[Test] 
		public void TestConfigureAndWatchAbsolutePathWithFileSuffix()
		{
			_fileName = "file://"+Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;
			ConfigureHandler handler = new ConfigureHandler(Configure);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.ConfigureAndWatch(_fileName, handler);

			Assert.IsNotNull(mapper);
		}

		/// <summary>
		/// Test Configure via absolute path 
		/// </summary>
		[Test] 
		public void TestConfigureAndWatchAbsolutePathWithFileSuffixViaBuilder()
		{
			_fileName = "file://"+Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;
			ConfigureHandler handler = new ConfigureHandler(Configure);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.ConfigureAndWatch(_fileName, handler);

			Assert.IsNotNull(mapper);
		}

		/// <summary>
		/// Test Configure via absolute path via FileInfo
		/// </summary>
		[Test] 
		public void TestConfigureAndWatchAbsolutePathViaFileInfo()
		{
			_fileName = Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;
			FileInfo fileInfo = new FileInfo(_fileName);

			ConfigureHandler handler = new ConfigureHandler(Configure);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.ConfigureAndWatch(fileInfo, handler);

			Assert.IsNotNull(mapper);
		}
		#endregion 

		#region Stream / Embedded 

		/// <summary>
		/// Test Configure via Stream/embedded
		/// </summary>
		[Test] 
		public void TestConfigureViaStream()
		{
			// embeddedResource = "SqlMap_MSSQL_SqlClient.config, IBatisNet.DataMapper.Test";
			
#if dotnet2
            Assembly assembly = Assembly.Load("IBatisNet.DataMapper.Test");
#else
			Assembly assembly = Assembly.LoadWithPartialName ("IBatisNet.DataMapper.Test");
#endif
			Stream stream = assembly.GetManifestResourceStream("IBatisNet.DataMapper.Test.SqlMap_MSSQL_SqlClient.config");

			DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.Configure(stream);

			Assert.IsNotNull(mapper);
		}
		#endregion 


        private bool _hasChanged = false;

        /// <summary>
        /// ConfigurationWatcher Test
        /// </summary>
        [Test]
        public void ConfigurationWatcherTestOnSqlMapConfig()
        {
            //string fileName = @"..\..\Maps\MSSQL\SqlClient\Account.xml";

   			ConfigureHandler handler = new ConfigureHandler(MyHandler);

            DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.ConfigureAndWatch(_fileName, handler);

            // test that the mapper was correct build
            Assert.IsNotNull(mapper);

            FileInfo fi = Resources.GetFileInfo(_fileName);
            fi.LastWriteTime = DateTime.Now;

            fi.Refresh();

            // Let's give a small bit of time for the change to propagate.
            // The ConfigWatcherHandler class has a timer which 
            // waits for 500 Millis before delivering
            // the event notification.
            System.Threading.Thread.Sleep(600);

            Assert.IsTrue(_hasChanged);
            
            _hasChanged = false;
            
        }

        /// <summary>
        /// ConfigurationWatcher Test
        /// </summary>
        [Test]
        public void ConfigurationWatcherTestOnMappingFile()
        {
            string fileName = @"..\..\Maps\MSSQL\SqlClient\Account.xml";

            ConfigureHandler handler = new ConfigureHandler(MyHandler);

            DomSqlMapBuilder builder = new DomSqlMapBuilder();

            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test");
            properties.Add("nullableInt", "int");

            builder.Properties = properties;

            ISqlMapper mapper = builder.ConfigureAndWatch(_fileName, handler);

            // test that the mapper was correct build
            Assert.IsNotNull(mapper);

            FileInfo fi = Resources.GetFileInfo(fileName);
            fi.LastWriteTime = DateTime.Now;

            fi.Refresh();

            // Let's give a small bit of time for the change to propagate.
            // The ConfigWatcherHandler class has a timer which 
            // waits for 500 Millis before delivering
            // the event notification.
            System.Threading.Thread.Sleep(600);

            Assert.IsTrue(_hasChanged);

            _hasChanged = false;

        }

        protected void MyHandler(object obj)
        {
            _hasChanged = true;
        }

	}
}
