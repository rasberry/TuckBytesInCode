using System;

namespace TuckBytesInCode
{
	public sealed class CodecBase64 : ICharLookup, IBaseInfo
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
		public int CharsOut { get { return 4; }}
		public bool TreatAsBinary { get { return false; }}

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

		public Base Identifier { get { return Base.Base64; }}
		public string DisplayName { get { return "Base 64"; }}
		public string Description { get { return "text-encoded base 64"; }}
		public ICharLookup BaseInstance { get { return Self; }}


		static string AllChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
	}
}