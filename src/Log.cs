using System;
using System.Diagnostics;

namespace TuckBytesInCode
{
	public static class Log
	{
		public static void Message(string m)
		{
			Console.WriteLine(m);
		}
		public static void Info(string m)
		{
			Console.WriteLine("I: "+m);
		}
		public static void Debug(string m)
		{
			Console.WriteLine("D: "+m);
		}
		public static void Warning(string m)
		{
			Console.Error.WriteLine("W: "+m);
		}
		public static void Error(string m)
		{
			Console.Error.WriteLine("E: "+m);
		}
	}
}