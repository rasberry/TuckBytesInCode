using System;
using System.Collections.Generic;
using System.IO;

namespace TuckBytesInCode
{
	public static class TuckBytes
	{
		public static IEnumerable<char> Encode(Stream s, ICharLookup map)
		{
			int baseBitLength = (int)Math.Floor(Math.Log((double)map.Base,2.0));
			var reader = new BitReader(s);

			while(true)
			{
				int bits = reader.ReadBitsAsInt(baseBitLength);
				if (bits == -1) {
					break;
				} else {
					yield return map.Map(bits);
				}
			}
		}
	}
}
