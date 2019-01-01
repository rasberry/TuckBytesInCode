using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TuckBytesInCode
{
	public class BaseReader : IEnumerable<char>
	{
		public BaseReader(IEnumerable<char> src, ICharLookup map)
		{
			Source = src;
			Map = map;
		}

		IEnumerable<char> Source;
		ICharLookup Map;

		public IEnumerator<char> GetEnumerator()
		{
			return null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}