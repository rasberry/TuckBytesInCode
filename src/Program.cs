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
				var chars = TuckBytes.Encode(fs,CodecUtf8NAscii.Self);
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

		static void GenerateUtf8Chars()
		{
			int linelen = 80;
			using (var fs = File.Open("char.txt",FileMode.Create,FileAccess.Write,FileShare.Read))
			using (var sw = new StreamWriter(fs,Encoding.UTF8))
			{
				int count = 0;
				foreach(char c in DoSomething())
				{
					var cat = char.GetUnicodeCategory(c);
					if (false
						|| cat == UnicodeCategory.UppercaseLetter
						|| cat == UnicodeCategory.LowercaseLetter
						|| cat == UnicodeCategory.TitlecaseLetter
						|| cat == UnicodeCategory.ModifierLetter
						|| cat == UnicodeCategory.OtherLetter
						// || cat == UnicodeCategory.NonSpacingMark
						// || cat == UnicodeCategory.SpacingCombiningMark
						// || cat == UnicodeCategory.EnclosingMark
						|| cat == UnicodeCategory.DecimalDigitNumber
						|| cat == UnicodeCategory.LetterNumber
						|| cat == UnicodeCategory.OtherNumber 
						// || cat == UnicodeCategory.SpaceSeparator 
						// || cat == UnicodeCategory.LineSeparator 
						// || cat == UnicodeCategory.ParagraphSeparator 
						// || cat == UnicodeCategory.Control 
						// || cat == UnicodeCategory.Format 
						// || cat == UnicodeCategory.Surrogate 
						// || cat == UnicodeCategory.PrivateUse 
						// || cat == UnicodeCategory.ConnectorPunctuation 
						// || cat == UnicodeCategory.DashPunctuation 
						// || cat == UnicodeCategory.OpenPunctuation 
						// || cat == UnicodeCategory.ClosePunctuation 
						// || cat == UnicodeCategory.InitialQuotePunctuation 
						// || cat == UnicodeCategory.FinalQuotePunctuation 
						// || cat == UnicodeCategory.OtherPunctuation 
						|| cat == UnicodeCategory.MathSymbol 
						|| cat == UnicodeCategory.CurrencySymbol 
						// || cat == UnicodeCategory.ModifierSymbol 
						|| cat == UnicodeCategory.OtherSymbol 
						// || cat == UnicodeCategory.OtherNotAssigned
					) {
						sw.Write(c);
						count = (count + 1) % linelen;
						if (count <1 ) { sw.Write("\"\n\t+@\""); }
					}
				}
			}
		}

		static IEnumerable<char> DoSomething()
		{
			System.Net.WebClient client = new System.Net.WebClient();
			string definedCodePoints = File.ReadAllText("UnicodeData.txt");
			//string definedCodePoints = client.DownloadString("http://unicode.org/Public/UNIDATA/UnicodeData.txt");
			System.IO.StringReader reader = new System.IO.StringReader(definedCodePoints);
			System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
			while(true) {
				string line = reader.ReadLine();
				if(line == null) { break; }
				int codePoint = Convert.ToInt32(line.Substring(0, line.IndexOf(";")), 16);
				if(codePoint >= 0xD800 && codePoint <= 0xDFFF) {
					//surrogate boundary; not valid codePoint, but listed in the document
				} else {
					string utf16 = char.ConvertFromUtf32(codePoint);
					byte[] utf8 = encoder.GetBytes(utf16);
					//TODO: something with the UTF-8-encoded character
					if (Char.TryParse(utf16,out char c)) {
						yield return c;
					}
				}
			}
		}
	}
}