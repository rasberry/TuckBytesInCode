using System;

namespace TuckBytesInCode
{
	public sealed class CodecBase16 : ICharLookup, IBaseInfo
	{
		public char Map(int index)
		{
			if (index < 0 || index >= AllChars.Length) {
				throw new IndexOutOfRangeException();
			}
			return AllChars[index];
		}

		public char Padding { get { return '\0'; }}
		public bool IncludePadding { get { return false; }}
		public int BytesIn { get { return 1; }}
		public int CharsOut { get { return 2; }}
		public bool TreatAsBinary { get { return false; }}

		private CodecBase16() {}

		static CodecBase16 _self = null;
		public static CodecBase16 Self {
			get {
				if (_self == null) {
					_self = new CodecBase16();
				}
				return _self;
			}
		}

		public Base Identifier { get { return Base.Base16; }}
		public string DisplayName { get { return "Base 16"; }}
		public string Description { get { return "text-encoded base 16 - characters 0-9,A-F"; }}
		public ICharLookup BaseInstance { get { return Self; }}


		static string AllChars = "0123456789ABCDEF";
	}
}