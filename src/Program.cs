using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace TuckBytesInCode
{
	public static class Program
	{
		//TODO
		//=add option to choose conversion bases
		// maybe -from / -to parameters or something
		// might be good to choose base256 if either side if left out
		// also need to go though base256 if boths sides are not base256
		//  otherwise conversion will be incorrect
		//=be able to read base-encoded formatted text files
		// basically ignore control characters
		//=add an option to control the output line length
		// 76 is the recommended value

		static void Main(string[] args)
		{
			if (args.Length < 1) {
				Console.WriteLine("Usage "+nameof(TuckBytesInCode)+" (file)");
				return;
			}

			using (var fs = File.Open(args[0],FileMode.Open,FileAccess.Read,FileShare.Read))
			{
				int count = 0;
				var chars = TuckBytes.Encode(fs,CodecBase14k.Self);
				foreach(char c in chars) {
					Console.Write(c);
					count++;
					if (count >= 76) {
						Console.WriteLine();
						count = 0;
					}
				}
				Console.WriteLine();
			}
		}
	}
}