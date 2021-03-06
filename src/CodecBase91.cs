using System;

namespace TuckBytesInCode
{
	public sealed class CodecBase91 : ICharLookup, IBaseInfo
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
		public int BytesIn { get { return 13; }}
		public int CharsOut { get { return 16; }}
		public bool TreatAsBinary { get { return false; }}

		private CodecBase91() {}

		static CodecBase91 _self = null;
		public static CodecBase91 Self {
			get {
				if (_self == null) {
					_self = new CodecBase91();
				}
				return _self;
			}
		}

		public Base Identifier { get { return Base.Base91; }}
		public string DisplayName { get { return "Base 91"; }}
		public string Description { get { return "text-encoded base 91"; }}
		public ICharLookup BaseInstance { get { return Self; }}


		// http://base91.sourceforge.net/
		static string AllChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!#$%&()*+,./:;<=>?@[]^_`{|}~\"";
	}
}