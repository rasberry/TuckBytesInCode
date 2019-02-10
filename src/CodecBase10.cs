using System;

namespace TuckBytesInCode
{
	public sealed class CodecBase10 : ICharLookup, IBaseInfo
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
		public int BytesIn { get { return 2; }}
		public int CharsOut { get { return 5; }}
		public bool TreatAsBinary { get { return false; }}

		private CodecBase10() {}

		static CodecBase10 _self = null;
		public static CodecBase10 Self {
			get {
				if (_self == null) {
					_self = new CodecBase10();
				}
				return _self;
			}
		}

		public Base Identifier { get { return Base.Base10; }}
		public string DisplayName { get { return "Base 10"; }}
		public string Description { get { return "text-encoded base 10"; }}
		public ICharLookup BaseInstance { get { return Self; }}

		static string AllChars = "0123456789";
	}
}