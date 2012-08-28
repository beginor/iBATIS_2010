using System;
#if dotnet2
using System.Collections.Generic;
#endif


namespace IBatisNet.DataMapper.Test.Domain
{
    public class NullableClass
    {

		public NullableClass()
		{
		}

#if dotnet2
        private int _id = int.MinValue;
        private bool? _testBool = null;
        private byte? _testByte = null;
        private char? _testChar = null;
        private DateTime? _testDateTime = null;
        private decimal? _testDecimal = null;
        private double? _testDouble = null;
        private Guid? _testGuid = null;
        private Int16? _testInt16 = null;
        private Int32? _testInt32 = null;
        private Int64? _testInt64 = null;
        private Single? _testSingle = null;
        private TimeSpan? _testTimeSpan = null;

        public NullableClass(Int32? id)
        {
            _testInt32 = id;
        }

        public TimeSpan? TestTimeSpan
        {
            get { return _testTimeSpan; }
            set { _testTimeSpan = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public bool? TestBool
        {
            get { return _testBool; }
            set { _testBool = value; }
        }

        public byte? TestByte
        {
            get { return _testByte; }
            set { _testByte = value; }
        }

        public char? TestChar
        {
            get { return _testChar; }
            set { _testChar = value; }
        }

        public DateTime? TestDateTime
        {
            get { return _testDateTime; }
            set { _testDateTime = value; }
        }

        public decimal? TestDecimal
        {
            get { return _testDecimal; }
            set { _testDecimal = value; }
        }

        public double? TestDouble
        {
            get { return _testDouble; }
            set { _testDouble = value; }
        }

        public Guid? TestGuid
        {
            get { return _testGuid; }
            set { _testGuid = value; }
        }

        public Int16? TestInt16
        {
            get { return _testInt16; }
            set { _testInt16 = value; }
        }

        public Int32? TestInt32
        {
            get { return _testInt32; }
            set { _testInt32 = value; }
        }

        public Int64? TestInt64
        {
            get { return _testInt64; }
            set { _testInt64 = value; }
        }


        public Single? TestSingle
        {
            get { return _testSingle; }
            set { _testSingle = value; }
        }  
#else
        private int _id = int.MinValue;
        private bool _testBool = false;
        private byte _testByte = 1;
        private char _testChar = 'a';
        private DateTime _testDateTime = DateTime.MinValue;
        private decimal _testDecimal = 0;
        private double _testDouble = 0;
        private Guid _testGuid = Guid.NewGuid();
        private Int16 _testInt16 = 0;
        private Int32 _testInt32 = 0;
        private Int64 _testInt64 = 0;
        private Single _testSingle = 0;
		private TimeSpan _testTimeSpan;

		public NullableClass(int id)
		{
			_id = id;
		}

		public TimeSpan TestTimeSpan
		{
			get { return _testTimeSpan; }
			set { _testTimeSpan = value; }
		}
		
    	
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public bool TestBool
        {
            get { return _testBool; }
            set { _testBool = value; }
        }

        public byte TestByte
        {
            get { return _testByte; }
            set { _testByte = value; }
        }

        public char TestChar
        {
            get { return _testChar; }
            set { _testChar = value; }
        }

        public DateTime TestDateTime
        {
            get { return _testDateTime; }
            set { _testDateTime = value; }
        }

        public decimal TestDecimal
        {
            get { return _testDecimal; }
            set { _testDecimal = value; }
        }

        public double TestDouble
        {
            get { return _testDouble; }
            set { _testDouble = value; }
        }

        public Guid TestGuid
        {
            get { return _testGuid; }
            set { _testGuid = value; }
        }

        public Int16 TestInt16
        {
            get { return _testInt16; }
            set { _testInt16 = value; }
        }

        public Int32 TestInt32
        {
            get { return _testInt32; }
            set { _testInt32 = value; }
        }

        public Int64 TestInt64
        {
            get { return _testInt64; }
            set { _testInt64 = value; }
        }


        public Single TestSingle
        {
            get { return _testSingle; }
            set { _testSingle = value; }
        }  
#endif

    }
}
