using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace TuckBytesInCode
{
	public static class Program
	{
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