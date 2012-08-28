using System;
using System.Data;
using System.Xml.Serialization;

namespace IBatisNet.Common
{
	/// <summary>
	/// Factory interface to create provider specific ado.net objects.
	/// </summary>
	public interface IDbProvider
	{
		/// <summary>
		/// The name of the assembly which conatins the definition of the provider.
		/// </summary>
		/// <example>Examples : "System.Data", "Microsoft.Data.Odbc"</example>
		[XmlAttribute("assemblyName")]
		string AssemblyName { get; set; }

		/// <summary>
		/// Tell us if it is the default data source.
		/// Default false.
		/// </summary>
		[XmlAttribute("default")]
		bool IsDefault { get; set; }

		/// <summary>
		/// Tell us if this provider is enabled.
		/// Default true.
		/// </summary>
		[XmlAttribute("enabled")]
		bool IsEnabled { get; set; }

		/// <summary>
		/// Tell us if this provider allows having multiple open <see cref="IDataReader"/> with
		/// the same <see cref="IDbConnection"/>.
		/// </summary>
		[XmlAttribute("allowMultipleActiveDataReaders")]
		bool AllowMARS { get; set; }

		/// <summary>
		/// The connection class name to use.
		/// </summary>
		/// <example>
		/// "System.Data.OleDb.OleDbConnection", 
		/// "System.Data.SqlClient.SqlConnection", 
		/// "Microsoft.Data.Odbc.OdbcConnection"
		/// </example>
		[XmlAttribute("connectionClass")]
		string DbConnectionClass { get; set; }

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
		bool UseParameterPrefixInSql { get; set; }

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
		bool UseParameterPrefixInParameter { get; set; }

		/// <summary>
		/// The OLE DB/OBDC .NET Provider uses positional parameters that are marked with a 
		/// question mark (?) instead of named parameters.
		/// </summary>
		[XmlAttribute("usePositionalParameters")]
		bool UsePositionalParameters { get; set; }

		/// <summary>
		/// Used to indicate whether or not the provider 
		/// supports parameter size.
		/// </summary>
		/// <remarks>
		/// See JIRA-49 about SQLite.Net provider not supporting parameter size.
		/// </remarks>
		[XmlAttribute("setDbParameterSize")]
		bool SetDbParameterSize { get; set; }

		/// <summary>
		/// Used to indicate whether or not the provider 
		/// supports parameter precision.
		/// </summary>
		/// <remarks>
		/// See JIRA-49 about SQLite.Net provider not supporting parameter precision.
		/// </remarks>
		[XmlAttribute("setDbParameterPrecision")]
		bool SetDbParameterPrecision { get; set; }

		/// <summary>
		/// Used to indicate whether or not the provider 
		/// supports a parameter scale.
		/// </summary>
		/// <remarks>
		/// See JIRA-49 about SQLite.Net provider not supporting parameter scale.
		/// </remarks>
		[XmlAttribute("setDbParameterScale")]
		bool SetDbParameterScale { get; set; }

		/// <summary>
		/// Used to indicate whether or not the provider 
		/// supports DeriveParameters method for procedure.
		/// </summary>
		[XmlAttribute("useDeriveParameters")]
		bool UseDeriveParameters { get; set; }

		/// <summary>
		/// The command class name to use.
		/// </summary>
		/// <example>
		/// "System.Data.SqlClient.SqlCommand"
		/// </example>
		[XmlAttribute("commandClass")]
		string DbCommandClass { get; set; }

		/// <summary>
		/// The ParameterDbType class name to use.
		/// </summary>			
		/// <example>
		/// "System.Data.SqlDbType"
		/// </example>
		[XmlAttribute("parameterDbTypeClass")]
		string ParameterDbTypeClass { get; set; }

		/// <summary>
		/// The ParameterDbTypeProperty class name to use.
		/// </summary>
		/// <example >
		/// SqlDbType in SqlParamater.SqlDbType, 
		/// OracleType in OracleParameter.OracleType.
		/// </example>
		[XmlAttribute("parameterDbTypeProperty")]
		string ParameterDbTypeProperty { get; set; }

		/// <summary>
		/// The dataAdapter class name to use.
		/// </summary>
		/// <example >
		/// "System.Data.SqlDbType"
		/// </example>
		[XmlAttribute("dataAdapterClass")]
		string DataAdapterClass { get; set; }

		/// <summary>
		/// The commandBuilder class name to use.
		/// </summary>
		/// <example >
		/// "System.Data.OleDb.OleDbCommandBuilder", 
		/// "System.Data.SqlClient.SqlCommandBuilder", 
		/// "Microsoft.Data.Odbc.OdbcCommandBuilder"
		/// </example>
		[XmlAttribute("commandBuilderClass")]
		string CommandBuilderClass { get; set; }

		/// <summary>
		/// Name used to identify the provider amongst the others.
		/// </summary>
		[XmlAttribute("name")]
		string Name { get; set; }

		/// <summary>
		/// Description.
		/// </summary>
		[XmlAttribute("description")]
		string Description { get; set; }

		/// <summary>
		/// Parameter prefix use in store procedure.
		/// </summary>
		/// <example> @ for Sql Server.</example>
		[XmlAttribute("parameterPrefix")]
		string ParameterPrefix { get; set; }

		/// <summary>
		/// Check if this provider is Odbc ?
		/// </summary>
		[XmlIgnore]
		bool IsObdc { get; }

		/// <summary>
		/// Create a connection object for this provider.
		/// </summary>
		/// <returns>An 'IDbConnection' object.</returns>
		IDbConnection CreateConnection();

		/// <summary>
		/// Create a command object for this provider.
		/// </summary>
		/// <returns>An 'IDbCommand' object.</returns>
		IDbCommand CreateCommand();

		/// <summary>
		/// Create a dataAdapter object for this provider.
		/// </summary>
		/// <returns>An 'IDbDataAdapter' object.</returns>
		IDbDataAdapter CreateDataAdapter();

		/// <summary>
		/// Create a IDataParameter object for this provider.
		/// </summary>
		/// <returns>An 'IDbDataParameter' object.</returns>
		IDbDataParameter CreateDataParameter();

		/// <summary>
		/// Create the CommandBuilder Type for this provider.
		/// </summary>
		/// <returns>An object.</returns>
		Type CommandBuilderType { get; }

		/// <summary>
		/// Get the ParameterDb Type for this provider.
		/// </summary>
		/// <returns>An object.</returns>
		Type ParameterDbType { get; }


        /// <summary>
        /// Change the parameterName into the correct format IDbCommand.CommandText
        /// for the ConnectionProvider
        /// </summary>
        /// <param name="parameterName">The unformatted name of the parameter</param>
        /// <returns>A parameter formatted for an IDbCommand.CommandText</returns>
        string FormatNameForSql(string parameterName);

        /// <summary>
        /// Changes the parameterName into the correct format for an IDbParameter
        /// for the Driver.
        /// </summary>
        /// <remarks>
        /// For SqlServerConnectionProvider it will change <c>id</c> to <c>@id</c>
        /// </remarks>
        /// <param name="parameterName">The unformatted name of the parameter</param>
        /// <returns>A parameter formatted for an IDbParameter.</returns>
        string FormatNameForParameter(string parameterName);

		/// <summary>
		/// Init the provider.
		/// </summary>
		void Initialize();

	}
}