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
			//To deal with the padding, this hangs on to the previous array of converted characters
			// so that we can take any trailing 0 values and either remove or convert to padding
			//TODO maybe merge this with ChangeBaseInternal since ChangeBaseInternal is now farily simple
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
			//the following calculation is a bit magical
			// i'm not exactly sure why this works out but it does
			int charsOut = inMap.BytesIn * outMap.BytesOut;
			int charsIn = inMap.BytesOut * outMap.BytesIn;

			var unInMap = new GlifMap(inMap);
			int[] inArr = new int[charsIn];
			int[] outArr = new int[charsOut];
			int inBase = inMap.Base();
			int outBase = outMap.Base();

			var iter = src.GetEnumerator();
			bool done = false;

			while(!done)
			{
				Array.Clear(inArr,0,charsIn);
				Array.Clear(outArr,0,charsOut);

				int inCount = 0;
				while(inCount < charsIn)
				{
					if (iter.MoveNext()) {
						int digit = unInMap.Map(iter.Current);

						//reverse incoming bigendgian order
						int iIndex = charsIn - inCount - 1;
						inArr[iIndex] = digit;
					}
					else {
						done = true;
						break;
					}
					inCount++;
				}

				//base calculation is done in little endian
				int charsSet = ChangeBase(inBase,outBase,inArr,ref outArr);

				//reverse again to restore big endianess
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
