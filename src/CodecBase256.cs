using System;

namespace TuckBytesInCode
{
	public sealed class CodecBase256 : ICharLookup
	{
		public char Map(int index)
		{
			if (index < 0 || index >= 256) {
				throw new IndexOutOfRangeException();
			}
			//byte to char maps directly
			return (char)index;
		}

		public char Padding { get { return '\0'; }}
		public bool IncludePadding { get { return false; }}
		public int BytesIn { get { return 1; }}
		public int BytesOut { get { return 1; }}

		private CodecBase256() {}

		static CodecBase256 _self = null;
		public static CodecBase256 Self {
			get {
				if (_self == null) {
					_self = new CodecBase256();
				}
				return _self;
			}
		}
	}
}