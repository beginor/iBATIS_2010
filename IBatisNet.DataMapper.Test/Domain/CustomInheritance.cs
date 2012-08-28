using System;

using IBatisNet.DataMapper.TypeHandlers;

namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// Description résumée de CustomInheritance.
	/// </summary>
	public class CustomInheritance : ITypeHandlerCallback
	{

		#region ITypeHandlerCallback members

		public object ValueOf(string nullValue)
		{
			throw new NotImplementedException();
		}

		public object GetResult(IResultGetter getter)
		{
			string type = getter.Value.ToString();

			if (type=="Monograph" || type=="Book")
			{
				return "Book";
			}
			else if (type=="Tabloid" || type=="Broadsheet" || type=="Newspaper")
			{

				return "Newspaper";
			}
			else
			{
				return "Document";
			}

		}

		public void SetParameter(IParameterSetter setter, object parameter)
		{
			throw new NotImplementedException(); 
		}

        public object NullValue
        {
            get { throw new InvalidCastException("CustomInheritance TypeHandlerCallback could not cast a null value in the field."); }
        }
		#endregion
	}
}
