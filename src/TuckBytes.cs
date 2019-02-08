using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Linq;

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

		//public static IEnumerable<char> Encode_1(Stream s, ICharLookup map)
		//{
		//	int bytesIn = map.BytesIn;
		//	int bytesOut = map.BytesOut;
		//	int @base = map.Base();
		//	byte[] bufferIn = new byte[bytesIn];
		//	int[] bufferOut = new int[bytesOut];
		//	bool printPadding = map.IncludePadding;
		//	long streamLength = 0;
		//	int paddingCount = 0;
		//
		//	while(true)
		//	{
		//		Array.Clear(bufferIn,0,bufferIn.Length);
		//		Array.Clear(bufferOut,0,bufferOut.Length);
		//		int read = s.Read(bufferIn,0,bytesIn);
		//		if (read < 1) { break; }
		//		streamLength += read;
		//
		//		Array.Reverse(bufferIn); //bit to little endian
		//		BigInteger bi = new BigInteger(bufferIn);
		//
		//		//..and return the chars backwards also
		//		int count = bufferOut.Length;
		//		while(bi > 0) {
		//			//core of the conversion is just finding successive remainders
		//			bi = BigInteger.DivRem(bi,@base,out BigInteger rem);
		//			bufferOut[--count] = (int)rem;
		//		}
		//
		//		//deal with the padding
		//		if (read < bytesIn) {
		//			long outLen = (long)Math.Ceiling(
		//				streamLength * (double)bytesOut/(double)bytesIn
		//			);
		//			long padLen = LongCeil(outLen,bytesOut) * bytesOut;
		//			paddingCount = (int)(padLen - outLen);
		//		}
		//
		//		// actuall encoding step
		//		for(int i = 0; i < bytesOut - paddingCount; i++) {
		//			int val = bufferOut[i];
		//			char c = map.Map(val);
		//			yield return c;
		//		}
		//
		//		// return padding if necessary
		//		if (printPadding && paddingCount > 0) {
		//			for(int p = 0; p < paddingCount; p++) {
		//				yield return map.Padding;
		//			}
		//		}
		//
		//		if (read < bytesIn) { break; }
		//	}
		//}
		//
		//public static IEnumerable<char> Decode_1(Stream s, ICharLookup map)
		//{
		//	int bytesIn = map.BytesIn;
		//	int bytesOut = map.BytesOut;
		//	int @base = map.Base();
		//	byte[] bufferIn = new byte[bytesIn];
		//	int[] bufferOut = new int[bytesOut];
		//
		//	return null;
		//}

		public static IEnumerable<char> Encode(Stream s, ICharLookup map)
		{
			var chars = StreamAsChars(s);
			var iter = ChangeBase(chars,CodecBase256.Self,map);
			return iter;
		}

		public static IEnumerable<char> Decode(Stream s, ICharLookup map)
		{
			var chars = StreamAsChars(s);
			var iter = ChangeBase(chars,map,CodecBase256.Self);
			return iter;
		}

		public static IEnumerable<char> ChangeBase(IEnumerable<char> src, ICharLookup inMap, ICharLookup outMap)
		{
			int charsOut = inMap.BytesIn * outMap.BytesOut;
			var valIter = ChangeBaseInternal(src,inMap,outMap);
			int[] currArr = new int[charsOut];
			int[] lastArr = null;
			int pos = 0;

			foreach(int val in valIter)
			{
				currArr[pos] = val;
				if (lastArr != null) {
					char c = outMap.Map(lastArr[pos]);
					yield return c;
				}
				pos++;
				if (pos >= charsOut) {
					pos = 0;
					if (lastArr == null) {
						lastArr = new int[charsOut];
					}
					int[] temp = currArr;
					currArr = lastArr;
					lastArr = temp;
					Array.Clear(currArr,0,charsOut);
				}
			}

			if (lastArr != null)
			{
				int pad = 0;
				for(int v = charsOut - 1; v>=0; v--) {
					if (lastArr[v] == 0) { pad++; }
				}
				for(int i = 0; i<charsOut - pad; i++) {
					char c = outMap.Map(lastArr[i]);
					yield return c;
				}
				if (outMap.IncludePadding) {
					for(int p = 0; p<pad; p++) {
						yield return outMap.Padding;
					}
				}
			}
		}

		static IEnumerable<int> ChangeBaseInternal(IEnumerable<char> src, ICharLookup inMap, ICharLookup outMap)
		{
			int charsOut = inMap.BytesIn * outMap.BytesOut;
			int charsIn = inMap.BytesOut * outMap.BytesIn;
			var unInMap = new GlifMap(inMap);
			int[] inArr = new int[charsIn];
			int[] outArr = new int[charsOut];
			var etor = src.GetEnumerator();
			bool done = false;
			int inBase = inMap.Base();
			int outBase = outMap.Base();

			while(!done)
			{
				Array.Clear(inArr,0,charsIn);
				Array.Clear(outArr,0,charsOut);

				int inCount = 0;
				while(inCount < charsIn)
				{
					if (etor.MoveNext()) {
						int digit = unInMap.Map(etor.Current);
						int iIndex = charsIn - inCount - 1;
						inArr[iIndex] = digit;
					}
					else {
						done = true;
						break;
					}
					inCount++;
				}

				int charsSet = ChangeBase(inBase,outBase,inArr,ref outArr);

				for(int o=charsSet-1; o >= 0; o--)
				{
					yield return outArr[o];
				}
			}
		}

		static int ChangeBase(int baseIn, int baseOut, int[] digitsIn, ref int[] digitsOut)
		{
			BigInteger base10 = BigInteger.Zero;
			BigInteger bBaseIn = (BigInteger)baseIn;
			BigInteger bBaseOut = (BigInteger)baseOut;
			for(int i = 0; i < digitsIn.Length; i++)
			{
				BigInteger digit = (BigInteger)digitsIn[i];
				if (digit >= baseIn) {
					throw new ArgumentNullException("Input digit must be smaller than the base");
				}
				base10 += digit * BigInteger.Pow(bBaseIn,i);
			}
			int o = 0;
			while(base10 > 0)
			{
				base10 = BigInteger.DivRem(base10,bBaseOut,out BigInteger rem);
				digitsOut[o] = (int)rem;
				o++;
			}
			return o;
		}

		static long LongCeil(long num, long den)
		{
			return num / den + (num % den == 0 ? 0 : 1);
		}

		static IEnumerable<char> StreamAsChars(Stream s)
		{
			int val;
			while(0 <= (val = s.ReadByte())) {
				yield return (char)val;
			}
		}
	}
}
