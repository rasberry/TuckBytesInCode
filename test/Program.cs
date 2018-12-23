using System;

namespace test
{
	class Program
	{
		static void Main(string[] args)
		{
			var x = TuckBytesInCode.CodecUtf8NAscii.Self;
			Console.WriteLine("nn = "+x.Base);

			int count = 14390;
			double ratio = Math.Log(count)/(8*Math.Log(2));
			//double ratio = 1.71998602290273;
			FindFraction(out double num, out double den, ratio,0.000001);
			Console.WriteLine(num+"/"+den+" = "+(num/den)+" diff "+Math.Abs(num/den-ratio));
			Console.WriteLine("count = "+Math.Pow(2,8*num/den));
		}

		//TODO look at this way of doing this
		// https://math.stackexchange.com/questions/1981310/how-to-find-fraction-from-decimal
		static void FindFraction(out double num, out double den,double ratio, double accuracy = 0.001)
		{
			num = 1; den = 1;
			double placeCount = Math.Pow(10,CountPlaces(ratio));
			Console.WriteLine("placeCount = "+placeCount);
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
			while (a != 0 && b != 0)
			{
				if (a > b)
					a %= b;
				else
					b %= a;
			}

			return a == 0 ? b : a;
		}
	}
}
