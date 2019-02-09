using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TuckBytesInCode;

namespace test
{
	[TestClass]
	public class TestBase
	{
		public static string B64Text = "TWFuIGlzIGRpc3Rpbmd1aXNoZWQsIG5vdCBvbmx5IGJ5IGhpcyByZWFzb24sIGJ1dCBieSB0aGlzIHNpbmd1bGFyIHBhc3Npb24gZnJvbSBvdGhlciBhbmltYWxzLCB3aGljaCBpcyBhIGx1c3Qgb2YgdGhlIG1pbmQsIHRoYXQgYnkgYSBwZXJzZXZlcmFuY2Ugb2YgZGVsaWdodCBpbiB0aGUgY29udGludWVkIGFuZCBpbmRlZmF0aWdhYmxlIGdlbmVyYXRpb24gb2Yga25vd2xlZGdlLCBleGNlZWRzIHRoZSBzaG9ydCB2ZWhlbWVuY2Ugb2YgYW55IGNhcm5hbCBwbGVhc3VyZS4=";
		public static string B85Text = "9jqo^BlbD-BleB1DJ+*+F(f,q/0JhKF<GL>Cj@.4Gp$d7F!,L7@<6@)/0JDEF<G%<+EV:2F!,O<DJ+*.@<*K0@<6L(Df-\\0Ec5e;DffZ(EZee.Bl.9pF\"AGXBPCsi+DGm>@3BB/F*&OCAfu2/AKYi(DIb:@FD,*)+C]U=@3BN#EcYf8ATD3s@q?d$AftVqCh[NqF<G:8+EV:.+Cf>-FD5W8ARlolDIal(DId<j@<?3r@:F%a+D58'ATD4$Bl@l3De:,-DJs`8ARoFb/0JMK@qB4^F!,R<AKZ&-DfTqBG%G>uD.RTpAKYo'+CT/5+Cei#DII?(E,9)oF*2M7/cYkO";
		public static string B91Text = "8D$J`/wC4!c.hQ;mT8,<p/&Y/H@$]xlL3oDg<W.0$FW6GFMo_D8=8=}AMf][|LfVd/<P1o/1Z2(.I+LR6tQQ0o1a/2/WtN3$3t[x&k)zgZ5=p;LRe.{B[pqa(I.WRT%yxtB92oZB,2,Wzv;Rr#N.cju\"JFXiZBMf<WMC&$@+e95p)z01_*UCxT0t88Km=UQJ;WH[#F]4pE>i3o(g7=$e7R2u>xjLxoefB.6Yy#~uex8jEU_1e,MIr%!&=EHnLBn2h>M+;Rl3qxcL5)Wfc,HT$F]4pEsofrFK;W&eh#=#},|iKB,2,W]@fVlx,a<m;i=CY<=Hb%}+},0D";
		public static string OrigText = "Man is distinguished, not only by his reason, but by this singular passion from other animals, which is a lust of the mind, that by a perseverance of delight in the continued and indefatigable generation of knowledge, exceeds the short vehemence of any carnal pleasure.";

		static void TestEncode(string inText,string outText, ICharLookup codec)
		{
			var s = new MemoryStream(Encoding.ASCII.GetBytes(inText));
			Trace.WriteLine("s length = "+s.Length);
			var enu = TuckBytes.Encode(s,codec);
			string test = String.Concat(enu);
			Assert.AreEqual(outText.Length,test.Length);
			for(int c=0; c<outText.Length; c++) {
				Trace.WriteLine(c+" ["+outText[c]+","+test[c]+"]");
				Assert.AreEqual(outText[c],test[c]);
			}
		}

		static void TestDecode(string inText,string outText, ICharLookup codec)
		{
			var s = new MemoryStream(Encoding.ASCII.GetBytes(inText));
			var enu = TuckBytes.Decode(s,codec);
			string test = String.Concat(enu);
			Assert.AreEqual(outText.Length,test.Length);
			for(int c=0; c<outText.Length; c++) {
				Trace.WriteLine(c+" ["+outText[c]+","+test[c]+"]");
				Assert.AreEqual(outText[c],test[c]);
			}
		}

		[TestMethod] public void Test64_Encode_1()
		{
			TestEncode(OrigText,B64Text,CodecBase64.Self);
		}
		[TestMethod] public void Test64_Decode_1()
		{
			TestDecode(B64Text,OrigText,CodecBase64.Self);
		}

		[TestMethod] public void Test85_Encode_1()
		{
			TestEncode(OrigText,B85Text,CodecBase85.Self);
		}
		[TestMethod] public void Test85_Decode_1()
		{
			TestDecode(B85Text,OrigText,CodecBase85.Self);
		}

		//TODO base91 seems to encode differently than 64/85
		// maybe it uses little endian ?
		//[TestMethod] public void Test91_Encode_1()
		//{
		//	TestEncode(OrigText,B91Text,CodecBase91.Self);
		//}
		//[TestMethod] public void Test91_Decode_1()
		//{
		//	TestDecode(B91Text,OrigText,CodecBase91.Self);
		//}

		// [TestMethod]
		public void Test64_to_85_1()
		{
			var s = new MemoryStream(Encoding.ASCII.GetBytes(B64Text));
			var enu = TuckBytes.ChangeBase(B64Text,CodecBase64.Self,CodecBase85.Self);
			string test = String.Concat(enu);
			//Assert.AreEqual(B85Text.Length,test.Length);
			for(int c=0; c<B85Text.Length; c++) {
				Trace.WriteLine(c+" ["+B85Text[c]+","+test[c]+"]");
				//Assert.AreEqual(B85Text[c],test[c]);
			}
		}
	}
}
