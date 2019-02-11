using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TuckBytesInCode
{
	public static class Options
	{
		public static void Usage()
		{
			Log.Message(
				nameof(TuckBytesInCode) + " - Converts text-encoded binary data from one base to another"
				+"\nUsage: "+nameof(TuckBytesInCode)+" [options] [data]"
				+"\n\n Options:"
				+"\n -f (file)        read data from a file"
				+"\n -o (file)        write data to a file"
				+"\n -bi (base)       input base  (default base256)"
				+"\n -bo (base)       output base (default base256)"
				+"\n -dc              enable direct conversion without going though base256 first"
				+"\n                   note: in many cases this will produce invalid output"
				+"\n -dl              disables adding newlines to the output"
				+"\n -ll (number)     set how many characters per line (default 76)"
				// +"\n\n Custom Base Options:"
				// +"\n -cbi (name)      custom input base (see below)"
				// +"\n -cbo (name)      custom output base (see below)"
				// +"\n -cdef (json)     custom base definition (see below)
				// {name:"", alphabet:"", padding:""}
			);

			PrintBasesAndVariants();
		}

		static void PrintBasesAndVariants()
		{
			Log.Message("\nAvailable Bases:");
			foreach(Base item in Enum.GetValues(typeof(Base)))
			{
				if (item == Base.None) { continue; }
				IBaseInfo info = BaseMap.GetInfo(item);
				if (info == null) { continue; }
				string arg = item.ToString().ToLowerInvariant();
				Log.Message(
					" [" + arg + "] "+info.DisplayName+" - "+info.Description
				);
			}
		}

		public static bool Parse(string[] args)
		{
			int len = args.Length;
			for(int a=0; a<len; a++)
			{
				string curr = args[a];
				if (curr == "-f" && ++a < len) {
					FileNameIn = args[a];
				}
				else if (curr == "-o" && ++a < len) {
					FileNameOut = args[a];
				}
				else if (curr == "-bi" && ++a < len) {
					if (!TryParseBase(args[a], out BaseIn)) {
						Log.Error("Cound not parse base "+args[a]);
						return false;
					}
				}
				else if (curr == "-bo" && ++a < len) {
					if (!TryParseBase(args[a], out BaseOut)) {
						Log.Error("Cound not parse base "+args[a]);
						return false;
					}
				}
				else if (curr == "-dl") {
					DisableNewLines = true;
				}
				else if (curr == "-ll" && ++a < len) {
					if (!int.TryParse(args[a],out int CharactersPerLine)) {
						Log.Error("Count not parse number \""+args[a]+"\"");
						return false;
					}
				}
				else if (curr == "-dc") {
					DirectConvert = true;
				}
				else {
					if (DataFromArgs == null) { DataFromArgs = ""; }
					DataFromArgs += curr;
				}
			}

			//sanity checks
			if (FileNameIn != null && !File.Exists(FileNameIn)) {
				Log.Error("Cannot find file \""+FileNameIn+"\"");
				return false;
			}
			if (CharactersPerLine < 1) {
				Log.Error("Characters per line must be at least 1");
				return false;
			}

			return true;
		}

		static bool TryParseBase(string arg, out Base found)
		{
			found = Base.None;
			foreach(string name in Enum.GetNames(typeof(Base)))
			{
				if (!name.StartsWithIC("base")) { continue; }

				//try to match the whole name
				if (name.EqualsIC(arg)) {
					found = Enum.Parse<Base>(name);
					return true;
				}
				//try to match just the number part
				string num = name.Substring(4);
				if (num.EqualsIC(arg)) {
					found = Enum.Parse<Base>(name);
					return true;
				}
			}
			return false;
		}

		public static string FileNameIn = null;
		public static string FileNameOut = null;
		public static Base BaseIn = Base.Base256;
		public static Base BaseOut = Base.Base256;
		public static bool DisableNewLines = false;
		public static int CharactersPerLine = 76;
		public static bool DirectConvert = false;
		public static string DataFromArgs = null;

	}
}