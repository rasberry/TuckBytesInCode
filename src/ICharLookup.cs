using System;

namespace TuckBytesInCode
{
	// number of chars = 2^(8*in/out)
	public interface ICharLookup
	{
		char Map(int index);
		int BytesIn { get; }
		int CharsOut { get; }
		char Padding { get; }
		bool IncludePadding { get; }
		bool TreatAsBinary { get; }
	}
}
