using System;
using System.IO;

namespace TuckBytesInCode
{
	public class BitReader
	{
		public BitReader(Stream s)
		{
			BaseStream = s;
		}

		Stream BaseStream;
		byte Current;
		int BitsLeft;
		const int BitsInByte = 8; //this is a bit silly.. no magic numbers here!

		public bool[] ReadBits(int length)
		{
			int counter = 0;
			bool[] bits = null;

			while(counter < length)
			{
				while(BitsLeft > 0) {
					//delay instantiate bits so that reads past then end of the stream return null
					if (bits == null) {
						bits = new bool[length];
					}
					bits[counter] = (1 << (BitsLeft - 1) & Current) != 0;
					counter++;
					BitsLeft--;
					if (counter >= length) { break; }
				}

				if (counter >= length) { break; }
				int next = BaseStream.ReadByte();
				if (next == -1) { break; } //end of stream reached
				Current = (byte)next;
				BitsLeft = BitsInByte;
			}
			return bits;
		}

		public int ReadBitsAsInt(int length)
		{
			return BitsToInt(ReadBits(length));
		}

		public static int BitsToInt(bool[] bits)
		{
			if (bits == null) { return -1; }
			int final = 0;
			for(int b=0; b<bits.Length; b++) {
				final |= (bits[b] ? 1 : 0) << (bits.Length - b - 1);
			}
			return final;
		}
	}
}