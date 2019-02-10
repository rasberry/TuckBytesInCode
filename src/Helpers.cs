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
			return (int)Math.Ceiling(Math.Pow(2.0, 8.0 * map.BytesIn / map.CharsOut));
		}

		public static bool StartsWithIC(this string subj, string test)
		{
			if (subj == null || test == null) {
				return false;
			}
			return 0 == subj.IndexOf(test,StringComparison.OrdinalIgnoreCase);
		}

		public static bool ContainsIC(this string subj, string test)
		{
			if (subj == null || test == null) {
				return false;
			}
			return -1 != subj.IndexOf(test,StringComparison.OrdinalIgnoreCase);
		}

		public static bool EqualsIC(this string subj, string test)
		{
			if (subj == null || test == null) {
				return false;
			}
			return subj.Equals(test,StringComparison.OrdinalIgnoreCase);
		}
	}
}