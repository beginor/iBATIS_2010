using System;

using IBatisNet.DataMapper.TypeHandlers;

namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// GuidStringTypeHandlerCallback.
	/// </summary>
	public class GuidStringTypeHandlerCallback : ITypeHandlerCallback
	{
		private const string GUIDNULL = "00000000-0000-0000-0000-000000000000";

		#region ITypeHandlerCallback members

		public object ValueOf(string nullValue)
		{
			if (GUIDNULL.Equals(nullValue)) 
			{
				return Guid.Empty;
			} 
			else 
			{
				throw new Exception("Unexpected value " + nullValue + " found where "+GUIDNULL+" was expected to represent a null value.");
			}		
		}

		public object GetResult(IResultGetter getter)
		{
			try {
				Guid result = new Guid(getter.Value.ToString());
				return result;
			} 
			catch
			{
 				 throw new Exception("Unexpected value " + getter.Value.ToString() + " found where a valid GUID string value was expected.");
			}
		}

		public void SetParameter(IParameterSetter setter, object parameter)
		{
			setter.Value = parameter.ToString();
		}

        public object NullValue
        {
            get { throw new InvalidCastException("GuidStringTypeHandlerCallback could not cast a null value in a guid field."); }
        }
		#endregion
	}
}
