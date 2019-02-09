using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TuckBytesInCode
{
	public static class Helpers
	{
		public static int Base(this ICharLookup map)
		{
			return (int)Math.Ceiling(Math.Pow(2.0, 8.0 * map.BytesIn / map.BytesOut));
		}
	}
}