using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// Description résumée de LineItem.
	/// </summary>
	[Serializable]
	public class LineItem
	{
		private int _id;
		private Order _order;
		private string _code;
		private int _quantity;
		private decimal _price = decimal.MinValue;
		private byte[] _pictureData = null;

		public int Id
		{
			get
			{
				return _id; 
			}
			set
			{ 
				_id = value; 
			}
		}

		public Order Order
		{
			get
			{
				return _order; 
			}
			set
			{ 
				_order = value; 
			}
		}

		public string Code
		{
			get
			{
				return _code; 
			}
			set
			{ 
				_code = value; 
			}
		}

		public int Quantity
		{
			get
			{
				return _quantity; 
			}
			set
			{ 
				_quantity = value; 
			}
		}

		public decimal Price
		{
			get
			{
				return _price; 
			}
			set
			{ 
				_price = value; 
			}
		}
		public byte[] PictureData 
		{
			get{ return _pictureData; }
			set{ _pictureData = value; }
		}
		public Image Picture 
		{
			set
			{
                if (value!=null)
                {
				    _pictureData = LineItem.ConvertToByteArray( value ); 
                }
                else
                {
                    _pictureData = null;
                }
			}
			get
			{ 
				return LineItem.ConvertToImage( _pictureData ); 
			}
		}

		#region Image converters
		static protected byte[] ConvertToByteArray( Image picture ) 
		{
			MemoryStream memoryStream = new MemoryStream();
			picture.Save( memoryStream, ImageFormat.Jpeg );
			return memoryStream.ToArray ();
		}
		static protected Image ConvertToImage( byte[] pictureData ) 
		{
			if (pictureData != null)
			{
				return Image.FromStream( new MemoryStream( pictureData ) );
			}
			else
			{
				return null;
			}
		}
		#endregion
	}
}
