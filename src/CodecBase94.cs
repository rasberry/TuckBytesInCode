using System;

namespace TuckBytesInCode
{
	public sealed class CodecBase94 : ICharLookup, IBaseInfo
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
		public int BytesIn { get { return 9; }}
		public int CharsOut { get { return 11; }}
		public bool TreatAsBinary { get { return false; }}

		private CodecBase94() {}

		static CodecBase94 _self = null;
		public static CodecBase94 Self {
			get {
				if (_self == null) {
					_self = new CodecBase94();
				}
				return _self;
			}
		}

		public Base Identifier { get { return Base.Base94; }}
		public string DisplayName { get { return "Base 94"; }}
		public string Description { get { return "text-encoded base 94 - all printable ASCII characters"; }}
		public ICharLookup BaseInstance { get { return Self; }}


		// http://www.asciichars.com/ascii-table-printable-characters
		static string AllChars = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
	}
}