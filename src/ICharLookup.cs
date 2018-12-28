using System;

namespace TuckBytesInCode
{
	// number of chars = 2^(8*in/out)
	public interface ICharLookup
	{
		char Map(int index);

		int BytesIn { get; }
		int BytesOut { get; }
		char Padding { get; }
		bool IncludePadding { get; }
	}
}
