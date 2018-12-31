using System;

namespace TuckBytesInCode
{
	public sealed class CodecBase64 : ICharLookup
	{
		public char Map(int index)
		{
			if (index < 0 || index >= AllChars.Length) {
				throw new IndexOutOfRangeException();
			}
			return AllChars[index];
		}

		public char Padding { get { return '='; }}
		public bool IncludePadding { get { return true; }}
		public int BytesIn { get { return 3; }}
		public int BytesOut { get { return 4; }}

		private CodecBase64() {}

		static CodecBase64 _self = null;
		public static CodecBase64 Self {
			get {
				if (_self == null) {
					_self = new CodecBase64();
				}
				return _self;
			}
		}

		static string AllChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
	}
}