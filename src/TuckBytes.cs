using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;

namespace TuckBytesInCode
{
	public static class TuckBytes
	{
		//public static IEnumerable<char> Encode(Stream s, ICharLookup map)
		//{
		//	//int baseBitLength = (int)Math.Floor(Math.Log((double)map.Base,2.0));
		//	int baseBitLength = 1;
		//	var reader = new BitReader(s);
		//
		//	while(true)
		//	{
		//		int bits = reader.ReadBitsAsInt(baseBitLength);
		//		if (bits == -1) {
		//			break;
		//		} else {
		//			yield return map.Map(bits);
		//		}
		//	}
		//}

		public static IEnumerable<char> Encode(Stream s, ICharLookup map)
		{
			int bytesIn = map.BytesIn;
			int bytesOut = map.BytesOut;
			int @base = map.Base();
			byte[] bufferIn = new byte[bytesIn];
			int[] bufferOut = new int[bytesOut];
			bool printPadding = map.IncludePadding;
			long streamLength = 0;
			int paddingCount = 0;

			while(true)
			{
				Array.Clear(bufferIn,0,bufferIn.Length);
				Array.Clear(bufferOut,0,bufferOut.Length);
				int read = s.Read(bufferIn,0,bytesIn);
				if (read < 1) { break; }
				streamLength += read;

				Array.Reverse(bufferIn); //bit to little endian
				BigInteger bi = new BigInteger(bufferIn);

				//..and return the chars backwards also
				int count = bufferOut.Length;
				while(bi > 0) {
					//core of the conversion is just finding successive remainders
					bi = BigInteger.DivRem(bi,@base,out BigInteger rem);
					bufferOut[--count] = (int)rem;
				}

				//deal with the padding
				if (read < bytesIn) {
					long outLen = (long)Math.Ceiling(
						streamLength * (double)bytesOut/(double)bytesIn
					);
					long padLen = LongCeil(outLen,bytesOut) * bytesOut;
					paddingCount = (int)(padLen - outLen);
				}

				// actuall encoding step
				for(int i = 0; i < bytesOut - paddingCount; i++) {
					int val = bufferOut[i];
					char c = map.Map(val);
					yield return c;
				}

				// return padding if necessary
				if (printPadding && paddingCount > 0) {
					for(int p = 0; p < paddingCount; p++) {
						yield return map.Padding;
					}
				}

				if (read < bytesIn) { break; }
			}
		}

		public static IEnumerable<char> Decode(Stream s, ICharLookup map)
		{
			int bytesIn = map.BytesIn;
			int bytesOut = map.BytesOut;
			int @base = map.Base();
			byte[] bufferIn = new byte[bytesIn];
			int[] bufferOut = new int[bytesOut];
			//bool printPadding = map.IncludePadding;
			//long streamLength = 0;
			//int paddingCount = 0;

			return null;
		}

		static long LongCeil(long num, long den)
		{
			return num / den + (num % den == 0 ? 0 : 1);
		}
	}
}
