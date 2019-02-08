using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using TuckBytesInCode;

namespace test
{
	class Program
	{
		static void Main2(string[] args)
		{
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

			var tb = new TestBase();
			//tb.Test64_Encode_1();
			//tb.Test85_Encode_1();
			tb.Test64_to_85_1();
		}

		static void Main(string[] args)
		{
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

			// var next = TuckBytes.ChangeBase(TestBase.B64Text,CodecBase64.Self,CodecBase256.Self);
			// var next = TuckBytes.ChangeBase(TestBase.OrigText,CodecBase256.Self,CodecBase64.Self);
			// var next = TuckBytes.ChangeBase(TestBase.B85Text,CodecBase85.Self,CodecBase256.Self);
			// var next = TuckBytes.ChangeBase(TestBase.OrigText,CodecBase256.Self,CodecBase85.Self);
			var next = TuckBytes.ChangeBase(TestBase.OrigText,CodecBase256.Self,CodecBase91.Self);
			
			foreach(char c in next) {
				char p = c;
				if (Char.IsControl(c)) { p = ' '; }
				Console.WriteLine(p+"\t"+((int)c));
			}
			return;
		}

		static void Main4(string[] args)
		{
			var gf = new GlifMap(CodecBase85.Self);
			for(int i=0; i<16; i++)
			{
				char c = TestBase.B85Text[i];
				int x = gf.Map(c);
				Console.WriteLine(c+"\t"+x);
			}
			return;

			//Console.WriteLine(CodecUtf8NAscii.AllChars.Length); return;
			//GenerateUtf8Chars(); return;

			/// var x = TuckBytesInCode.CodecUtf8NAscii.Self;
			// Console.WriteLine("nn = "+x.Base);
			//14390
			//for (int count = 15000; count >= 13000; count--)
			//{
			//	for(int p=1; p<=5; p++)
			//	{
			//		double ratio = Math.Log(count)/(8*Math.Log(2));
			//		//double ratio = 1.71998602290273;
			//		FindFraction(out double num, out double den, ratio, 1.0/Math.Pow(10,p));
			//		Console.WriteLine(count+"@"+p+" : "+num+"/"+den+" = "+(num/den)+" diff "+Math.Abs(num/den-ratio)+" count = "+Math.Pow(2,8*num/den));
			//	}
			//}
		}

		//TODO look at this way of doing this
		// https://math.stackexchange.com/questions/1981310/how-to-find-fraction-from-decimal
		static void FindFraction(out double num, out double den,double ratio, double accuracy = 0.001)
		{
			num = 1; den = 1;
			double placeCount = Math.Pow(10,CountPlaces(ratio));
			// Console.WriteLine("placeCount = "+placeCount);
			if (placeCount <= 1) {
				num = ratio;
				return;
			}
			int round = Math.Max(15,Math.Min(0,(int)Math.Log(1/accuracy,10)));
			placeCount +=2; //do at least one round

			for(double p=2; p<placeCount; p++)
			{
				num = Math.Floor(Math.Round(ratio * p,round));
				// Console.WriteLine(num+"/"+p+" = "+(num/p));
				double test = Math.Abs(num/p - ratio);
				if (test < accuracy) { den = p; break; }
			}
		}

		static int CountPlaces(double d)
		{
			decimal argument = (decimal)d;
			int count = BitConverter.GetBytes(decimal.GetBits(argument)[3])[2];
			return count;
		}

		static double GCD(double a, double b)
		{
			while (a > 0 && b > 0)
			{
				if (a > b) {
					a %= b;
				} else {
					b %= a;
				}
			}
			return a <= 0 ? b : a;
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
						|| cat == UnicodeCategory.EnclosingMark
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
						|| cat == UnicodeCategory.OpenPunctuation
						|| cat == UnicodeCategory.ClosePunctuation
						// || cat == UnicodeCategory.InitialQuotePunctuation
						// || cat == UnicodeCategory.FinalQuotePunctuation
						|| cat == UnicodeCategory.OtherPunctuation
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
