using System;

namespace TuckBytesInCode
{
	public sealed class CodecBase85 : ICharLookup
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
		public int BytesIn { get { return 4; }}
		public int BytesOut { get { return 5; }}

		private CodecBase85() {}

		static CodecBase85 _self = null;
		public static CodecBase85 Self {
			get {
				if (_self == null) {
					_self = new CodecBase85();
				}
				return _self;
			}
		}

		// https://tools.ietf.org/html/rfc1924
		//static string AllChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!#$%&()*+-;<=>?@^_`{|}~";
		static string AllChars = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstu";
	}
}