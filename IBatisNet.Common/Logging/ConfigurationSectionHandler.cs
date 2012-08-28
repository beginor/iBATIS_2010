
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 474141 $
 * $Date: 2006-11-12 21:43:37 -0700 (Sun, 12 Nov 2006) $
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

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;
using IBatisNet.Common.Logging.Impl;
using ConfigurationException = IBatisNet.Common.Exceptions.ConfigurationException;

namespace IBatisNet.Common.Logging
{
	/// <summary>
	/// Used in an application's configuration file (App.Config or Web.Config) to configure the logging subsystem.
	/// </summary>
	/// <remarks>
	/// <example>
	/// An example configuration section that writes IBatisNet messages to the Console using the built-in Console Logger.
	/// <code lang="XML" escaped="true">
	/// <configuration>
	///		<configSections>
	///			<sectionGroup name="iBATIS">
	///				<section name="logging" type="IBatisNet.Common.Logging.ConfigurationSectionHandler, IBatisNet.Common" />
	///			</sectionGroup>	
	///		</configSections>
	///		<iBATIS>
	///			<logging>
	///				<logFactoryAdapter type="IBatisNet.Common.Logging.Impl.ConsoleOutLoggerFA, IBatisNet.Common">
	///					<arg key="showLogName" value="true" />
	///					<arg key="showDataTime" value="true" />
	///					<arg key="level" value="ALL" />
	///					<arg key="dateTimeFormat" value="yyyy/MM/dd HH:mm:ss:SSS" />
	///				</logFactoryAdapter>
	///			</logging>
	///		</iBATIS>
	/// </configuration>
	/// </code> 
	/// </example>
	/// <para>
	/// The following aliases are recognized for the type attribute of logFactoryAdapter: 
	/// </para>
	/// <list type="table">
	/// <item><term>CONSOLE</term><description>Alias for IBatisNet.Common.Logging.Impl.ConsoleOutLoggerFA, IBatisNet.Common</description></item>
	/// <item><term>TRACE</term><description>Alias for IBatisNet.Common.Logging.Impl.TraceLoggerFA, IBatisNet.Common</description></item>
	/// <item><term>NOOP</term><description>Alias IBatisNet.Common.Logging.Impl.NoOpLoggerFA, IBatisNet.Common</description></item>
	/// </list>
	/// </remarks>
	public class ConfigurationSectionHandler: IConfigurationSectionHandler
	{

		#region Fields

		private static readonly string LOGFACTORYADAPTER_ELEMENT = "logFactoryAdapter";
		private static readonly string LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB = "type";
		private static readonly string ARGUMENT_ELEMENT = "arg";
		private static readonly string ARGUMENT_ELEMENT_KEY_ATTRIB = "key";
		private static readonly string ARGUMENT_ELEMENT_VALUE_ATTRIB = "value";

		#endregion 

		/// <summary>
		/// Constructor
		/// </summary>
		public ConfigurationSectionHandler()
		{
		}

		/// <summary>
		/// Retrieves the <see cref="Type" /> of the logger the use by looking at the logFactoryAdapter element
		/// of the logging configuration element.
		/// </summary>
		/// <param name="section"></param>
		/// <returns>
		/// A <see cref="LogSetting" /> object containing the specified type that implements 
		/// <see cref="ILoggerFactoryAdapter" /> along with zero or more properties that will be 
		/// passed to the logger factory adapter's constructor as an <see cref="IDictionary" />.
		/// </returns>
		private LogSetting ReadConfiguration( XmlNode section )
		{
			XmlNode logFactoryElement = section.SelectSingleNode( LOGFACTORYADAPTER_ELEMENT );
			
			string factoryTypeString = string.Empty;
			if ( logFactoryElement.Attributes[LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB] != null )
				factoryTypeString = logFactoryElement.Attributes[LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB].Value;
            
			if ( factoryTypeString == string.Empty )
			{
				throw new ConfigurationException
					( "Required Attribute '" 
					+ LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB 
					+ "' not found in element '"
					+ LOGFACTORYADAPTER_ELEMENT
					+ "'"
					);
			}

			Type factoryType = null;
			try
			{
				if (String.Compare(factoryTypeString, "CONSOLE", true) == 0)
				{
					factoryType = typeof(ConsoleOutLoggerFA);
				}
				else if (String.Compare(factoryTypeString, "TRACE", true) == 0)
				{
					factoryType = typeof(TraceLoggerFA);
				}
				else if (String.Compare(factoryTypeString, "NOOP", true) == 0)
				{
					factoryType = typeof(NoOpLoggerFA);
				}
				else
				{
					factoryType = Type.GetType( factoryTypeString, true, false );
				}
			}
			catch ( Exception e )
			{
				throw new ConfigurationException
					( "Unable to create type '" + factoryTypeString + "'"
					  , e
					);
			}
			
			XmlNodeList propertyNodes = logFactoryElement.SelectNodes( ARGUMENT_ELEMENT );

			NameValueCollection properties = null;
#if dotnet2
            properties = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);
#else
			properties = new NameValueCollection( null, new CaseInsensitiveComparer() );
#endif
			foreach ( XmlNode propertyNode in propertyNodes )
			{
				string key = string.Empty;
				string itsValue = string.Empty;

				XmlAttribute keyAttrib = propertyNode.Attributes[ARGUMENT_ELEMENT_KEY_ATTRIB];
				XmlAttribute valueAttrib = propertyNode.Attributes[ARGUMENT_ELEMENT_VALUE_ATTRIB];

				if ( keyAttrib == null )
				{
					throw new ConfigurationException
						( "Required Attribute '" 
						  + ARGUMENT_ELEMENT_KEY_ATTRIB 
						  + "' not found in element '"
						  + ARGUMENT_ELEMENT
						  + "'"
						);
				}
				else
				{
					key = keyAttrib.Value;
				}

				if ( valueAttrib != null )
				{
					itsValue = valueAttrib.Value;
				}

				properties.Add( key, itsValue );
			}

			return new LogSetting( factoryType, properties );
		}

		#region IConfigurationSectionHandler Members

		/// <summary>
		/// Verifies that the logFactoryAdapter element appears once in the configuration section.
		/// </summary>
		/// <param name="parent">The parent of the current item.</param>
		/// <param name="configContext">Additional information about the configuration process.</param>
		/// <param name="section">The configuration section to apply an XPath query too.</param>
		/// <returns>
		/// A <see cref="LogSetting" /> object containing the specified logFactoryAdapter type
		/// along with user supplied configuration properties.
		/// </returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			int logFactoryElementsCount = section.SelectNodes( LOGFACTORYADAPTER_ELEMENT ).Count;
			
			if ( logFactoryElementsCount > 1 )
			{
				throw new ConfigurationException( "Only one <logFactoryAdapter> element allowed" );
			}
			else if ( logFactoryElementsCount == 1 )
			{
				return ReadConfiguration( section );
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}

