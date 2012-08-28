using System;

namespace IBatisNet.DataMapper.Test.Domain
{

    public enum ESearchProfile
    {
        Temp = 'T',
        Permanent = 'P'
    }

	public enum Days 
	{
		Sat=1, 
		Sun, 
		Mon, 
		Tue, 
		Wed, 
		Thu, 
		Fri
	};

	[FlagsAttribute]
	public enum Colors 
	{ 
		Red = 1, 
		Green = 2, 
		Blue = 4, 
		Yellow = 8 
	};

	[Flags()]
	public enum Months : long
	{
		January  = 1,
		February =2,
		March =4,
		April =8,
		May =16,
		June =32,
		July =64,
		August =128,
		September =256,
		October =512,
		November =1024,
		December =2048,
		All = January|February|March|April|May|June|July|August|September|October|November|December
	}

	/// <summary>
	/// Summary description for EnumTest.
	/// </summary>
	public class Enumeration
	{
		private Days _day;
		private Months _month;
		private Colors _color;
		private int _id;
        private ESearchProfile search = ESearchProfile.Temp;

        public ESearchProfile SearchProfile
        {
            get { return search; }
            set { search = value; }
        }

		public Months Month
		{
			get
			{
				return _month; 
			}
			set
			{ 
				_month = value; 
			}
		}

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

		public Days Day
		{
			get
			{
				return _day; 
			}
			set
			{ 
				_day = value; 
			}
		}

		public Colors Color
		{
			get
			{
				return _color; 
			}
			set
			{ 
				_color = value; 
			}
		}
	}

}
