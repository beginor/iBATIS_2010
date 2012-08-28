using System;
using System.Xml;
using System.Collections.Specialized;

namespace IBatisNet.Common.Xml
{
	/// <summary>
	/// Summary description for NodeUtils.
	/// </summary>
	public sealed class NodeUtils
	{

		/// <summary>
		/// Searches for the attribute with the specified name in this attributes list.
		/// </summary>
		/// <param name="attributes"></param>
		/// <param name="name">The key</param>
		/// <returns></returns>
		public static string GetStringAttribute(NameValueCollection attributes, string name) 
		{
			string value = attributes[name];
			if (value == null) 
			{
				return string.Empty;
			} 
			else 
			{
				return value;
			}
		}

		/// <summary>
		/// Searches for the attribute with the specified name in this attributes list.
		/// </summary>
		/// <param name="attributes"></param>
		/// <param name="name">The key</param>
		/// <param name="def">The default value to be returned if the attribute is not found.</param>
		/// <returns></returns>
		public static string GetStringAttribute(NameValueCollection attributes, string name, string def) 
		{
			string value = attributes[name];
			if (value == null) 
			{
				return def;
			} 
			else 
			{
				return value;
			}
		}
		/// <summary>
		/// Searches for the attribute with the specified name in this attributes list.
		/// </summary>
		/// <param name="attributes"></param>
		/// <param name="name">The key</param>
		/// <param name="def">The default value to be returned if the attribute is not found.</param>
		/// <returns></returns>
		public static byte GetByteAttribute(NameValueCollection attributes, string name, byte def) 
		{
			string value = attributes[name];
			if (value == null) 
			{
				return def;
			} 
			else 
			{
				return XmlConvert.ToByte(value);
			}
		}

		/// <summary>
		/// Searches for the attribute with the specified name in this attributes list.
		/// </summary>
		/// <param name="attributes"></param>
		/// <param name="name">The key</param>
		/// <param name="def">The default value to be returned if the attribute is not found.</param>
		/// <returns></returns>
		public static int GetIntAttribute(NameValueCollection attributes, string name, int def) 
		{
			string value = attributes[name];
			if (value == null) 
			{
				return def;
			} 
			else 
			{
				return XmlConvert.ToInt32(value);
			}
		}

		/// <summary>
		/// Searches for the attribute with the specified name in this attributes list.
		/// </summary>
		/// <param name="attributes"></param>
		/// <param name="name">The key</param>
		/// <param name="def">The default value to be returned if the attribute is not found.</param>
		/// <returns></returns>
		public static bool GetBooleanAttribute(NameValueCollection attributes, string name, bool def) 
		{
			string value = attributes[name];
			if (value == null) 
			{
				return def;
			} 
			else 
			{
				return XmlConvert.ToBoolean(value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static NameValueCollection ParseAttributes(XmlNode node) 
		{
			return ParseAttributes(node, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <param name="variables"></param>
		/// <returns></returns>
		public static NameValueCollection ParseAttributes(XmlNode node, NameValueCollection variables) 
		{
			NameValueCollection attributes = new NameValueCollection();
			int count = node.Attributes.Count;
			for (int i = 0; i < count; i++) 
			{
				XmlAttribute attribute = node.Attributes[i];
				String value = ParsePropertyTokens(attribute.Value, variables);
				attributes.Add(attribute.Name, value);
			}
			return attributes;
		}


		/// <summary>
		/// Replace properties by their values in the given string
		/// </summary>
		/// <param name="str"></param>
		/// <param name="properties"></param>
		/// <returns></returns>
		public static string ParsePropertyTokens(string str, NameValueCollection  properties) 
		{
			string OPEN = "${";
			string CLOSE = "}";

			string newString = str;
			if (newString != null && properties != null) 
			{
				int start = newString.IndexOf(OPEN);
				int end = newString.IndexOf(CLOSE);

				while (start > -1 && end > start) 
				{
					string prepend = newString.Substring(0, start);
					string append = newString.Substring(end + CLOSE.Length);

					int index = start + OPEN.Length;
					string propName = newString.Substring(index, end-index);
					string propValue = properties.Get(propName);
					if (propValue == null) 
					{
						newString = prepend + propName + append;
					} 
					else 
					{
						newString = prepend + propValue + append;
					}
					start = newString.IndexOf(OPEN);
					end = newString.IndexOf(CLOSE);
				}
			}
			return newString;
		}

	}
}
