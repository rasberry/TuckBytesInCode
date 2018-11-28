using System;

namespace TuckBytesInCode
{
	public interface ICharLookup
	{
		char Map(int index);
		int @Base { get; }
	}
}