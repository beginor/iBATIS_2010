
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 638539 $
 * $Date: 2008-03-18 13:53:02 -0600 (Tue, 18 Mar 2008) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

#region Using

using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using IBatisNet.Common;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Logging;
using IBatisNet.Common.Utilities;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.Common.Utilities.Objects.Members;
using IBatisNet.Common.Xml;
using IBatisNet.DataMapper.Configuration.Alias;
using IBatisNet.DataMapper.Configuration.Cache;
using IBatisNet.DataMapper.Configuration.Cache.Fifo;
using IBatisNet.DataMapper.Configuration.Cache.Lru;
using IBatisNet.DataMapper.Configuration.Cache.Memory;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Configuration.Serializers;
using IBatisNet.DataMapper.Configuration.Sql;
using IBatisNet.DataMapper.Configuration.Sql.Dynamic;
using IBatisNet.DataMapper.Configuration.Sql.Dynamic.Elements;
using IBatisNet.DataMapper.Configuration.Sql.SimpleDynamic;
using IBatisNet.DataMapper.Configuration.Sql.Static;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.MappedStatements.ArgumentStrategy;
using IBatisNet.DataMapper.MappedStatements.PropertyStrategy;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.TypeHandlers;

#endregion

namespace IBatisNet.DataMapper.Configuration
{
	/// <summary>
	/// Builds an ISqlMapper instance from the supplied resources (e.g. XML configuration files).
	/// </summary>
	public class DomSqlMapBuilder
	{
		#region Embedded resource

		// Which files must we allow to be used as Embedded Resources ?
		// - slqMap.config [Yes]
		// - providers.config [Yes]
		// - sqlMap files [Yes]
		// - properties file (like Database.config) [Yes]
		// see contribution, NHibernate usage,
		// see http://www.codeproject.com/csharp/EmbeddedResourceStrings.asp
		// see http://www.devhood.com/tutorials/tutorial_details.aspx?tutorial_id=75
		#endregion

		#region Constant

		private const string PROPERTY_ELEMENT_KEY_ATTRIB = "key";
		private const string PROPERTY_ELEMENT_VALUE_ATTRIB = "value";

		/// <summary>
		/// 
		/// </summary>
		private const string DATAMAPPER_NAMESPACE_PREFIX = "mapper";
		private const string PROVIDERS_NAMESPACE_PREFIX = "provider";
		private const string MAPPING_NAMESPACE_PREFIX = "mapping";
		private const string DATAMAPPER_XML_NAMESPACE = "http://ibatis.apache.org/dataMapper";
		private const string PROVIDER_XML_NAMESPACE = "http://ibatis.apache.org/providers";
		private const string MAPPING_XML_NAMESPACE = "http://ibatis.apache.org/mapping";

		/// <summary>
		/// Default filename of main configuration file.
		/// </summary>
		public const string DEFAULT_FILE_CONFIG_NAME = "SqlMap.config";

		/// <summary>
		/// Default provider name
		/// </summary>
		private const string DEFAULT_PROVIDER_NAME = "_DEFAULT_PROVIDER_NAME";

		/// <summary>
		/// Dot representation.
		/// </summary>
		public const string DOT = ".";

		/// <summary>
		/// Token for SqlMapConfig xml root element.
		/// </summary>
		private const string XML_DATAMAPPER_CONFIG_ROOT = "sqlMapConfig";

		/// <summary>
		/// Token for xml path to SqlMapConfig settings element.
		/// </summary>
		private const string XML_CONFIG_SETTINGS = "sqlMapConfig/settings/setting";

		/// <summary>
		/// Token for default providers config file name.
		/// </summary>
		private const string PROVIDERS_FILE_NAME = "providers.config";

		/// <summary>
		/// Token for xml path to SqlMapConfig providers element.
		/// </summary>
		private const string XML_CONFIG_PROVIDERS = "sqlMapConfig/providers";

		/// <summary>
		/// Token for xml path to properties elements.
		/// </summary>
		private const string XML_PROPERTIES = "properties";

		/// <summary>
		/// Token for xml path to property elements.
		/// </summary>
		private const string XML_PROPERTY = "property";

		/// <summary>
		/// Token for xml path to settings add elements.
		/// </summary>
		private const string XML_SETTINGS_ADD = "/*/add";

		/// <summary>
		/// Token for xml path to global properties elements.
		/// </summary>
		private const string XML_GLOBAL_PROPERTIES = "*/add";

		/// <summary>
		/// Token for xml path to provider elements.
		/// </summary>
		private const string XML_PROVIDER = "providers/provider";

		/// <summary>
		/// Token for xml path to database provider elements.
		/// </summary>
		private const string XML_DATABASE_PROVIDER = "sqlMapConfig/database/provider";

		/// <summary>
		/// Token for xml path to database source elements.
		/// </summary>
		private const string XML_DATABASE_DATASOURCE = "sqlMapConfig/database/dataSource";

		/// <summary>
		/// Token for xml path to global type alias elements.
		/// </summary>
		private const string XML_GLOBAL_TYPEALIAS = "sqlMapConfig/alias/typeAlias";

		/// <summary>
		/// Token for xml path to global type alias elements.
		/// </summary>
		private const string XML_GLOBAL_TYPEHANDLER = "sqlMapConfig/typeHandlers/typeHandler";

		/// <summary>
		/// Token for xml path to sqlMap elements.
		/// </summary>
		private const string XML_SQLMAP = "sqlMapConfig/sqlMaps/sqlMap";

		/// <summary>
		/// Token for mapping xml root.
		/// </summary>
		private const string XML_MAPPING_ROOT = "sqlMap";

		/// <summary>
		/// Token for xml path to type alias elements.
		/// </summary>
		private const string XML_TYPEALIAS = "sqlMap/alias/typeAlias";

		/// <summary>
		/// Token for xml path to resultMap elements.
		/// </summary>
		private const string XML_RESULTMAP = "sqlMap/resultMaps/resultMap";

		/// <summary>
		/// Token for xml path to parameterMap elements.
		/// </summary>
		private const string XML_PARAMETERMAP = "sqlMap/parameterMaps/parameterMap";

        /// <summary>
        /// Token for xml path to sql elements.
        /// </summary>
        private const string SQL_STATEMENT = "sqlMap/statements/sql";
     	    
		/// <summary>
		/// Token for xml path to statement elements.
		/// </summary>
		private const string XML_STATEMENT = "sqlMap/statements/statement";

		/// <summary>
		/// Token for xml path to select elements.
		/// </summary>
		private const string XML_SELECT = "sqlMap/statements/select";

		/// <summary>
		/// Token for xml path to insert elements.
		/// </summary>
		private const string XML_INSERT = "sqlMap/statements/insert";

		/// <summary>
		/// Token for xml path to selectKey elements.
		/// </summary>
		private const string XML_SELECTKEY= "selectKey";

		/// <summary>
		/// Token for xml path to update elements.
		/// </summary>
		private const string XML_UPDATE ="sqlMap/statements/update";

		/// <summary>
		/// Token for xml path to delete elements.
		/// </summary>
		private const string XML_DELETE ="sqlMap/statements/delete";

		/// <summary>
		/// Token for xml path to procedure elements.
		/// </summary>
		private const string XML_PROCEDURE ="sqlMap/statements/procedure";

		/// <summary>
		/// Token for xml path to cacheModel elements.
		/// </summary>
		private const string XML_CACHE_MODEL = "sqlMap/cacheModels/cacheModel";

		/// <summary>
		/// Token for xml path to flushOnExecute elements.
		/// </summary>
		private const string XML_FLUSH_ON_EXECUTE = "flushOnExecute";

		/// <summary>
		/// Token for xml path to search statement elements.
		/// </summary>
		private const string XML_SEARCH_STATEMENT ="sqlMap/statements";

		/// <summary>
		/// Token for xml path to search parameterMap elements.
		/// </summary>
		private const string XML_SEARCH_PARAMETER ="sqlMap/parameterMaps/parameterMap[@id='";

		/// <summary>
		/// Token for xml path to search resultMap elements.
		/// </summary>
		private const string XML_SEARCH_RESULTMAP ="sqlMap/resultMaps/resultMap[@id='";

		/// <summary>
		/// Token for useStatementNamespaces attribute.
		/// </summary>
		private const string ATR_USE_STATEMENT_NAMESPACES = "useStatementNamespaces";
		/// <summary>
		/// Token for cacheModelsEnabled attribute.
		/// </summary>
		private const string ATR_CACHE_MODELS_ENABLED = "cacheModelsEnabled";

		/// <summary>
		/// Token for validateSqlMap attribute.
		/// </summary>
		private const string ATR_VALIDATE_SQLMAP = "validateSqlMap";
		/// <summary>
		/// Token for useReflectionOptimizer attribute.
		/// </summary>
		private const string ATR_USE_REFLECTION_OPTIMIZER = "useReflectionOptimizer";

		#endregion

		#region Fields

		private static readonly ILog _logger = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );

		private ConfigurationScope _configScope = null;
		private DeSerializerFactory _deSerializerFactory = null; 
		private InlineParameterMapParser _paramParser = null;
        private IObjectFactory _objectFactory = null;
        private ISetAccessorFactory _setAccessorFactory = null;
        private IGetAccessorFactory _getAccessorFactory = null;
	    private ISqlMapper _sqlMapper = null;
        private bool _validateSqlMapConfig = true;

		#endregion 		
		
		#region Properties
        
        /// <summary>
        /// Allow properties to be set before configuration.
        /// </summary>
        public NameValueCollection Properties
        {
            set { _configScope.Properties.Add(value); }
        }

        /// <summary>
        /// Allow a custom <see cref="ISetAccessorFactory"/> to be set before configuration.
        /// </summary>
        public ISetAccessorFactory SetAccessorFactory
        {
            set { _setAccessorFactory = value; }
        }

        /// <summary>
        /// Allow a custom <see cref="IGetAccessorFactory"/> to be set before configuration.
        /// </summary>
        public IGetAccessorFactory GetAccessorFactory
        {
            set { _getAccessorFactory = value; }
        }

        /// <summary>
        /// Allow a custom <see cref="IObjectFactory"/> to be set before configuration.
        /// </summary>
        public IObjectFactory ObjectFactory
        {
            set { _objectFactory = value; }
        }

        /// <summary>
        /// Allow a custom <see cref="ISqlMapper"/> to be set before configuration.
        /// </summary>
        public ISqlMapper SqlMapper
        {
            set { _sqlMapper = value; }
        }
	    
        /// <summary>
        /// Enable validation of SqlMap document. This property must be set before configuration.
        /// </summary>
        public bool ValidateSqlMapConfig
        {
            set { _validateSqlMapConfig = value; }
        }
		#endregion

		#region Constructor

		/// <summary>
        /// Constructs a DomSqlMapBuilder.
		/// </summary>
		public DomSqlMapBuilder()
		{
            _configScope = new ConfigurationScope();
            _paramParser = new InlineParameterMapParser();
            _deSerializerFactory = new DeSerializerFactory(_configScope);
		}
	
		#endregion 

		#region Configure

		/// <summary>
		/// Configure a SqlMapper from default resource file named 'SqlMap.config'.
		/// </summary>
		/// <returns>An ISqlMapper instance.</returns>
		/// <remarks>
		/// The file path is relative to the application root. For ASP.Net applications
		/// this would be the same directory as the Web.config file. For other .Net
		/// applications the SqlMap.config file should be placed in the same folder
		/// as the executable. 
		/// </remarks>
        public ISqlMapper Configure()
		{
			return Configure( Resources.GetConfigAsXmlDocument(DEFAULT_FILE_CONFIG_NAME) );
		}

		/// <summary>
		/// Configure and returns an ISqlMapper instance.
		/// </summary>
		/// <param name="document">An xml sql map configuration document.</param>
		/// <returns>An ISqlMapper instance.</returns>
        public ISqlMapper Configure(XmlDocument document)
		{
			return Build( document, false );
		}


		/// <summary>
		/// Configure an ISqlMapper object from a file path.
		/// </summary>
		/// <param name="resource">
		/// A relative ressource path from your Application root 
		/// or a absolue file path file:\\c:\dir\a.config
		/// </param>
		/// <returns>An ISqlMapper instance.</returns>
        public ISqlMapper Configure(string resource)
		{
			XmlDocument document;
			if (resource.StartsWith("file://"))
			{
				document = Resources.GetUrlAsXmlDocument( resource.Remove(0, 7) );	
			}
			else
			{
				document = Resources.GetResourceAsXmlDocument( resource );	
			}
			return Build( document, false);
		}

		/// <summary>
		///  Configure an ISqlMapper object from a stream.
		/// </summary>
		/// <param name="resource">A Stream resource.</param>
		/// <returns>An SqlMap</returns>
		public ISqlMapper Configure(Stream resource)
		{
			XmlDocument document = Resources.GetStreamAsXmlDocument( resource );
			return Build( document, false);
		}

		/// <summary>
		///  Configure an ISqlMapper object from a FileInfo.
		/// </summary>
		/// <param name="resource">A FileInfo resource.</param>
		/// <returns>An ISqlMapper instance.</returns>
		public ISqlMapper Configure(FileInfo resource)
		{
			XmlDocument document = Resources.GetFileInfoAsXmlDocument( resource );
			return Build( document, false);
		}

		/// <summary>
		///  Configure an ISqlMapper object from an Uri.
		/// </summary>
		/// <param name="resource">A Uri resource.</param>
		/// <returns>An ISqlMapper instance.</returns>
		public ISqlMapper Configure(Uri resource)
		{
			XmlDocument document = Resources.GetUriAsXmlDocument( resource );
			return Build( document, false);
		}

		/// <summary>
		/// Configure and monitor the default configuration file (SqlMap.config) for modifications 
		/// and automatically reconfigure SqlMap. 
		/// </summary>
		/// <returns>An ISqlMapper instance.</returns>
		public ISqlMapper ConfigureAndWatch(ConfigureHandler configureDelegate)
		{
			return ConfigureAndWatch( DEFAULT_FILE_CONFIG_NAME, configureDelegate ) ;
		}

		/// <summary>
		/// Configure and monitor the configuration file for modifications 
		/// and automatically reconfigure the ISqlMapper instance.
		/// </summary>
		/// <param name="resource">
		/// A relative ressource path from your Application root 
		/// or an absolue file path file:\\c:\dir\a.config
		/// </param>
		///<param name="configureDelegate">
		/// Delegate called when the file has changed.
		/// </param>
		/// <returns>An ISqlMapper instance.</returns>
		public ISqlMapper ConfigureAndWatch( string resource, ConfigureHandler configureDelegate )
		{
			XmlDocument document = null;
			if (resource.StartsWith("file://"))
			{
				document = Resources.GetUrlAsXmlDocument( resource.Remove(0, 7) );	
			}
			else
			{
				document = Resources.GetResourceAsXmlDocument( resource );	
			}

			ConfigWatcherHandler.ClearFilesMonitored();
			ConfigWatcherHandler.AddFileToWatch( Resources.GetFileInfo( resource ) );

			TimerCallback callBakDelegate = new TimerCallback( OnConfigFileChange );

			StateConfig state = new StateConfig();
			state.FileName = resource;
			state.ConfigureHandler = configureDelegate;
            
			ISqlMapper sqlMapper = Build( document, true );
		    
			new ConfigWatcherHandler( callBakDelegate, state );

            return sqlMapper;
		}

		/// <summary>
		/// Configure and monitor the configuration file for modifications 
		/// and automatically reconfigure the ISqlMapper instance.
		/// </summary>
		/// <param name="resource">
		/// A FileInfo to a SqlMap.config file.
		/// </param>
		///<param name="configureDelegate">
		/// Delegate called when the file has changed.
		/// </param>
		/// <returns>An ISqlMapper instance.</returns>
		public ISqlMapper ConfigureAndWatch( FileInfo resource, ConfigureHandler configureDelegate )
		{
			XmlDocument document = Resources.GetFileInfoAsXmlDocument(resource);

			ConfigWatcherHandler.ClearFilesMonitored();
			ConfigWatcherHandler.AddFileToWatch( resource );

			TimerCallback callBakDelegate = new TimerCallback( OnConfigFileChange );

			StateConfig state = new StateConfig();
			state.FileName = resource.FullName;
			state.ConfigureHandler = configureDelegate;

			ISqlMapper sqlMapper = Build( document, true );
    
		    new ConfigWatcherHandler(callBakDelegate, state);

            return sqlMapper;
		}

		/// <summary>
		/// Callback called when the SqlMap.config file has changed.
		/// </summary>
		/// <param name="obj">The <see cref="StateConfig"/> object.</param>
		public static void OnConfigFileChange(object obj)
		{
			StateConfig state = (StateConfig)obj;
			state.ConfigureHandler( null );
		}

		#endregion 

		#region Methods

		/// <summary>
		/// Build an ISqlMapper instance.
		/// </summary>
		/// <param name="document">An xml configuration document.</param>
		/// <param name="dataSource">A data source.</param>
		/// <param name="useConfigFileWatcher"></param>
		/// <param name="isCallFromDao"></param>
		/// <returns>Returns an ISqlMapper instance.</returns>
		private ISqlMapper Build(XmlDocument document,DataSource dataSource, 
			bool useConfigFileWatcher, bool isCallFromDao)
		{
			_configScope.SqlMapConfigDocument = document;
			_configScope.DataSource = dataSource;
			_configScope.IsCallFromDao = isCallFromDao;
			_configScope.UseConfigFileWatcher = useConfigFileWatcher;
			
			_configScope.XmlNamespaceManager = new XmlNamespaceManager(_configScope.SqlMapConfigDocument.NameTable);
			_configScope.XmlNamespaceManager.AddNamespace(DATAMAPPER_NAMESPACE_PREFIX, DATAMAPPER_XML_NAMESPACE);
			_configScope.XmlNamespaceManager.AddNamespace(PROVIDERS_NAMESPACE_PREFIX, PROVIDER_XML_NAMESPACE);
			_configScope.XmlNamespaceManager.AddNamespace(MAPPING_NAMESPACE_PREFIX, MAPPING_XML_NAMESPACE);

			try
			{
                if (_validateSqlMapConfig) 
				{
					ValidateSchema( document.ChildNodes[1], "SqlMapConfig.xsd" );
				}
				Initialize();
				return _configScope.SqlMapper;
			}
			catch(Exception e)
			{	
				throw new ConfigurationException(_configScope.ErrorContext.ToString(), e);
			}
		}

		/// <summary>
		/// Validates an XmlNode against a schema file.
		/// </summary>
		/// <param name="section">The doc to validate.</param>
		/// <param name="schemaFileName">Schema file name.</param>
		private void ValidateSchema( XmlNode section, string schemaFileName )
		{
#if dotnet2
            XmlReader validatingReader = null;
#else
            XmlValidatingReader validatingReader = null;
#endif
            Stream xsdFile = null; 

			_configScope.ErrorContext.Activity = "Validate SqlMap config";
			try
			{               
				//Validate the document using a schema               
				xsdFile = GetStream( schemaFileName ); 

				if (xsdFile == null)
				{
					// TODO: avoid using hard-coded value "IBatisNet.DataMapper"
					throw new ConfigurationException( "Unable to locate embedded resource [IBatisNet.DataMapper."+schemaFileName+"]. If you are building from source, verfiy the file is marked as an embedded resource.");
				}
				
				XmlSchema schema = XmlSchema.Read( xsdFile, new ValidationEventHandler(ValidationCallBack) );

#if dotnet2
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;

                // Create the XmlSchemaSet class.
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add(schema);

                settings.Schemas = schemas;
                validatingReader = XmlReader.Create( new XmlNodeReader(section) ,  settings);

				// Wire up the call back.  The ValidationEvent is fired when the
				// XmlValidatingReader hits an issue validating a section of the xml
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
#else
                validatingReader = new XmlValidatingReader(new XmlTextReader(new StringReader(section.OuterXml)));
                validatingReader.ValidationType = ValidationType.Schema;

                validatingReader.Schemas.Add(schema);

                // Wire up the call back.  The ValidationEvent is fired when the
                // XmlValidatingReader hits an issue validating a section of the xml
                validatingReader.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
#endif
                // Validate the document
				while (validatingReader.Read()){}

				if(! _configScope.IsXmlValid )
				{
					throw new ConfigurationException( "Invalid SqlMap.config document. cause :"+_configScope.ErrorContext.Resource);
				}
			}
			finally
			{
				if( validatingReader != null ) validatingReader.Close();
				if( xsdFile != null ) xsdFile.Close();
			}
		}

		private void ValidationCallBack( object sender, ValidationEventArgs args )
		{
			_configScope.IsXmlValid = false;
			_configScope.ErrorContext.Resource += args.Message + Environment.NewLine;
		}

		/// <summary>
		/// Load statements (select, insert, update, delete), parameters, and resultMaps.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="dataSource"></param>
		/// <param name="useConfigFileWatcher"></param>
		/// <param name="properties"></param>
		/// <returns></returns>
		/// <remarks>Used by Dao</remarks>
		public ISqlMapper Build(XmlDocument document, DataSource dataSource, bool useConfigFileWatcher, NameValueCollection properties)
		{
			_configScope.Properties.Add(properties);
			return Build(document, dataSource, useConfigFileWatcher, true);
		}

		/// <summary>
		/// Load SqlMap configuration from
		/// from the XmlDocument passed in parameter.
		/// </summary>
		/// <param name="document">The xml sql map configuration.</param>
		/// <param name="useConfigFileWatcher"></param>
		public ISqlMapper Build(XmlDocument document, bool useConfigFileWatcher)
		{
			return Build(document, null, useConfigFileWatcher, false);
		}

		/// <summary>
		/// Reset PreparedStatements cache
		/// </summary>
		private void Reset()
		{
		}

		/// <summary>
		/// Intialize the internal ISqlMapper instance.
		/// </summary>
		private void Initialize()
		{
			Reset();

			#region Load Global Properties
			if (_configScope.IsCallFromDao == false)
			{
				_configScope.NodeContext = _configScope.SqlMapConfigDocument.SelectSingleNode( ApplyDataMapperNamespacePrefix(XML_DATAMAPPER_CONFIG_ROOT), _configScope.XmlNamespaceManager);

				ParseGlobalProperties();
			}
			#endregion

			#region Load settings

			_configScope.ErrorContext.Activity = "loading global settings";

			XmlNodeList settings = _configScope.SqlMapConfigDocument.SelectNodes( ApplyDataMapperNamespacePrefix(XML_CONFIG_SETTINGS), _configScope.XmlNamespaceManager);

			if (settings!=null)
			{
				foreach (XmlNode setting in settings)
				{
					if (setting.Attributes[ATR_USE_STATEMENT_NAMESPACES] != null )
					{	
						string value = NodeUtils.ParsePropertyTokens(setting.Attributes[ATR_USE_STATEMENT_NAMESPACES].Value, _configScope.Properties);
						_configScope.UseStatementNamespaces =  Convert.ToBoolean( value ); 
					}
					if (setting.Attributes[ATR_CACHE_MODELS_ENABLED] != null )
					{		
						string value = NodeUtils.ParsePropertyTokens(setting.Attributes[ATR_CACHE_MODELS_ENABLED].Value, _configScope.Properties);
						_configScope.IsCacheModelsEnabled =  Convert.ToBoolean( value ); 
					}
					if (setting.Attributes[ATR_USE_REFLECTION_OPTIMIZER] != null )
					{		
						string value = NodeUtils.ParsePropertyTokens(setting.Attributes[ATR_USE_REFLECTION_OPTIMIZER].Value, _configScope.Properties);
						_configScope.UseReflectionOptimizer =  Convert.ToBoolean( value ); 
					}
					if (setting.Attributes[ATR_VALIDATE_SQLMAP] != null )
					{		
						string value = NodeUtils.ParsePropertyTokens(setting.Attributes[ATR_VALIDATE_SQLMAP].Value, _configScope.Properties);
						_configScope.ValidateSqlMap =  Convert.ToBoolean( value ); 
					}
				}
			}

			#endregion            
			
            if (_objectFactory == null)
            {
                _objectFactory = new ObjectFactory(_configScope.UseReflectionOptimizer);
            }
            if (_setAccessorFactory == null)
            {
                _setAccessorFactory = new SetAccessorFactory(_configScope.UseReflectionOptimizer);
            }
            if (_getAccessorFactory == null)
            {
                _getAccessorFactory = new GetAccessorFactory(_configScope.UseReflectionOptimizer);
            }
		    if (_sqlMapper == null)
		    {
                AccessorFactory accessorFactory = new AccessorFactory(_setAccessorFactory, _getAccessorFactory);
                _configScope.SqlMapper = new SqlMapper(_objectFactory, accessorFactory);
		    }
		    else
		    {
                _configScope.SqlMapper = _sqlMapper;
		    }

            ParameterMap emptyParameterMap = new ParameterMap(_configScope.DataExchangeFactory);
            emptyParameterMap.Id = ConfigurationScope.EMPTY_PARAMETER_MAP;
            _configScope.SqlMapper.AddParameterMap( emptyParameterMap );

            _configScope.SqlMapper.IsCacheModelsEnabled = _configScope.IsCacheModelsEnabled;

			#region Cache Alias

			TypeAlias cacheAlias = new TypeAlias(typeof(MemoryCacheControler));
			cacheAlias.Name = "MEMORY";
			_configScope.SqlMapper.TypeHandlerFactory.AddTypeAlias(cacheAlias.Name, cacheAlias);
			cacheAlias = new TypeAlias(typeof(LruCacheController));
			cacheAlias.Name = "LRU";
			_configScope.SqlMapper.TypeHandlerFactory.AddTypeAlias(cacheAlias.Name, cacheAlias);
			cacheAlias = new TypeAlias(typeof(FifoCacheController));
			cacheAlias.Name = "FIFO";
			_configScope.SqlMapper.TypeHandlerFactory.AddTypeAlias(cacheAlias.Name, cacheAlias);
            cacheAlias = new TypeAlias(typeof(AnsiStringTypeHandler));
            cacheAlias.Name = "AnsiStringTypeHandler";
            _configScope.SqlMapper.TypeHandlerFactory.AddTypeAlias(cacheAlias.Name, cacheAlias);

			#endregion 

			#region Load providers
			if (_configScope.IsCallFromDao == false)
			{
				GetProviders();
			}
			#endregion

			#region Load DataBase
			#region Choose the  provider
			IDbProvider provider = null;
			if ( _configScope.IsCallFromDao==false )
			{
				provider = ParseProvider();
				_configScope.ErrorContext.Reset();
			}
			#endregion

			#region Load the DataSources

			_configScope.ErrorContext.Activity = "loading Database DataSource";
			XmlNode nodeDataSource = _configScope.SqlMapConfigDocument.SelectSingleNode( ApplyDataMapperNamespacePrefix(XML_DATABASE_DATASOURCE), _configScope.XmlNamespaceManager );
			
			if (nodeDataSource == null)
			{
				if (_configScope.IsCallFromDao == false)
				{
					throw new ConfigurationException("There's no dataSource tag in SqlMap.config.");
				}
				else  // patch from Luke Yang
				{
					_configScope.SqlMapper.DataSource = _configScope.DataSource;
				}
			}
			else
			{
				if (_configScope.IsCallFromDao == false)
				{
					_configScope.ErrorContext.Resource = nodeDataSource.OuterXml.ToString();
					_configScope.ErrorContext.MoreInfo = "parse DataSource";

					DataSource dataSource = DataSourceDeSerializer.Deserialize( nodeDataSource );

					dataSource.DbProvider = provider;
					dataSource.ConnectionString = NodeUtils.ParsePropertyTokens(dataSource.ConnectionString, _configScope.Properties);

					_configScope.DataSource = dataSource;
					_configScope.SqlMapper.DataSource = _configScope.DataSource;
				}
				else
				{
					_configScope.SqlMapper.DataSource = _configScope.DataSource;
				}
				_configScope.ErrorContext.Reset();
			}
			#endregion
			#endregion

			#region Load Global TypeAlias
			foreach (XmlNode xmlNode in _configScope.SqlMapConfigDocument.SelectNodes( ApplyDataMapperNamespacePrefix(XML_GLOBAL_TYPEALIAS), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.Activity = "loading global Type alias";
				TypeAliasDeSerializer.Deserialize(xmlNode, _configScope);
			}
			_configScope.ErrorContext.Reset();
			#endregion

			#region Load TypeHandlers
			foreach (XmlNode xmlNode in _configScope.SqlMapConfigDocument.SelectNodes( ApplyDataMapperNamespacePrefix(XML_GLOBAL_TYPEHANDLER), _configScope.XmlNamespaceManager))
			{
				try
				{
					_configScope.ErrorContext.Activity = "loading typeHandler";
					TypeHandlerDeSerializer.Deserialize( xmlNode, _configScope );
				} 
				catch (Exception e) 
				{
					NameValueCollection prop = NodeUtils.ParseAttributes(xmlNode, _configScope.Properties);

					throw new ConfigurationException(
						String.Format("Error registering TypeHandler class \"{0}\" for handling .Net type \"{1}\" and dbType \"{2}\". Cause: {3}", 
						NodeUtils.GetStringAttribute(prop, "callback"),
						NodeUtils.GetStringAttribute(prop, "type"),
						NodeUtils.GetStringAttribute(prop, "dbType"),
						e.Message), e);
				}
			}
			_configScope.ErrorContext.Reset();
			#endregion

			#region Load sqlMap mapping files
			
			foreach (XmlNode xmlNode in _configScope.SqlMapConfigDocument.SelectNodes( ApplyDataMapperNamespacePrefix(XML_SQLMAP), _configScope.XmlNamespaceManager))
			{
				_configScope.NodeContext = xmlNode;
				ConfigureSqlMap();
			}

			#endregion

			#region Attach CacheModel to statement

			if (_configScope.IsCacheModelsEnabled)
			{
				foreach(DictionaryEntry entry in _configScope.SqlMapper.MappedStatements)
				{
					_configScope.ErrorContext.Activity = "Set CacheModel to statement";

					IMappedStatement mappedStatement = (IMappedStatement)entry.Value;
					if (mappedStatement.Statement.CacheModelName.Length >0)
					{
						_configScope.ErrorContext.MoreInfo = "statement : "+mappedStatement.Statement.Id;
						_configScope.ErrorContext.Resource = "cacheModel : " +mappedStatement.Statement.CacheModelName;
						mappedStatement.Statement.CacheModel = _configScope.SqlMapper.GetCache(mappedStatement.Statement.CacheModelName);
					}
				}
			}
			_configScope.ErrorContext.Reset();
			#endregion 

			#region Register Trigger Statements for Cache Models
			foreach (DictionaryEntry entry in _configScope.CacheModelFlushOnExecuteStatements)
			{
				string cacheModelId = (string)entry.Key;
				IList statementsToRegister = (IList)entry.Value;

				if (statementsToRegister != null && statementsToRegister.Count > 0)
				{
					foreach (string statementName in statementsToRegister)
					{
						IMappedStatement mappedStatement = _configScope.SqlMapper.MappedStatements[statementName] as IMappedStatement;

						if (mappedStatement != null)
						{
							CacheModel cacheModel = _configScope.SqlMapper.GetCache(cacheModelId);

							if (_logger.IsDebugEnabled)
							{
								_logger.Debug("Registering trigger statement [" + mappedStatement.Id + "] to cache model [" + cacheModel.Id + "]");
							}

							cacheModel.RegisterTriggerStatement(mappedStatement);
						}
						else
						{
							if (_logger.IsWarnEnabled)
							{
								_logger.Warn("Unable to register trigger statement [" + statementName + "] to cache model [" + cacheModelId + "]. Statement does not exist.");
							}
						}
					}
				}
			}
			#endregion

			#region Resolve resultMap / Discriminator / PropertyStategy attributes on Result/Argument Property 

			foreach(DictionaryEntry entry in _configScope.SqlMapper.ResultMaps)
			{
				_configScope.ErrorContext.Activity = "Resolve 'resultMap' attribute on Result Property";

				ResultMap resultMap = (ResultMap)entry.Value;
				for(int index=0; index< resultMap.Properties.Count; index++)
				{
					ResultProperty result = resultMap.Properties[index];
					if(result.NestedResultMapName.Length >0)
					{
						result.NestedResultMap = _configScope.SqlMapper.GetResultMap(result.NestedResultMapName);
					}
					result.PropertyStrategy = PropertyStrategyFactory.Get(result);
				}
				for(int index=0; index< resultMap.Parameters.Count; index++)
				{
					ResultProperty result = resultMap.Parameters[index];
					if(result.NestedResultMapName.Length >0)
					{
						result.NestedResultMap = _configScope.SqlMapper.GetResultMap(result.NestedResultMapName);
					}
					result.ArgumentStrategy = ArgumentStrategyFactory.Get( (ArgumentProperty)result );
				}
				if (resultMap.Discriminator != null)
				{
					resultMap.Discriminator.Initialize(_configScope);
				}
			}

			_configScope.ErrorContext.Reset();

			#endregion

		}

		/// <summary>
		/// Load and initialize providers from specified file.
		/// </summary>
		private void GetProviders()
		{
			IDbProvider provider;
			XmlDocument xmlProviders;

			_configScope.ErrorContext.Activity = "loading Providers";

			XmlNode providersNode;
			providersNode = _configScope.SqlMapConfigDocument.SelectSingleNode( ApplyDataMapperNamespacePrefix(XML_CONFIG_PROVIDERS), _configScope.XmlNamespaceManager);

			if (providersNode != null )
			{
				xmlProviders = Resources.GetAsXmlDocument( providersNode, _configScope.Properties );
			}
			else
			{
				xmlProviders = Resources.GetConfigAsXmlDocument(PROVIDERS_FILE_NAME);
			}

			foreach (XmlNode node in xmlProviders.SelectNodes( ApplyProviderNamespacePrefix(XML_PROVIDER), _configScope.XmlNamespaceManager ) )
			{
				_configScope.ErrorContext.Resource = node.InnerXml.ToString();
				provider = ProviderDeSerializer.Deserialize(node);

				if (provider.IsEnabled)
				{
					_configScope.ErrorContext.ObjectId = provider.Name;
					_configScope.ErrorContext.MoreInfo = "initialize provider";

					provider.Initialize();
					_configScope.Providers.Add(provider.Name, provider);

					if (provider.IsDefault)
					{
						if (_configScope.Providers[DEFAULT_PROVIDER_NAME] == null) 
						{
							_configScope.Providers.Add(DEFAULT_PROVIDER_NAME,provider);
						} 
						else 
						{
							throw new ConfigurationException(
								string.Format("Error while configuring the Provider named \"{0}\" There can be only one default Provider.",provider.Name));
						}
					}
				}
			}
			_configScope.ErrorContext.Reset();
		}


		/// <summary>
		/// Parse the provider tag.
		/// </summary>
		/// <returns>A provider object.</returns>
		private IDbProvider ParseProvider()
		{
			_configScope.ErrorContext.Activity = "load DataBase Provider";
			XmlNode node = _configScope.SqlMapConfigDocument.SelectSingleNode( ApplyDataMapperNamespacePrefix(XML_DATABASE_PROVIDER), _configScope.XmlNamespaceManager  );

			if (node != null)
			{
				_configScope.ErrorContext.Resource = node.OuterXml.ToString();
				string providerName = NodeUtils.ParsePropertyTokens(node.Attributes["name"].Value, _configScope.Properties);

				_configScope.ErrorContext.ObjectId = providerName;

				if (_configScope.Providers.Contains(providerName))
				{
					return (IDbProvider) _configScope.Providers[providerName];
				}
				else
				{
					throw new ConfigurationException(
						string.Format("Error while configuring the Provider named \"{0}\". Cause : The provider is not in 'providers.config' or is not enabled.",
							providerName));
				}
			}
			else
			{
				if (_configScope.Providers.Contains(DEFAULT_PROVIDER_NAME))
				{
					return (IDbProvider) _configScope.Providers[DEFAULT_PROVIDER_NAME];
				}
				else
				{
					throw new ConfigurationException(
						string.Format("Error while configuring the SqlMap. There is no provider marked default in 'providers.config' file."));
				}
			}
		}


		/// <summary>
		/// Load sqlMap statement.
		/// </summary>
		private void ConfigureSqlMap( )
		{
			XmlNode sqlMapNode = _configScope.NodeContext;

			_configScope.ErrorContext.Activity = "loading SqlMap";
			_configScope.ErrorContext.Resource = sqlMapNode.OuterXml.ToString();

			if (_configScope.UseConfigFileWatcher)
			{
				if (sqlMapNode.Attributes["resource"] != null || sqlMapNode.Attributes["url"] != null) 
				{ 
					ConfigWatcherHandler.AddFileToWatch( Resources.GetFileInfo( Resources.GetValueOfNodeResourceUrl(sqlMapNode, _configScope.Properties) ) );
				}
			}

			// Load the file 
			_configScope.SqlMapDocument = Resources.GetAsXmlDocument(sqlMapNode, _configScope.Properties);
			
			if (_configScope.ValidateSqlMap)
			{
				ValidateSchema( _configScope.SqlMapDocument.ChildNodes[1], "SqlMap.xsd" );
			}

			_configScope.SqlMapNamespace = _configScope.SqlMapDocument.SelectSingleNode( ApplyMappingNamespacePrefix(XML_MAPPING_ROOT), _configScope.XmlNamespaceManager ).Attributes["namespace"].Value;

			#region Load TypeAlias

			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_TYPEALIAS), _configScope.XmlNamespaceManager))
			{
				TypeAliasDeSerializer.Deserialize(xmlNode, _configScope);
			}
			_configScope.ErrorContext.MoreInfo = string.Empty;
			_configScope.ErrorContext.ObjectId = string.Empty;

			#endregion

			#region Load resultMap

			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_RESULTMAP), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading ResultMap tag";
				_configScope.NodeContext = xmlNode; // A ResultMap node

				BuildResultMap();
			}

			#endregion

			#region Load parameterMaps

			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_PARAMETERMAP), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading ParameterMap tag";
				_configScope.NodeContext = xmlNode; // A ParameterMap node

				BuildParameterMap();
			}

			#endregion
		
			#region Load statements

            #region Sql tag
            foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes(ApplyMappingNamespacePrefix(SQL_STATEMENT), _configScope.XmlNamespaceManager))
            {
                _configScope.ErrorContext.MoreInfo = "loading sql tag";
                _configScope.NodeContext = xmlNode; // A sql tag

                SqlDeSerializer.Deserialize(xmlNode, _configScope);
            }
            #endregion
		    
			#region Statement tag
			Statement statement;
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_STATEMENT), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading statement tag";
				_configScope.NodeContext = xmlNode; // A statement tag

				statement = StatementDeSerializer.Deserialize(xmlNode, _configScope);
                statement.CacheModelName = _configScope.ApplyNamespace(statement.CacheModelName);
                statement.ParameterMapName = _configScope.ApplyNamespace(statement.ParameterMapName);
                //statement.ResultMapName = ApplyNamespace( statement.ResultMapName );

				if (_configScope.UseStatementNamespaces)
				{
                    statement.Id = _configScope.ApplyNamespace(statement.Id);
				}
				_configScope.ErrorContext.ObjectId = statement.Id;
				statement.Initialize( _configScope );

				// Build ISql (analyse sql statement)		
				ProcessSqlStatement( statement  );

				// Build MappedStatement
                MappedStatement mappedStatement = new MappedStatement(_configScope.SqlMapper, statement);
                IMappedStatement mapStatement = mappedStatement;
                if (statement.CacheModelName != null && statement.CacheModelName.Length > 0 && _configScope.IsCacheModelsEnabled)
                {
                    mapStatement = new CachingStatement(mappedStatement);
                }

                _configScope.SqlMapper.AddMappedStatement(mapStatement.Id, mapStatement);
			}
			#endregion

			#region Select tag
			Select select;
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_SELECT), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading select tag";
				_configScope.NodeContext = xmlNode; // A select node

				select = SelectDeSerializer.Deserialize(xmlNode, _configScope);
                select.CacheModelName = _configScope.ApplyNamespace(select.CacheModelName);
                select.ParameterMapName = _configScope.ApplyNamespace(select.ParameterMapName);
                //select.ResultMapName = ApplyNamespace( select.ResultMapName );

				if (_configScope.UseStatementNamespaces)
				{
                    select.Id = _configScope.ApplyNamespace(select.Id);
				}
				_configScope.ErrorContext.ObjectId = select.Id;
				
				select.Initialize( _configScope );

				if (select.Generate != null)
				{
					GenerateCommandText(_configScope, select);
				}
				else
				{
					// Build ISql (analyse sql statement)		
					ProcessSqlStatement( select);
				}

				// Build MappedStatement
                MappedStatement mappedStatement = new SelectMappedStatement(_configScope.SqlMapper, select);
                IMappedStatement mapStatement = mappedStatement;
				if (select.CacheModelName != null && select.CacheModelName.Length> 0 && _configScope.IsCacheModelsEnabled)
				{
                    mapStatement = new CachingStatement(mappedStatement);
				}

                _configScope.SqlMapper.AddMappedStatement(mapStatement.Id, mapStatement);
			}
			#endregion

			#region Insert tag
			Insert insert;
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_INSERT), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading insert tag";
				_configScope.NodeContext = xmlNode; // A insert tag

				MappedStatement mappedStatement;

				insert = InsertDeSerializer.Deserialize(xmlNode, _configScope);
                insert.CacheModelName = _configScope.ApplyNamespace(insert.CacheModelName);
                insert.ParameterMapName = _configScope.ApplyNamespace(insert.ParameterMapName);
                //insert.ResultMapName = ApplyNamespace( insert.ResultMapName );

				if (_configScope.UseStatementNamespaces)
				{
                    insert.Id = _configScope.ApplyNamespace(insert.Id);
				}
				_configScope.ErrorContext.ObjectId = insert.Id;
				insert.Initialize( _configScope );

				// Build ISql (analyse sql command text)
				if (insert.Generate != null)
				{
					GenerateCommandText(_configScope, insert);
				}
				else
				{
					ProcessSqlStatement( insert);
				}

				// Build MappedStatement
				mappedStatement = new InsertMappedStatement( _configScope.SqlMapper, insert);

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Id, mappedStatement);

				#region statement SelectKey
				// Set sql statement SelectKey 
				if (insert.SelectKey != null)
				{
					_configScope.ErrorContext.MoreInfo = "loading selectKey tag";
					_configScope.NodeContext = xmlNode.SelectSingleNode( ApplyMappingNamespacePrefix(XML_SELECTKEY), _configScope.XmlNamespaceManager);

					insert.SelectKey.Id = insert.Id;
					insert.SelectKey.Initialize( _configScope );
					insert.SelectKey.Id += DOT + "SelectKey";

					// Initialize can also use _configScope.ErrorContext.ObjectId to get the id
					// of the parent <select> node
					// insert.SelectKey.Initialize( _configScope );
					// insert.SelectKey.Id = insert.Id + DOT + "SelectKey";

					ProcessSqlStatement(insert.SelectKey);
					
					// Build MappedStatement
					mappedStatement = new MappedStatement( _configScope.SqlMapper, insert.SelectKey);

					_configScope.SqlMapper.AddMappedStatement(mappedStatement.Id, mappedStatement);
				}
				#endregion
			}
			#endregion

			#region Update tag
			Update update;
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_UPDATE), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading update tag";
				_configScope.NodeContext = xmlNode; // A update tag

				MappedStatement mappedStatement;

				update = UpdateDeSerializer.Deserialize(xmlNode, _configScope);
                update.CacheModelName = _configScope.ApplyNamespace(update.CacheModelName);
                update.ParameterMapName = _configScope.ApplyNamespace(update.ParameterMapName);
                //update.ResultMapName = ApplyNamespace( update.ResultMapName );

				if (_configScope.UseStatementNamespaces)
				{
                    update.Id = _configScope.ApplyNamespace(update.Id);
				}
				_configScope.ErrorContext.ObjectId = update.Id;
				update.Initialize( _configScope );

				// Build ISql (analyse sql statement)	
				if (update.Generate != null)
				{
					GenerateCommandText(_configScope, update);
				}
				else
				{
					// Build ISql (analyse sql statement)		
					ProcessSqlStatement(update);
				}	

				// Build MappedStatement
				mappedStatement = new UpdateMappedStatement( _configScope.SqlMapper, update);

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Id, mappedStatement);
			}
			#endregion

			#region Delete tag
			Delete delete;
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_DELETE), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading delete tag";
				_configScope.NodeContext = xmlNode; // A delete tag
				MappedStatement mappedStatement;

				delete = DeleteDeSerializer.Deserialize(xmlNode, _configScope);
                delete.CacheModelName = _configScope.ApplyNamespace(delete.CacheModelName);
                delete.ParameterMapName = _configScope.ApplyNamespace(delete.ParameterMapName);
                //delete.ResultMapName = ApplyNamespace( delete.ResultMapName );

				if (_configScope.UseStatementNamespaces)
				{
                    delete.Id = _configScope.ApplyNamespace(delete.Id);
				}
				_configScope.ErrorContext.ObjectId = delete.Id;
				delete.Initialize( _configScope );

				// Build ISql (analyse sql statement)
				if (delete.Generate != null)
				{
					GenerateCommandText(_configScope, delete);
				}
				else
				{
					// Build ISql (analyse sql statement)		
					ProcessSqlStatement(delete);
				}	

				// Build MappedStatement
				mappedStatement = new DeleteMappedStatement( _configScope.SqlMapper, delete);

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Id, mappedStatement);
			}
			#endregion

			#region Procedure tag
			Procedure procedure;
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_PROCEDURE), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading procedure tag";
				_configScope.NodeContext = xmlNode; // A procedure tag

				procedure = ProcedureDeSerializer.Deserialize(xmlNode, _configScope);
                procedure.CacheModelName = _configScope.ApplyNamespace(procedure.CacheModelName);
                procedure.ParameterMapName = _configScope.ApplyNamespace(procedure.ParameterMapName);
                //procedure.ResultMapName = ApplyNamespace( procedure.ResultMapName );

				if (_configScope.UseStatementNamespaces)
				{
                    procedure.Id = _configScope.ApplyNamespace(procedure.Id);
				}
				_configScope.ErrorContext.ObjectId = procedure.Id;
				procedure.Initialize( _configScope );

				// Build ISql (analyse sql command text)
				ProcessSqlStatement( procedure );

				// Build MappedStatement
                MappedStatement mappedStatement = new MappedStatement(_configScope.SqlMapper, procedure);
                IMappedStatement mapStatement = mappedStatement;		    
                if (procedure.CacheModelName != null && procedure.CacheModelName.Length > 0 && _configScope.IsCacheModelsEnabled)
                {
                    mapStatement = new CachingStatement(mappedStatement);
                }

                _configScope.SqlMapper.AddMappedStatement(mapStatement.Id, mapStatement);
			}
			#endregion

			#endregion

			#region Load CacheModels

			if (_configScope.IsCacheModelsEnabled)
			{
				CacheModel cacheModel;
				foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_CACHE_MODEL), _configScope.XmlNamespaceManager))
				{
					cacheModel = CacheModelDeSerializer.Deserialize(xmlNode, _configScope);
                    cacheModel.Id = _configScope.ApplyNamespace(cacheModel.Id);

					// Attach ExecuteEventHandler
					foreach(XmlNode flushOn in xmlNode.SelectNodes( ApplyMappingNamespacePrefix(XML_FLUSH_ON_EXECUTE), _configScope.XmlNamespaceManager  ))
					{
						string statementName = flushOn.Attributes["statement"].Value;
						if (_configScope.UseStatementNamespaces)
						{
                            statementName = _configScope.ApplyNamespace(statementName); 
						}

						// delay registering statements to cache model until all sqlMap files have been processed
						IList statementNames = (IList)_configScope.CacheModelFlushOnExecuteStatements[cacheModel.Id];
						if (statementNames == null)
						{
							statementNames = new ArrayList();
						}
						statementNames.Add(statementName);
						_configScope.CacheModelFlushOnExecuteStatements[cacheModel.Id] = statementNames;
					}

					// Get Properties
					foreach(XmlNode propertie in xmlNode.SelectNodes( ApplyMappingNamespacePrefix(XML_PROPERTY), _configScope.XmlNamespaceManager))
					{
						string name = propertie.Attributes["name"].Value;
						string value = propertie.Attributes["value"].Value;
					
						cacheModel.AddProperty(name, value);
					}

					cacheModel.Initialize();

					_configScope.SqlMapper.AddCache( cacheModel );
				}
			}

			#endregion

			_configScope.ErrorContext.Reset();
		}


		/// <summary>
		/// Process the Sql Statement
		/// </summary>
		/// <param name="statement"></param>
		private void ProcessSqlStatement( IStatement statement )
		{
			bool isDynamic = false;
			XmlNode commandTextNode = _configScope.NodeContext;
			DynamicSql dynamic = new DynamicSql( _configScope,  statement );
			StringBuilder sqlBuffer = new StringBuilder();

			if (statement.Id=="DynamicJIRA")
			{
				Console.Write("tt");
			}
			
			_configScope.ErrorContext.MoreInfo = "process the Sql statement";

			// Resolve "extend" attribute on Statement
			if (statement.ExtendStatement.Length >0)
			{
				// Find 'super' statement
				XmlNode supperStatementNode = _configScope.SqlMapDocument.SelectSingleNode( ApplyMappingNamespacePrefix(XML_SEARCH_STATEMENT)+"/child::*[@id='"+statement.ExtendStatement+"']",_configScope.XmlNamespaceManager );
				if (supperStatementNode!=null)
				{
					commandTextNode.InnerXml = supperStatementNode.InnerXml + commandTextNode.InnerXml;
				}
				else
				{
					throw new ConfigurationException("Unable to find extend statement named '"+statement.ExtendStatement+"' on statement '"+statement.Id+"'.'");
				}
			}

			_configScope.ErrorContext.MoreInfo = "parse dynamic tags on sql statement";

			isDynamic = ParseDynamicTags( commandTextNode, dynamic, sqlBuffer, isDynamic, false, statement);

			if (isDynamic) 
			{
				statement.Sql = dynamic;
			} 
			else 
			{	
				string sqlText = sqlBuffer.ToString();
				ApplyInlineParemeterMap( statement, sqlText);
			}
		}

				
		/// <summary>
		/// Parse dynamic tags
		/// </summary>
		/// <param name="commandTextNode"></param>
		/// <param name="dynamic"></param>
		/// <param name="sqlBuffer"></param>
		/// <param name="isDynamic"></param>
		/// <param name="postParseRequired"></param>
		/// <param name="statement"></param>
		/// <returns></returns>
		private bool ParseDynamicTags( XmlNode commandTextNode, IDynamicParent dynamic, 
			StringBuilder sqlBuffer, bool isDynamic, bool postParseRequired, IStatement statement) 
		{
			XmlNodeList children = commandTextNode.ChildNodes;
			int count = children.Count;
			for (int i = 0; i < count; i++) 
			{
				XmlNode child = children[i];
				if ( (child.NodeType == XmlNodeType.CDATA) || (child.NodeType == XmlNodeType.Text) )
				{
					string data = child.InnerText.Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' '); //??

					data = NodeUtils.ParsePropertyTokens(data, _configScope.Properties);

					SqlText sqlText;
					if (postParseRequired) 
					{
						sqlText = new SqlText();
						sqlText.Text = data.ToString();
					} 
					else 
					{
						sqlText = _paramParser.ParseInlineParameterMap(_configScope, null, data );
					}

					dynamic.AddChild(sqlText);
					sqlBuffer.Append(data);
				}
                else if (child.Name == "include")
				{
                    NameValueCollection prop = NodeUtils.ParseAttributes(child, _configScope.Properties);
                    string refid = NodeUtils.GetStringAttribute(prop, "refid");
                    XmlNode includeNode = (XmlNode)_configScope.SqlIncludes[refid];

                    if (includeNode == null)
                    {
                        String nsrefid = _configScope.ApplyNamespace(refid);
                        includeNode = (XmlNode)_configScope.SqlIncludes[nsrefid];
                        if (includeNode == null)
                        {
                            throw new ConfigurationException("Could not find SQL tag to include with refid '" + refid + "'");
                        }
                    }
                    isDynamic = ParseDynamicTags(includeNode, dynamic, sqlBuffer, isDynamic, false, statement);
				}
				else
				{
					string nodeName = child.Name;
					IDeSerializer serializer = _deSerializerFactory.GetDeSerializer(nodeName);

					if (serializer != null) 
					{
						isDynamic = true;
						SqlTag tag;

						tag = serializer.Deserialize(child);

						dynamic.AddChild(tag);

						if (child.HasChildNodes) 
						{
							isDynamic = ParseDynamicTags( child, tag, sqlBuffer, isDynamic, tag.Handler.IsPostParseRequired, statement );
						}
					}
				}
			}

			return isDynamic;
		}


		#region Inline Parameter parsing

		/// <summary>
		/// Apply inline paremeterMap
		/// </summary>
		/// <param name="statement"></param>
		/// <param name="sqlStatement"></param>
		private void ApplyInlineParemeterMap( IStatement statement, string sqlStatement)
		{
			string newSql = sqlStatement;

			_configScope.ErrorContext.MoreInfo = "apply inline parameterMap";

			// Check the inline parameter
			if (statement.ParameterMap == null)
			{
				// Build a Parametermap with the inline parameters.
				// if they exist. Then delete inline infos from sqltext.
				
				SqlText sqlText = _paramParser.ParseInlineParameterMap(_configScope,  statement, newSql );

				if (sqlText.Parameters.Length > 0)
				{
					ParameterMap map = new ParameterMap(_configScope.DataExchangeFactory);
					map.Id = statement.Id + "-InLineParameterMap";
					if (statement.ParameterClass!=null)
					{
						map.Class = statement.ParameterClass;
					}
					map.Initialize(_configScope.DataSource.DbProvider.UsePositionalParameters,_configScope);
					if (statement.ParameterClass==null && 
						sqlText.Parameters.Length==1 && sqlText.Parameters[0].PropertyName=="value")//#value# parameter with no parameterClass attribut
					{
						map.DataExchange = _configScope.DataExchangeFactory.GetDataExchangeForClass( typeof(int) );//Get the primitiveDataExchange
					}
					statement.ParameterMap = map;	
				
					int lenght = sqlText.Parameters.Length;
					for(int index=0;index<lenght;index++)
					{
						map.AddParameterProperty( sqlText.Parameters[index] );
					}
				}
				newSql = sqlText.Text;
			}

			ISql sql = null;

			newSql = newSql.Trim();

			if (SimpleDynamicSql.IsSimpleDynamicSql(newSql)) 
			{
				sql = new SimpleDynamicSql(_configScope, newSql, statement);
			} 
			else 
			{
				if (statement is Procedure)
				{
					sql = new ProcedureSql(_configScope, newSql, statement);
					// Could not call BuildPreparedStatement for procedure because when NUnit Test
					// the database is not here (but in theory procedure must be prepared like statement)
					// It's even better as we can then switch DataSource.
				}
				else if (statement is Statement)
				{
					sql = new StaticSql(_configScope, statement);
					ISqlMapSession session = new SqlMapSession(_configScope.SqlMapper);

					((StaticSql)sql).BuildPreparedStatement( session, newSql );
				}					
			}
			statement.Sql = sql;
		}

		#endregion

		
		/// <summary>
		/// Initialize the list of variables defined in the
		/// properties file.
		/// </summary>
		private void ParseGlobalProperties()
		{
			XmlNode nodeProperties = _configScope.NodeContext.SelectSingleNode( ApplyDataMapperNamespacePrefix(XML_PROPERTIES), _configScope.XmlNamespaceManager);
			_configScope.ErrorContext.Activity = "loading global properties";
			
			if (nodeProperties != null)
			{
				if (nodeProperties.HasChildNodes)
				{
					foreach (XmlNode propertyNode in nodeProperties.SelectNodes( ApplyDataMapperNamespacePrefix(XML_PROPERTY), _configScope.XmlNamespaceManager))
					{
						XmlAttribute keyAttrib = propertyNode.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB];
						XmlAttribute valueAttrib = propertyNode.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB];

						if ( keyAttrib != null && valueAttrib!=null)
						{
							_configScope.Properties.Add( keyAttrib.Value,  valueAttrib.Value);

							if (_logger.IsDebugEnabled)
							{
								_logger.Debug( string.Format("Add property \"{0}\" value \"{1}\"",keyAttrib.Value,valueAttrib.Value) );
							}
						}
						else
						{
							// Load the file defined by the attribute
							XmlDocument propertiesConfig = Resources.GetAsXmlDocument(propertyNode, _configScope.Properties); 
							
							foreach (XmlNode node in propertiesConfig.SelectNodes( XML_GLOBAL_PROPERTIES, _configScope.XmlNamespaceManager))
							{
								_configScope.Properties[node.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB].Value] = node.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB].Value;

								if (_logger.IsDebugEnabled)
								{
									_logger.Debug( string.Format("Add property \"{0}\" value \"{1}\"",node.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB].Value,node.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB].Value) );
								}
							}
						}
					}
				}
				else
				{
					// JIRA-38 Fix 
					// <properties> element's InnerXml is currently an empty string anyway
					// since <settings> are in properties file

					_configScope.ErrorContext.Resource = nodeProperties.OuterXml.ToString();

					// Load the file defined by the attribute
					XmlDocument propertiesConfig = Resources.GetAsXmlDocument(nodeProperties, _configScope.Properties); 

					foreach (XmlNode node in propertiesConfig.SelectNodes( XML_SETTINGS_ADD ) )
					{
						_configScope.Properties[node.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB].Value] = node.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB].Value;

						if (_logger.IsDebugEnabled)
						{
							_logger.Debug( string.Format("Add property \"{0}\" value \"{1}\"",node.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB].Value,node.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB].Value) );
						}
					}					
				}
			}
			_configScope.ErrorContext.Reset();
		}



		/// <summary>
		/// Generate the command text for CRUD operation
		/// </summary>
		/// <param name="configScope"></param>
		/// <param name="statement"></param>
		private void GenerateCommandText(ConfigurationScope configScope, IStatement statement)
		{
			string generatedSQL;

			//------ Build SQL CommandText
			generatedSQL = SqlGenerator.BuildQuery(statement);

			ISql sql = new StaticSql(configScope, statement);
			ISqlMapSession session = new SqlMapSession(configScope.SqlMapper);

			((StaticSql)sql).BuildPreparedStatement( session, generatedSQL );
			statement.Sql = sql;
		}

		
		/// <summary>
		/// Build a ParameterMap
		/// </summary>
		private void BuildParameterMap()
		 {
			ParameterMap parameterMap;
			XmlNode parameterMapNode = _configScope.NodeContext;

			_configScope.ErrorContext.MoreInfo = "build ParameterMap";

			// Get the parameterMap id
            string id = _configScope.ApplyNamespace((parameterMapNode.Attributes.GetNamedItem("id")).Value);
			_configScope.ErrorContext.ObjectId = id;

			// Did we already process it ?
			if (_configScope.SqlMapper.ParameterMaps.Contains( id ) == false)
			{
				parameterMap = ParameterMapDeSerializer.Deserialize(parameterMapNode, _configScope);

                parameterMap.Id = _configScope.ApplyNamespace(parameterMap.Id);
				string attributeExtendMap = parameterMap.ExtendMap;
                parameterMap.ExtendMap = _configScope.ApplyNamespace(parameterMap.ExtendMap);

				if (parameterMap.ExtendMap.Length >0)
				{
					ParameterMap superMap;
					// Did we already build Extend ParameterMap ?
					if (_configScope.SqlMapper.ParameterMaps.Contains( parameterMap.ExtendMap ) == false)
					{
                        XmlNode superNode = _configScope.SqlMapDocument.SelectSingleNode(ApplyMappingNamespacePrefix(XML_SEARCH_PARAMETER) + attributeExtendMap + "']", _configScope.XmlNamespaceManager);

						if (superNode != null)
						{
							_configScope.ErrorContext.MoreInfo = "Build parent ParameterMap";
							_configScope.NodeContext = superNode;
							BuildParameterMap();
							superMap = _configScope.SqlMapper.GetParameterMap( parameterMap.ExtendMap );
						}
						else
						{
							throw new ConfigurationException("In mapping file '"+ _configScope.SqlMapNamespace +"' the parameterMap '"+parameterMap.Id+"' can not resolve extends attribute '"+parameterMap.ExtendMap+"'");
						}
					}
					else
					{
						superMap = _configScope.SqlMapper.GetParameterMap( parameterMap.ExtendMap );
					}
					// Add extends property
					int index = 0;

					foreach(string propertyName in superMap.GetPropertyNameArray())
					{
                        ParameterProperty property = superMap.GetProperty(propertyName).Clone();
                        property.Initialize(_configScope, parameterMap.Class);
                        parameterMap.InsertParameterProperty(index, property);
						index++;
					}
				}
				_configScope.SqlMapper.AddParameterMap( parameterMap );
			}
		}


		/// <summary>
		/// Build a ResultMap
		/// </summary>
		private void BuildResultMap()
		 {
			ResultMap resultMap;
			XmlNode resultMapNode = _configScope.NodeContext;

			_configScope.ErrorContext.MoreInfo = "build ResultMap";

            string id = _configScope.ApplyNamespace((resultMapNode.Attributes.GetNamedItem("id")).Value);
			_configScope.ErrorContext.ObjectId = id;

			// Did we alredy process it
			if (_configScope.SqlMapper.ResultMaps.Contains( id ) == false)
			{
				resultMap =  ResultMapDeSerializer.Deserialize( resultMapNode, _configScope );

				string attributeExtendMap = resultMap.ExtendMap;
                resultMap.ExtendMap = _configScope.ApplyNamespace(resultMap.ExtendMap);

				if (resultMap.ExtendMap!=null && resultMap.ExtendMap.Length >0)
				{
					IResultMap superMap = null;			    
					// Did we already build Extend ResultMap?
					if (_configScope.SqlMapper.ResultMaps.Contains( resultMap.ExtendMap ) == false)
					{
						XmlNode superNode = _configScope.SqlMapDocument.SelectSingleNode( ApplyMappingNamespacePrefix(XML_SEARCH_RESULTMAP)+ attributeExtendMap +"']", _configScope.XmlNamespaceManager);

						if (superNode != null)
						{
							_configScope.ErrorContext.MoreInfo = "Build parent ResultMap";
							_configScope.NodeContext = superNode;
							BuildResultMap();
							superMap = _configScope.SqlMapper.GetResultMap( resultMap.ExtendMap );
						}
						else
						{
							throw new ConfigurationException("In mapping file '"+_configScope.SqlMapNamespace+"' the resultMap '"+resultMap.Id+"' can not resolve extends attribute '"+resultMap.ExtendMap+"'" );
						}
					}
					else
					{
						superMap = _configScope.SqlMapper.GetResultMap( resultMap.ExtendMap );
					}

					// Add parent property
					for(int index=0; index< superMap.Properties.Count; index++)
					{
						ResultProperty property = superMap.Properties[index].Clone();
                        property.Initialize(_configScope, resultMap.Class);
						resultMap.Properties.Add(property);
					}
                    // Add groupBy properties
                    if (resultMap.GroupByPropertyNames.Count == 0)
                    {
                        for(int i=0; i<superMap.GroupByPropertyNames.Count; i++)
                        {
                            resultMap.GroupByPropertyNames.Add(superMap.GroupByPropertyNames[i]);
                        }
                    }
                    // Add constructor arguments 
                    if (resultMap.Parameters.Count == 0)
                    {
                       for (int i = 0; i < superMap.Parameters.Count; i++)
                       {
                           resultMap.Parameters.Add(superMap.Parameters[i]);
                       }
                       if (resultMap.Parameters.Count>0)
                       {
                           resultMap.SetObjectFactory(_configScope);
                       }
                    }


				    // Verify that that each groupBy element correspond to a class member
                    // of one of result property
                    for (int i = 0; i < resultMap.GroupByPropertyNames.Count; i++)
                    {
                        string memberName = resultMap.GroupByPropertyNames[i];
                        if (!resultMap.Properties.Contains(memberName))
                        {
                            throw new ConfigurationException(
                                string.Format(
                                    "Could not configure ResultMap named \"{0}\". Check the groupBy attribute. Cause: there's no result property named \"{1}\".",
                                    resultMap.Id, memberName));
                        }
                    }
				}
			    resultMap.InitializeGroupByProperties();
				_configScope.SqlMapper.AddResultMap( resultMap );
			}
		 }


		/// <summary>
		/// Gets a resource stream.
		/// </summary>
		/// <param name="schemaResourceKey">The schema resource key.</param>
		/// <returns>A resource stream.</returns>
		public Stream GetStream( string schemaResourceKey )
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream("IBatisNet.DataMapper." + schemaResourceKey); 
		}


		/// <summary>
		/// Apply the dataMapper namespace prefix
		/// </summary>
		/// <param name="elementName"></param>
		/// <returns></returns>
		public string ApplyDataMapperNamespacePrefix( string elementName )
		{
			return DATAMAPPER_NAMESPACE_PREFIX+ ":" + elementName.
				Replace("/","/"+DATAMAPPER_NAMESPACE_PREFIX+":");
		}

		/// <summary>
		/// Apply the provider namespace prefix
		/// </summary>
		/// <param name="elementName"></param>
		/// <returns></returns>
		public string ApplyProviderNamespacePrefix( string elementName )
		{
			return PROVIDERS_NAMESPACE_PREFIX+ ":" + elementName.
				Replace("/","/"+PROVIDERS_NAMESPACE_PREFIX+":");
		}

		/// <summary>
		/// Apply the provider namespace prefix
		/// </summary>
		/// <param name="elementName"></param>
		/// <returns></returns>
		public static string ApplyMappingNamespacePrefix( string elementName )
		{
			return MAPPING_NAMESPACE_PREFIX+ ":" + elementName.
				Replace("/","/"+MAPPING_NAMESPACE_PREFIX+":");
		}

		#endregion
	}
}
