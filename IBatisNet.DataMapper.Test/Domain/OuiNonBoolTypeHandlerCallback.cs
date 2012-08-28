using System;

using IBatisNet.DataMapper.TypeHandlers;

namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// OuiNonBoolTypeHandlerCallback.
	/// </summary>
	/// <remarks>
	/// Used in account.xml on 
	///  &lt; result property="BannerOption" type="bool" dbType="Varchar" column="Account_Banner_Option" /&gt;
	/// </remarks>
	public class OuiNonBoolTypeHandlerCallback : ITypeHandlerCallback
	{
		private const string YES = "Oui";
		private const string NO = "Non";

		#region ITypeHandlerCallback members

		public object ValueOf(string nullValue)
		{
			if (YES.Equals(nullValue)) 
			{
				return true;
			} 
			else if (NO.Equals(nullValue)) 
			{
				return false;
			} 
			else 
			{
				throw new Exception("Unexpected value " + nullValue + " found where "+YES+" or "+NO+" was expected.");
			}		
		}

		public object GetResult(IResultGetter getter)
		{
			string s = getter.Value as string;
			if (YES.Equals(s)) 
			{
				return true;
			}
			else if (NO.Equals(s)) 
			{
				return false;
			} 
			else 
			{
				throw new Exception("Unexpected value " + s + " found where "+YES+" or "+NO+" was expected.");
			}
		}

		public void SetParameter(IParameterSetter setter, object parameter)
		{
			bool b = Convert.ToBoolean(parameter);
			if (b) 
			{
				setter.Value = YES;
			} 
			else 
			{
				setter.Value = NO;
			}			
		}

        public object NullValue
        {
            get { throw new InvalidCastException("OuiNonBoolTypeHandlerCallback could not cast a null value in the field."); }
        }
		#endregion
	}
}
