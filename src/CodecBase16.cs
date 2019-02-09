using System;

namespace TuckBytesInCode
{
	public sealed class CodecBase16 : ICharLookup
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
		public int BytesOut { get { return 2; }}

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

		static string AllChars = "0123456789ABCDEF";
	}
}