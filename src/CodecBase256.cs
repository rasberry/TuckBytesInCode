using System;

namespace TuckBytesInCode
{
	public sealed class CodecBase256 : ICharLookup, IBaseInfo
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
		public int CharsOut { get { return 1; }}
		public bool TreatAsBinary { get { return true; }}

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

		public Base Identifier { get { return Base.Base256; }}
		public string DisplayName { get { return "Base 256"; }}
		public string Description { get { return "also known as raw bytes"; }}
		public ICharLookup BaseInstance { get { return Self; }}

	}
}