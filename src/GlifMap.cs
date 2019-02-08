using System;
using System.Collections.Generic;

namespace TuckBytesInCode
{
	public class GlifMap
	{
		public GlifMap(ICharLookup map)
		{
			Init(map);
		}

		void Init(ICharLookup map)
		{
			int @base = map.Base();
			lookup = new Dictionary<char, int>(@base);
			for(int b=0; b<@base; b++) {
				lookup.Add(map.Map(b),b);
			}
			if (map.IncludePadding) {
				Padding = map.Padding;
			}
		}

		public int Map(char glif)
		{
			if (Padding.HasValue && glif == Padding.Value) {
				return 0;
			}
			if (!lookup.TryGetValue(glif,out int index)) {
				throw new KeyNotFoundException("Failed to reverse map "+glif);
			}
			return index;
		}

		Dictionary<char,int> lookup;
		char? Padding = null;
	}
}
