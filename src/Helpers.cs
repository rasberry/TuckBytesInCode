using System;
using System.Collections.Generic;
using System.Linq;

namespace TuckBytesInCode
{
	public static class Helpers
	{
		public static int Base(this ICharLookup map)
		{
			return (int)Math.Ceiling(Math.Pow(2.0, 8.0 * map.BytesIn / map.BytesOut));
		}

		public static int FillArray<T>(this IEnumerator<T> src, ref T[] array)
		{
			int len = array.Length;
			int count = 0;
			while(count < len && src.MoveNext())
			{
				array[count] = src.Current;
				count++;
			}
			return count;
		}

		public static IEnumerable<char> ReorderBase(this IEnumerable<char> src, int chunkSize)
		{
			var etor = src.GetEnumerator();
			char[] buffer = new char[chunkSize];
			bool done = false;

			while(!done)
			{
				int start = 0;
				int count = 0;
				while(!done && count < chunkSize)
				{
					if (etor.MoveNext()) {
						int rindex = chunkSize - count - 1;
						buffer[rindex] = etor.Current;
					}
					else {
						start = chunkSize - count;
						done = true;
					}
					count++;
				}
				for(int c = start; c < chunkSize; c++) {
					yield return buffer[c];
				}
			}
		}
	}
}