using System;

namespace TuckBytesInCode
{
	public interface IBaseInfo
	{
		Base Identifier { get; }
		string DisplayName { get; }
		string Description { get; }
		ICharLookup BaseInstance { get; }
	}
}
