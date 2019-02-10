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
				Options.Usage();
				return;
			} else {
				if (!Options.Parse(args)) { return; }
			}

			ICodecIO dataIn = null;
			ICodecIO dataOut = null;
			var codecIn = BaseMap.GetInstance(Options.BaseIn);
			var codecOut = BaseMap.GetInstance(Options.BaseOut);
			if (codecIn == null || codecOut == null) {
				Log.Error("codec not available");
				return;
			}

			try {
				if (Options.FileNameIn != null) {
					dataIn = new CodecIO(CodecIO.Mode.Read,Options.FileNameIn,codecIn);
				} else if (Options.DataFromArgs != null) {
					dataIn = new CodecIO(new StringReader(Options.DataFromArgs));
				} else {
					dataIn = new CodecIO(Console.In);
				}

				if (Options.FileNameOut != null) {
					dataOut = new CodecIO(CodecIO.Mode.Write,Options.FileNameOut,codecOut);
				} else {
					dataOut = new CodecIO(Console.Out);
				}

				DoConversion(dataIn,dataOut,codecIn,codecOut);
			}
			finally {
				//We're only disposing if we opened a file

				if (dataIn != null) {
					dataIn.Dispose();
				}
				if (dataOut != null) {
					dataOut.Dispose();
				}
			}
		}

		static void DoConversion(ICodecIO dataIn,ICodecIO dataOut, ICharLookup codecIn, ICharLookup codecOut)
		{
			IEnumerable<char> charsOut = null;

			// direct conversion is better, but only works sometimes
			bool shouldDirectConvert =
				Options.DirectConvert //override
				|| Options.BaseIn == Base.Base256
				|| Options.BaseOut == Base.Base256
			;
			Log.Debug("shouldDirectConvert = "+shouldDirectConvert);
			Log.Debug("BaseIn = "+Options.BaseIn);
			Log.Debug("BaseOut = "+Options.BaseOut);

			if (shouldDirectConvert) {
				charsOut = TuckBytes.ChangeBase(dataIn,codecIn,codecOut);
			} else {
				var binary = TuckBytes.ChangeBase(dataIn,codecIn,CodecBase256.Self);
				charsOut = TuckBytes.ChangeBase(dataIn,CodecBase256.Self,codecOut);
			}

			int count = Options.CharactersPerLine - 1;
			foreach(char c in charsOut)
			{
				dataOut.Write(c);
				if (!Options.DisableNewLines && !codecOut.TreatAsBinary) {
					if (count <= 0) {
						dataOut.Write('\n');
						count = Options.CharactersPerLine;
					}
					count--;
				}
			}
		}
	}
}