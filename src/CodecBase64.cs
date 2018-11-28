using System;

namespace TuckBytesInCode
{
	public sealed class CodecBase64 : ICharLookup
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