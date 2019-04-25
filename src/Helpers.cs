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

		public static bool FindBaseRatio(int @base, out int bytesIn, out int charsOut)
		{
			bytesIn = 0;
			charsOut = 0;

			double ratio = Math.Log(@base,2.0) / 8.0;
			const double searchMax = 16383.0;

			for(double den = 1; den <= searchMax; den++)
			{
				double num = Math.Floor(den * ratio);
				double testBase = Math.Ceiling(Math.Pow(2.0,8.0 * num / den));
				// Log.Debug("num = "+num+" den = "+den+" tb = "+testBase);
				if ((int)testBase == @base) {
					bytesIn = (int)num;
					charsOut = (int)den;
					return true;
				}
			}
			return false;
		}

		public static long LongCeil(long num, long den)
		{
			return num / den + (num % den == 0 ? 0 : 1);
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

		public static string PadTo(this string subj, int len, char padChar = ' ')
		{
			if (subj == null || subj.Length >= len) {
				return subj;
			}
			return subj + new String(padChar,len - subj.Length);
		}
	}
}