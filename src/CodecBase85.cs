using System;

namespace TuckBytesInCode
{
	public sealed class CodecBase85 : ICharLookup
	{
		public char Map(int index)
		{
			if (index >= AllChars.Length) {
				throw new IndexOutOfRangeException();
			}
			return AllChars[index];
		}

		public int @Base {
			get { return AllChars.Length; }
		}

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
		static string AllChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!#$%&()*+-;<=>?@^_`{|}~";
	}
}