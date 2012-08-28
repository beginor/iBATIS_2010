using System;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.TypeHandlers;

namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// OneZeroBoolTypeHandlerCallback.
	/// </summary>
	public class OneZeroBoolTypeHandlerCallback : ITypeHandlerCallback
	{
		private const int TRUE = 1;
		private const int FALSE = 0;

		#region ITypeHandlerCallback members

		public object ValueOf(string nullValue)
		{
			if (TRUE.ToString().Equals(nullValue)) 
			{
				return true;
			} 
			else if (FALSE.ToString().Equals(nullValue)) 
			{
				return false;
			} 
			else 
			{
				throw new DataMapperException("Unexpected value " + nullValue + " found where "+TRUE+" or "+FALSE+" was expected.");
			}		
		}

		public object GetResult(IResultGetter getter)
		{
			int i = int.Parse(getter.Value.ToString());
			if (TRUE.Equals(i)) 
			{
				return true;
			}
			else if (FALSE.Equals(i)) 
			{
				return false;
			} 
			else 
			{
 				 throw new DataMapperException("Unexpected value " + i + " found where "+TRUE+" or "+FALSE+" was expected.");
			}
		}

		public void SetParameter(IParameterSetter setter, object parameter)
		{
			bool b = Convert.ToBoolean(parameter);
			if (b) 
			{
				setter.Value = TRUE;
			} 
			else 
			{
				setter.Value = FALSE;
			}			
		}

        public object NullValue
        {
            get { throw new InvalidCastException("OneZeroBoolTypeHandlerCallback could not cast a null value in a bool field."); }
        }
		#endregion
	}
}
