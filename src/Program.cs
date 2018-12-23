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

		static IEnumerable<char> ConvertIt(ICharLookup map,IEnumerable<byte> data)
		{
			//converts from base 256 to map.Base

			int toBase = map.Base;
			foreach(byte b in data)
			{

			}

			yield return '0';
		}



		// https://www.codeproject.com/Articles/16035/Base-Conversion-of-Very-Long-Positive-Integers
		//Convert number in string representation from base:from to base:to. 
		//Return result as a string
		static String ConvertIt(int from, int to, String s)
		{
			//Return error if input is empty
			if (String.IsNullOrEmpty(s))
			{
				return ("Error: Nothing in Input String");
			}
			//only allow uppercase input characters in string
			s = s.ToUpper();

			//only do base 2 to base 36 (digit represented by characters 0-Z)"
			if (from < 2 || from > 36 || to < 2 || to > 36) 
			{ return ("Base requested outside range"); }

			//convert string to an array of integer digits representing number in base:from
			int il = s.Length;
			int[] fs = new int[il];
			int k = 0;
			for (int i = s.Length - 1; i >= 0; i--)
			{
				if (s[i] >= '0' && s[i] <= '9') { fs[k++] = (int)(s[i] - '0'); }
				else
				{
					if (s[i] >= 'A' && s[i] <= 'Z') { fs[k++] = 10 + (int)(s[i] - 'A'); }
					else
					{ return ("Error: Input string must only contain any of 0-9 or A-Z"); } //only allow 0-9 A-Z characters
				}
			}

			//check the input for digits that exceed the allowable for base:from
			foreach(int i in fs)
			{
				if (i >= from) { return ("Error: Not a valid number for this input base"); }
			}

			//find how many digits the output needs
			int ol = il * (from / to+1);
			int[] ts = new int[ol+10]; //assign accumulation array
			int[] cums = new int[ol+10]; //assign the result array
			ts[0] = 1; //initialize array with number 1 

			//evaluate the output
			for (int i = 0; i < il; i++) //for each input digit
			{
				for (int j = 0; j < ol; j++) //add the input digit 
					// times (base:to from^i) to the output cumulator
				{
					cums[j] += ts[j] * fs[i];
					int temp = cums[j];
					int rem = 0;
					int ip = j;
					do // fix up any remainders in base:to
					{
						rem = temp / to;
						cums[ip] = temp-rem*to; ip++;
						cums[ip] += rem;
						temp = cums[ip];
					}
					while (temp >=to);
				}

				//calculate the next power from^i) in base:to format
				for (int j = 0; j < ol; j++)
				{
					ts[j] = ts[j] * from;
				} 
				for(int j=0;j<ol;j++) //check for any remainders
				{
					int temp = ts[j];
					int rem = 0;
					int ip = j;
					do  //fix up any remainders
					{
						rem = temp / to;
						ts[ip] = temp - rem * to; ip++;
						ts[ip] += rem;
						temp = ts[ip];
					}
					while (temp >= to);
				}
			}

			//convert the output to string format (digits 0,to-1 converted to 0-Z characters) 
			String sout = String.Empty; //initialize output string
			bool first = false; //leading zero flag
			for (int i = ol ; i >= 0; i--)
			{
				if (cums[i] != 0) { first = true; }
				if (!first) { continue; }
				if (cums[i] < 10) { sout += (char)(cums[i] + '0'); }
				else { sout += (char)(cums[i] + 'A'-10); }
			}
			if (String.IsNullOrEmpty(sout)) { return "0"; } //input was zero, return 0
			//return the converted string
			return sout;
		}
	}
}