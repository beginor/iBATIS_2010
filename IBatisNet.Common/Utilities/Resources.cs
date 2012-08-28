
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
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Xml;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Logging;
using IBatisNet.Common.Utilities.TypesResolver;
using IBatisNet.Common.Xml;

#endregion

namespace IBatisNet.Common.Utilities
{
	/// <summary>
	/// A class to simplify access to resources.
	/// 
	/// The file can be loaded from the application root directory 
	/// (use the resource attribute) 
	/// or from any valid URL (use the url attribute). 
	/// For example,to load a fixed path file, use:
	/// &lt;properties url=”file:///c:/config/my.properties” /&gt;
	/// </summary>
	public class Resources
	{

		#region Fields
		private static string _applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
		private static string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //private static CachedTypeResolver _cachedTypeResolver = null;

		private static readonly ILog _logger = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );

		#endregion

		#region Properties
		/// <summary>
		/// The name of the directory containing the application
		/// </summary>
		public static string ApplicationBase
		{
			get
			{
				return _applicationBase;
			}
		}

		/// <summary>
		/// The name of the directory used to probe the assemblies.
		/// </summary>
		public static string BaseDirectory
		{
			get
			{
				return _baseDirectory;
			}
		}


		#endregion

		#region Constructor (s) / Destructor
		static Resources()
		{
            //_cachedTypeResolver = new CachedTypeResolver();
		}
		#endregion

		#region Methods

		/// <summary>
		/// Protocole separator
		/// </summary>
		 public const string PROTOCOL_SEPARATOR = "://";

		/// <summary>
		/// Strips protocol name from the resource name
		/// </summary>
		/// <param name="filePath">Name of the resource</param>
		/// <returns>Name of the resource without protocol name</returns>
		public static string GetFileSystemResourceWithoutProtocol(string filePath)
		{
			int pos = filePath.IndexOf(PROTOCOL_SEPARATOR);
			if (pos == -1)
			{
				return filePath;
			}
			else
			{
				// skip forward slashes after protocol name, if any
				if (filePath.Length > pos + PROTOCOL_SEPARATOR.Length)
				{
					while (filePath[++pos] == '/')
					{
						;
					}
				}
				return filePath.Substring(pos);
			}
		}

		/// <summary>
		/// Get config file
		/// </summary>
		/// <param name="resourcePath">
		/// A config resource path.
		/// </param>
		/// <returns>An XmlDocument representation of the config file</returns>
		public static XmlDocument GetConfigAsXmlDocument(string resourcePath)
		{
			XmlDocument config = new XmlDocument(); 
			XmlTextReader reader = null; 
			resourcePath = GetFileSystemResourceWithoutProtocol(resourcePath);
			
			if (!Resources.FileExists(resourcePath))
			{
				resourcePath = Path.Combine(_baseDirectory, resourcePath); 
			}

			try 
			{ 
				reader = new XmlTextReader( resourcePath ); 				
				config.Load(reader); 
			} 
			catch(Exception e) 
			{ 
				throw new ConfigurationException( 
					string.Format("Unable to load config file \"{0}\". Cause : {1}", 
					resourcePath, 
					e.Message ) ,e); 
			} 
			finally 
			{ 
				if (reader != null) 
				{ 
					reader.Close(); 
				} 
			} 
			return config; 

		}

		/// <summary>
		/// Determines whether the specified file exists.
		/// </summary>
		/// <param name="filePath">The file to check.</param>
		/// <returns>
		/// true if the caller has the required permissions and path contains the name of an existing file
		/// false if the caller has the required permissions and path doesn't contain the name of an existing file
		/// else exception
		/// </returns>
		public static bool FileExists(string filePath)
		{
			if (File.Exists(filePath) )
			{
				// true if the caller has the required permissions and path contains the name of an existing file; 
				return true;
			}
			else
			{
				// This method also returns false if the caller does not have sufficient permissions 
				// to read the specified file, 
				// no exception is thrown and the method returns false regardless of the existence of path.
				// So we check permissiion and throw an exception if no permission
				FileIOPermission filePermission = null;
				try
				{
					// filePath must be the absolute path of the file. 
					filePermission = new FileIOPermission(FileIOPermissionAccess.Read, filePath);
				}
				catch
				{
					return false;
				}
				try
				{
					filePermission.Demand();
				}
				catch(Exception e) 
				{ 
					throw new ConfigurationException( 
						string.Format("iBATIS doesn't have the right to read the config file \"{0}\". Cause : {1}", 
						filePath, 
						e.Message ) ,e); 
				} 

				return false;
			}
		}


		/// <summary>
		/// Load an XML resource from a location specify by the node.
		/// </summary>
		/// <param name="node">An location node</param>
		/// <param name="properties">the global properties</param>
		/// <returns>Return the Xml document load.</returns>
		public static XmlDocument GetAsXmlDocument(XmlNode node, NameValueCollection  properties)
		{
			XmlDocument xmlDocument = null;

			if (node.Attributes["resource"] != null)
			{
				string ressource = NodeUtils.ParsePropertyTokens( node.Attributes["resource"].Value, properties);
				xmlDocument = Resources.GetResourceAsXmlDocument( ressource );
			}
			else if (node.Attributes["url"] != null)
			{
				string url = NodeUtils.ParsePropertyTokens( node.Attributes["url"].Value, properties);
				xmlDocument = Resources.GetUrlAsXmlDocument( url );
			}
			else if (node.Attributes["embedded"] != null)
			{
				string embedded = NodeUtils.ParsePropertyTokens( node.Attributes["embedded"].Value, properties);
				xmlDocument = Resources.GetEmbeddedResourceAsXmlDocument(embedded);
			}

			return xmlDocument;
		}


		/// <summary>
		/// Get the path resource of an url or resource location.
		/// </summary>
		/// <param name="node">The specification from where to load.</param>
		/// <param name="properties">the global properties</param>
		/// <returns></returns>
		public static string GetValueOfNodeResourceUrl(XmlNode node, NameValueCollection  properties)
		{
			string path = null;

			if (node.Attributes["resource"] != null)
			{
				string ressource = NodeUtils.ParsePropertyTokens( node.Attributes["resource"].Value, properties);
				path = Path.Combine(_applicationBase, ressource);
			}
			else if (node.Attributes["url"] != null)
			{
				string url = NodeUtils.ParsePropertyTokens( node.Attributes["url"].Value, properties);
				path = url;
			}

			return path;
		}

		/// <summary>
		/// Get XmlDocument from a stream resource
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		public static XmlDocument GetStreamAsXmlDocument(Stream resource)
		{
			XmlDocument config = new XmlDocument();

			try 
			{
				config.Load(resource);
			}
			catch(Exception e)
			{
				throw new ConfigurationException(
					string.Format("Unable to load XmlDocument via stream. Cause : {0}", 
					e.Message ) ,e); 
			}

			return config;
		}

		/// <summary>
		/// Get XmlDocument from a FileInfo resource
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		public static XmlDocument GetFileInfoAsXmlDocument(FileInfo resource)
		{
			XmlDocument config = new XmlDocument();

			try 
			{
				config.Load( resource.FullName );
			}
			catch(Exception e)
			{
				throw new ConfigurationException(
					string.Format("Unable to load XmlDocument via FileInfo. Cause : {0}", 
					e.Message ) ,e); 
			}

			return config;
		}

		/// <summary>
		/// Get XmlDocument from a Uri resource
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		public static XmlDocument GetUriAsXmlDocument(Uri resource)
		{
			XmlDocument config = new XmlDocument();

			try 
			{
				config.Load( resource.AbsoluteUri );
			}
			catch(Exception e)
			{
				throw new ConfigurationException(
					string.Format("Unable to load XmlDocument via Uri. Cause : {0}", 
					e.Message ) ,e); 
			}

			return config;
		}

		/// <summary>
		/// Get XmlDocument from relative (from root directory of the application) path resource
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		public static XmlDocument GetResourceAsXmlDocument(string resource)
		{
			XmlDocument config = new XmlDocument();

			try 
			{
				config.Load( Path.Combine(_applicationBase, resource) );
			}
			catch(Exception e)
			{
				throw new ConfigurationException(
					string.Format("Unable to load file via resource \"{0}\" as resource. Cause : {1}", 
					resource, 
					e.Message ) ,e); 
			}

			return config;
		}


		/// <summary>
		/// Get XmlDocument from absolute path resource
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static XmlDocument GetUrlAsXmlDocument(string url)
		{
			XmlDocument config = new XmlDocument();

			try 
			{
				config.Load(url);
			}
			catch(Exception e)
			{
				throw new ConfigurationException(
					string.Format("Unable to load file via url \"{0}\" as url. Cause : {1}",
					url, 
					e.Message  ) ,e);
			}

			return config;
		}

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		public static XmlDocument GetEmbeddedResourceAsXmlDocument(string resource)
		{
			XmlDocument config = new XmlDocument();
			bool isLoad = false;

			FileAssemblyInfo fileInfo = new FileAssemblyInfo (resource);
			if (fileInfo.IsAssemblyQualified)
			{
				Assembly assembly = null;
#if dotnet2
                assembly = Assembly.Load(fileInfo.AssemblyName);
#else
                assembly = Assembly.LoadWithPartialName (fileInfo.AssemblyName);
#endif
                Stream stream = assembly.GetManifestResourceStream(fileInfo.ResourceFileName);
				// JIRA - IBATISNET-103 
				if (stream == null)
				{
					stream = assembly.GetManifestResourceStream(fileInfo.FileName);
				}
				if (stream != null)
				{
					try
					{
						config.Load(stream);
						isLoad = true;
					}
					catch(Exception e)
					{
						throw new ConfigurationException(
							string.Format("Unable to load file \"{0}\" in embedded resource. Cause : {1}",
							resource, 
							e.Message  ) ,e);
					}
				}
			} 
			else
			{
				// bare type name... loop thru all loaded assemblies
				Assembly [] assemblies = AppDomain.CurrentDomain.GetAssemblies ();
				foreach (Assembly assembly in assemblies)
				{
					Stream stream = assembly.GetManifestResourceStream(fileInfo.FileName);
					if (stream != null)
					{
						try
						{
							config.Load(stream);
							isLoad = true;
						}
						catch(Exception e)
						{
							throw new ConfigurationException(
								string.Format("Unable to load file \"{0}\" in embedded resource. Cause : ",
								resource, 
								e.Message  ) ,e);
						}
						break;
					}
				}
			}

			if (isLoad == false) 
			{
				_logger.Error("Could not load embedded resource from assembly");
				throw new ConfigurationException(
					string.Format("Unable to load embedded resource from assembly \"{0}\".",
					fileInfo.OriginalFileName));
			}

			return config;
		}


		/// <summary>
		/// Load a file from a given resource path
		/// </summary>
		/// <param name="resourcePath">
		/// The resource path
		/// </param>
		/// <returns>return a FileInfo</returns>
		public static FileInfo GetFileInfo(string resourcePath)
		{
			FileInfo fileInfo = null;
			resourcePath = GetFileSystemResourceWithoutProtocol(resourcePath);

			if ( !Resources.FileExists(resourcePath)) 
			{
				resourcePath = Path.Combine(_applicationBase, resourcePath);
			}

			try
			{
				//argument : The fully qualified name of the new file, or the relative file name. 
				fileInfo = new FileInfo(resourcePath);
			}
			catch(Exception e)
			{
				throw new ConfigurationException(
					string.Format("Unable to load file \"{0}\". Cause : \"{1}\"", resourcePath, e.Message),e);
			}
			return fileInfo;

		}


		/// <summary>
        /// Resolves the supplied type name into a <see cref="System.Type"/> instance.
		/// </summary>
        /// <param name="typeName">
        /// The (possibly partially assembly qualified) name of a <see cref="System.Type"/>.
        /// </param>
        /// <returns>
        /// A resolved <see cref="System.Type"/> instance.
        /// </returns>
        /// <exception cref="System.TypeLoadException">
        /// If the type cannot be resolved.
        /// </exception>
        [Obsolete("Use IBatisNet.Common.Utilities.TypeUtils")]
		public static Type TypeForName(string typeName)
		{
			return TypeUtils.ResolveType(typeName);
                //_cachedTypeResolver.Resolve(className);
		}

		#endregion

		#region Inner Class : FileAssemblyInfo
		/// <summary>
		/// Holds data about a <see cref="System.Type"/> and it's
		/// attendant <see cref="System.Reflection.Assembly"/>.
		/// </summary>
		internal class FileAssemblyInfo
		{
			#region Constants
			/// <summary>
			/// The string that separates file name
			/// from their attendant <see cref="System.Reflection.Assembly"/>
			/// names in an assembly qualified type name.
			/// </summary>
			public const string FileAssemblySeparator = ",";
			#endregion

			#region Fields
			private string _unresolvedAssemblyName= string.Empty;
			private string _unresolvedFileName= string.Empty;
			private string _originalFileName= string.Empty;
			#endregion

			#region Properties

			/// <summary>
			/// The resource file name .
			/// </summary>
			public string ResourceFileName
			{
				get { return AssemblyName+"."+FileName; }
			}

			/// <summary>
			/// The original name.
			/// </summary>
			public string OriginalFileName
			{
				get { return _originalFileName; }
			}

			/// <summary>
			/// The file name portion.
			/// </summary>
			public string FileName
			{
				get { return _unresolvedFileName; }
			}

			/// <summary>
			/// The (unresolved, possibly partial) name of the attandant assembly.
			/// </summary>
			public string AssemblyName
			{
				get { return _unresolvedAssemblyName; }
			}

			/// <summary>
			/// Is the type name being resolved assembly qualified?
			/// </summary>
			public bool IsAssemblyQualified
			{
				get
				{
					if (AssemblyName ==  null || AssemblyName.Trim().Length==0)
					{
						return false;
					}
					else
					{
						return true;
					}
				}
			}

			#endregion

			#region Constructor (s) / Destructor
			/// <summary>
			/// Creates a new instance of the FileAssemblyInfo class.
			/// </summary>
			/// <param name="unresolvedFileName">
			/// The unresolved name of a <see cref="System.Type"/>.
			/// </param>
			public FileAssemblyInfo (string unresolvedFileName)
			{
				SplitFileAndAssemblyNames (unresolvedFileName);
			}
			#endregion

			#region Methods
			/// <summary>
			/// 
			/// </summary>
			/// <param name="originalFileName"></param>
			private void SplitFileAndAssemblyNames (string originalFileName) 
			{
				_originalFileName = originalFileName;

				int separatorIndex = originalFileName.IndexOf(FileAssemblyInfo.FileAssemblySeparator);
				
				if (separatorIndex < 0)
				{
					_unresolvedFileName = originalFileName.Trim();
					_unresolvedAssemblyName = null; // IsAssemblyQualified will return false
				} 
				else
				{
					_unresolvedFileName = originalFileName.Substring(0, separatorIndex).Trim();
					_unresolvedAssemblyName = originalFileName.Substring(separatorIndex + 1).Trim();
				}
			}
			#endregion

		}
		#endregion

	}
}
