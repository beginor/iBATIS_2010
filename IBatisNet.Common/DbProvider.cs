
#region Apache Notice
/*****************************************************************************
 * $Revision: 408099 $
 * $LastChangedDate: 2006-05-20 15:56:36 -0600 (Sat, 20 May 2006) $
 * $LastChangedBy: gbayon $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2006/2005 - The Apache Software Foundation
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
using System.Data;
using System.Reflection;
using System.Xml.Serialization;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;

#endregion

namespace IBatisNet.Common
{
	/// <summary>
	/// Information about a data provider.
	/// </summary>
	[Serializable]
	[XmlRoot("provider", Namespace="http://ibatis.apache.org/providers")]
	public class DbProvider : IDbProvider
	{
        private const string SQLPARAMETER = "?";

		#region Fields
		[NonSerialized]
		private string _assemblyName = string.Empty;
		[NonSerialized]
		private string _connectionClass = string.Empty;
		[NonSerialized]
		private string _commandClass = string.Empty;

		[NonSerialized]
		private string _parameterDbTypeClass = string.Empty;
		[NonSerialized]
		private Type _parameterDbType = null;

		[NonSerialized]
		private string _parameterDbTypeProperty = string.Empty;
		[NonSerialized]
		private string _dataAdapterClass = string.Empty;
		[NonSerialized]
		private string _commandBuilderClass = string.Empty;

		[NonSerialized]
		private string _name = string.Empty;
		[NonSerialized]
		private string _description = string.Empty;
		[NonSerialized]
		private bool _isDefault = false;
		[NonSerialized]
		private bool _isEnabled = true;
		[NonSerialized]
		private IDbConnection _templateConnection = null;
		[NonSerialized]
		private IDbDataAdapter _templateDataAdapter= null;
		[NonSerialized]
		private Type _commandBuilderType = null;
		[NonSerialized]
		private string _parameterPrefix = string.Empty;
		[NonSerialized]
		private bool _useParameterPrefixInSql = true;
		[NonSerialized]
		private bool _useParameterPrefixInParameter = true;
		[NonSerialized]
		private bool _usePositionalParameters = false;
		[NonSerialized]
		private bool _templateConnectionIsICloneable = false;
		[NonSerialized]
		private bool _templateDataAdapterIsICloneable = false;
		[NonSerialized]
		private bool _setDbParameterSize = true;
		[NonSerialized]
		private bool _setDbParameterPrecision = true;
		[NonSerialized]
		private bool _setDbParameterScale = true;
		[NonSerialized]
		private bool _useDeriveParameters = true;
		[NonSerialized]
		private bool _allowMARS = false;

		
//		private static readonly ILog _connectionLogger = LogManager.GetLogger("System.Data.IDbConnection");

		#endregion
		
		#region Properties


		/// <summary>
		/// The name of the assembly which conatins the definition of the provider.
		/// </summary>
		/// <example>Examples : "System.Data", "Microsoft.Data.Odbc"</example>
		[XmlAttribute("assemblyName")]
		public string AssemblyName
		{
			get { return _assemblyName; }
			set
			{
				CheckPropertyString("AssemblyName", value);
				_assemblyName = value;
			}
		}


		/// <summary>
		/// Tell us if it is the default data source.
		/// Default false.
		/// </summary>
		[XmlAttribute("default")]
		public bool IsDefault
		{             
			get { return _isDefault; }
			set {_isDefault = value;}
		}
		

		/// <summary>
		/// Tell us if this provider is enabled.
		/// Default true.
		/// </summary>
		[XmlAttribute("enabled")]
		public bool IsEnabled
		{             
			get { return _isEnabled; }
			set {_isEnabled = value;}
		}

		/// <summary>
		/// Tell us if this provider allows having multiple open <see cref="IDataReader"/> with
		/// the same <see cref="IDbConnection"/>.
		/// </summary>
		/// <remarks>
		/// It's a new feature in ADO.NET 2.0 and Sql Server 2005 that allows for multiple forward only read only result sets (MARS).
		/// Some databases have supported this functionality for a long time :
		/// Not Supported : DB2, MySql.Data, OLE DB provider [except Sql Server 2005 when using MDAC 9], SQLite, Obdc 
		/// Supported :  Sql Server 2005, Npgsql
		/// </remarks>
		[XmlAttribute("allowMARS")]
		public bool AllowMARS
		{             
			get { return _allowMARS; }
			set {_allowMARS = value;}
		}
	
		/// <summary>
		/// The connection class name to use.
		/// </summary>
		/// <example>
		/// "System.Data.OleDb.OleDbConnection", 
		/// "System.Data.SqlClient.SqlConnection", 
		/// "Microsoft.Data.Odbc.OdbcConnection"
		/// </example>
		[XmlAttribute("connectionClass")]
		public string DbConnectionClass
		{             
			get { return _connectionClass; }
			set
			{
				CheckPropertyString("DbConnectionClass", value);
				_connectionClass = value;
			}
		}

		/// <summary>
		/// Does this ConnectionProvider require the use of a Named Prefix in the SQL 
		/// statement. 
		/// </summary>
		/// <remarks>
		/// The OLE DB/ODBC .NET Provider does not support named parameters for 
		/// passing parameters to an SQL Statement or a stored procedure called 
		/// by an IDbCommand when CommandType is set to Text.
		/// 
		/// For example, SqlClient requires select * from simple where simple_id = @simple_id
		/// If this is false, like with the OleDb or Obdc provider, then it is assumed that 
		/// the ? can be a placeholder for the parameter in the SQL statement when CommandType 
		/// is set to Text.		
		/// </remarks>
		[XmlAttribute("useParameterPrefixInSql")]
		public bool UseParameterPrefixInSql 
		{
			get { return _useParameterPrefixInSql; }
			set { _useParameterPrefixInSql = value;}
		}
		
		/// <summary>
		/// Does this ConnectionProvider require the use of the Named Prefix when trying
		/// to reference the Parameter in the Command's Parameter collection. 
		/// </summary>
		/// <remarks>
		/// This is really only useful when the UseParameterPrefixInSql = true. 
		/// When this is true the code will look like IDbParameter param = cmd.Parameters["@paramName"], 
		/// if this is false the code will be IDbParameter param = cmd.Parameters["paramName"] - ie - Oracle.
		/// </remarks>
		[XmlAttribute("useParameterPrefixInParameter")]
		public bool UseParameterPrefixInParameter  
		{
			get { return _useParameterPrefixInParameter; }
			set { _useParameterPrefixInParameter = value; }		
		}

		/// <summary>
		/// The OLE DB/OBDC .NET Provider uses positional parameters that are marked with a 
		/// question mark (?) instead of named parameters.
		/// </summary>
		[XmlAttribute("usePositionalParameters")]
		public bool UsePositionalParameters  
		{
			get { return _usePositionalParameters; }
			set { _usePositionalParameters = value; }		
		}

		/// <summary>
		/// Used to indicate whether or not the provider 
		/// supports parameter size.
		/// </summary>
		/// <remarks>
		/// See JIRA-49 about SQLite.Net provider not supporting parameter size.
		/// </remarks>
		[XmlAttribute("setDbParameterSize")]
		public bool SetDbParameterSize
		{
			get { return _setDbParameterSize; }
			set { _setDbParameterSize = value; }		
		}

		/// <summary>
		/// Used to indicate whether or not the provider 
		/// supports parameter precision.
		/// </summary>
		/// <remarks>
		/// See JIRA-49 about SQLite.Net provider not supporting parameter precision.
		/// </remarks>
		[XmlAttribute("setDbParameterPrecision")]
		public bool SetDbParameterPrecision
		{
			get { return _setDbParameterPrecision; }
			set { _setDbParameterPrecision = value; }		
		}

		/// <summary>
		/// Used to indicate whether or not the provider 
		/// supports a parameter scale.
		/// </summary>
		/// <remarks>
		/// See JIRA-49 about SQLite.Net provider not supporting parameter scale.
		/// </remarks>
		[XmlAttribute("setDbParameterScale")]
		public bool SetDbParameterScale
		{
			get { return _setDbParameterScale; }
			set { _setDbParameterScale = value; }		
		}

		/// <summary>
		/// Used to indicate whether or not the provider 
		/// supports DeriveParameters method for procedure.
		/// </summary>
		[XmlAttribute("useDeriveParameters")]
		public bool UseDeriveParameters
		{
			get { return _useDeriveParameters; }
			set { _useDeriveParameters = value; }		
		}

		/// <summary>
		/// The command class name to use.
		/// </summary>
		/// <example>
		/// "System.Data.SqlClient.SqlCommand"
		/// </example>
		[XmlAttribute("commandClass")]
		public string DbCommandClass
		{             
			get { return _commandClass; }
			set
			{
				CheckPropertyString("DbCommandClass", value);
				_commandClass = value;
			}
		}

	
		/// <summary>
		/// The ParameterDbType class name to use.
		/// </summary>			
		/// <example>
		/// "System.Data.SqlDbType"
		/// </example>
		[XmlAttribute("parameterDbTypeClass")]
		public string ParameterDbTypeClass
		{             
			get { return _parameterDbTypeClass; }
			set
			{
				CheckPropertyString("ParameterDbTypeClass", value);
				_parameterDbTypeClass = value;
			}
		}


		/// <summary>
		/// The ParameterDbTypeProperty class name to use.
		/// </summary>
		/// <example >
		/// SqlDbType in SqlParamater.SqlDbType, 
		/// OracleType in OracleParameter.OracleType.
		/// </example>
		[XmlAttribute("parameterDbTypeProperty")]
		public string ParameterDbTypeProperty
		{             
			get { return _parameterDbTypeProperty; }
			set
			{
				CheckPropertyString("ParameterDbTypeProperty", value);
				_parameterDbTypeProperty = value;
			}
		}

		/// <summary>
		/// The dataAdapter class name to use.
		/// </summary>
		/// <example >
		/// "System.Data.SqlDbType"
		/// </example>
		[XmlAttribute("dataAdapterClass")]
		public string DataAdapterClass
		{             
			get { return _dataAdapterClass; }
			set
			{
				CheckPropertyString("DataAdapterClass", value);
				_dataAdapterClass = value;
			}
		}

		/// <summary>
		/// The commandBuilder class name to use.
		/// </summary>
		/// <example >
		/// "System.Data.OleDb.OleDbCommandBuilder", 
		/// "System.Data.SqlClient.SqlCommandBuilder", 
		/// "Microsoft.Data.Odbc.OdbcCommandBuilder"
		/// </example>
		[XmlAttribute("commandBuilderClass")]
		public string CommandBuilderClass
		{             
			get { return _commandBuilderClass; }
			set
			{
				CheckPropertyString("CommandBuilderClass", value);
				_commandBuilderClass = value;
			}
		}


		/// <summary>
		/// Name used to identify the provider amongst the others.
		/// </summary>
		[XmlAttribute("name")]
		public string Name
		{
			get { return _name; }
			set 
			{ 
				CheckPropertyString("Name", value);
				_name = value; 			
			}
		}

		/// <summary>
		/// Description.
		/// </summary>
		[XmlAttribute("description")]
		public string Description
		{
			get { return _description; }
			set { _description = value;}
		}
		
		/// <summary>
		/// Parameter prefix use in store procedure.
		/// </summary>
		/// <example> @ for Sql Server.</example>
		[XmlAttribute("parameterPrefix")]
		public string ParameterPrefix
		{
			get { return _parameterPrefix; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
				{
					_parameterPrefix = ""; 
				}
				else
				{
					_parameterPrefix = value; 
				}
			}
		}

		/// <summary>
		/// Check if this provider is Odbc ?
		/// </summary>
		[XmlIgnore]
		public bool IsObdc
		{
			get { return (_connectionClass.IndexOf(".Odbc.")>0); }
		}

		/// <summary>
		/// Get the CommandBuilder Type for this provider.
		/// </summary>
		/// <returns>An object.</returns>
		public Type CommandBuilderType
		{
			get {return _commandBuilderType;}
		}

		/// <summary>
		/// Get the ParameterDb Type for this provider.
		/// </summary>
		/// <returns>An object.</returns>
		[XmlIgnore]
		public Type ParameterDbType
		{
			get { return _parameterDbType; }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		public DbProvider()
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// Init the provider.
		/// </summary>
		public void Initialize()
		{
			Assembly assembly = null;
			Type type = null;

			try
			{
				assembly = Assembly.Load(_assemblyName);

				// Build the DataAdapter template 
				type = assembly.GetType(_dataAdapterClass, true);
				CheckPropertyType("DataAdapterClass", typeof(IDbDataAdapter), type);
				_templateDataAdapter = (IDbDataAdapter)type.GetConstructor(Type.EmptyTypes).Invoke(null);
				
				// Build the connection template 
				type = assembly.GetType(_connectionClass, true);
				CheckPropertyType("DbConnectionClass", typeof(IDbConnection), type);
				_templateConnection = (IDbConnection)type.GetConstructor(Type.EmptyTypes).Invoke(null);

				// Get the CommandBuilder Type
				_commandBuilderType = assembly.GetType(_commandBuilderClass, true);
				if (_parameterDbTypeClass.IndexOf(',')>0)
				{
                    _parameterDbType = TypeUtils.ResolveType(_parameterDbTypeClass);
				}
				else
				{
					_parameterDbType = assembly.GetType(_parameterDbTypeClass, true);
				}

				_templateConnectionIsICloneable = _templateConnection is ICloneable;
				_templateDataAdapterIsICloneable = _templateDataAdapter is ICloneable;
			}
			catch(Exception e)
			{
				throw new ConfigurationException(
					string.Format("Could not configure providers. Unable to load provider named \"{0}\" not found, failed. Cause: {1}", _name, e.Message), e
					);
			}
		}


		/// <summary>
		/// Create a connection object for this provider.
		/// </summary>
		/// <returns>An 'IDbConnection' object.</returns>
		public virtual IDbConnection CreateConnection()
		{
			// Cannot do that because on 
			// IDbCommand.Connection = cmdConnection
			// .NET cast the cmdConnection to the real type (as SqlConnection)
			// and we pass a proxy --> exception invalid cast !
//			if (_connectionLogger.IsDebugEnabled)
//			{
//				connection = (IDbConnection)IDbConnectionProxy.NewInstance(connection, this);
//			}
			if (_templateConnectionIsICloneable)
			{
				return (IDbConnection) ((ICloneable)_templateConnection).Clone();
			}
			else
			{
				return (IDbConnection) Activator.CreateInstance(_templateConnection.GetType());
			}
		}

		
		/// <summary>
		/// Create a command object for this provider.
		/// </summary>
		/// <returns>An 'IDbCommand' object.</returns>
		public virtual IDbCommand CreateCommand()
		{
            return _templateConnection.CreateCommand();
		}

		/// <summary>
		/// Create a dataAdapter object for this provider.
		/// </summary>
		/// <returns>An 'IDbDataAdapter' object.</returns>
		public virtual IDbDataAdapter CreateDataAdapter()
		{
			if (_templateDataAdapterIsICloneable)
			{
				return (IDbDataAdapter) ((ICloneable)_templateDataAdapter).Clone();
			}
			else
			{
				return (IDbDataAdapter) Activator.CreateInstance(_templateDataAdapter.GetType());
			}
		}


		/// <summary>
		/// Create a IDbDataParameter object for this provider.
		/// </summary>
		/// <returns>An 'IDbDataParameter' object.</returns>
		public virtual IDbDataParameter CreateDataParameter()
		{
            return _templateConnection.CreateCommand().CreateParameter();
		}

        /// <summary>
        /// Change the parameterName into the correct format IDbCommand.CommandText
        /// for the ConnectionProvider
        /// </summary>
        /// <param name="parameterName">The unformatted name of the parameter</param>
        /// <returns>A parameter formatted for an IDbCommand.CommandText</returns>
        public virtual string FormatNameForSql(string parameterName)
        {
            return _useParameterPrefixInSql ? (_parameterPrefix + parameterName) : SQLPARAMETER;
        }

        /// <summary>
        /// Changes the parameterName into the correct format for an IDbParameter
        /// for the Driver.
        /// </summary>
        /// <remarks>
        /// For SqlServerConnectionProvider it will change <c>id</c> to <c>@id</c>
        /// </remarks>
        /// <param name="parameterName">The unformatted name of the parameter</param>
        /// <returns>A parameter formatted for an IDbParameter.</returns>
        public virtual string FormatNameForParameter(string parameterName)
        {
            return _useParameterPrefixInParameter ? (_parameterPrefix + parameterName) : parameterName;
        }
        
        /// <summary>
		/// Equals implemantation.
		/// </summary>
		/// <param name="obj">The test object.</param>
		/// <returns>A boolean.</returns>
		public override bool Equals(object obj)
		{
			if ((obj != null) && (obj is IDbProvider))
			{
				IDbProvider that = (IDbProvider) obj;
				return ((this._name == that.Name) && 
					(this._assemblyName == that.AssemblyName) &&
					(this._connectionClass == that.DbConnectionClass));
			}
			return false;
		}

		/// <summary>
		/// A hashcode for the provider.
		/// </summary>
		/// <returns>An integer.</returns>
		public override int GetHashCode()
		{
			return (_name.GetHashCode() ^ _assemblyName.GetHashCode() ^ _connectionClass.GetHashCode());
		}

		/// <summary>
		/// ToString implementation.
		/// </summary>
		/// <returns>A string that describes the provider.</returns>
		public override string ToString()
		{
			return "Provider " + _name;
		}

		private void CheckPropertyString(string propertyName, string value)
		{
			if (value == null || value.Trim().Length == 0)
			{
				throw new ArgumentException(
					"The "+propertyName+" property cannot be " +
					"set to a null or empty string value.", propertyName);
			}
		}

		private void CheckPropertyType(string propertyName,Type expectedType, Type value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(
					propertyName, "The "+propertyName+" property cannot be null.");
			}
			if (!expectedType.IsAssignableFrom(value))
			{
				throw new ArgumentException(
					"The Type passed to the "+propertyName+" property must be an "+expectedType.Name+" implementation.");
			}
		}
		#endregion

	}
}
