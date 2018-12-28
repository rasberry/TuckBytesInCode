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
		static string B64Text = "TWFuIGlzIGRpc3Rpbmd1aXNoZWQsIG5vdCBvbmx5IGJ5IGhpcyByZWFzb24sIGJ1dCBieSB0aGlzIHNpbmd1bGFyIHBhc3Npb24gZnJvbSBvdGhlciBhbmltYWxzLCB3aGljaCBpcyBhIGx1c3Qgb2YgdGhlIG1pbmQsIHRoYXQgYnkgYSBwZXJzZXZlcmFuY2Ugb2YgZGVsaWdodCBpbiB0aGUgY29udGludWVkIGFuZCBpbmRlZmF0aWdhYmxlIGdlbmVyYXRpb24gb2Yga25vd2xlZGdlLCBleGNlZWRzIHRoZSBzaG9ydCB2ZWhlbWVuY2Ugb2YgYW55IGNhcm5hbCBwbGVhc3VyZS4=";
		static string B85Text = "9jqo^BlbD-BleB1DJ+*+F(f,q/0JhKF<GL>Cj@.4Gp$d7F!,L7@<6@)/0JDEF<G%<+EV:2F!,O<DJ+*.@<*K0@<6L(Df-\\0Ec5e;DffZ(EZee.Bl.9pF\"AGXBPCsi+DGm>@3BB/F*&OCAfu2/AKYi(DIb:@FD,*)+C]U=@3BN#EcYf8ATD3s@q?d$AftVqCh[NqF<G:8+EV:.+Cf>-FD5W8ARlolDIal(DId<j@<?3r@:F%a+D58'ATD4$Bl@l3De:,-DJs`8ARoFb/0JMK@qB4^F!,R<AKZ&-DfTqBG%G>uD.RTpAKYo'+CT/5+Cei#DII?(E,9)oF*2M7/c";
		static string OrigText = "Man is distinguished, not only by his reason, but by this singular passion from other animals, which is a lust of the mind, that by a perseverance of delight in the continued and indefatigable generation of knowledge, exceeds the short vehemence of any carnal pleasure.";

		[TestMethod]
		public void Test64_Encode_1()
		{
			var s = new MemoryStream(Encoding.ASCII.GetBytes(OrigText));
			var enu = TuckBytes.Encode(s,CodecBase64.Self);
			string test = String.Concat(enu);
			Assert.AreEqual(B64Text.Length,test.Length);
			for(int c=0; c<B64Text.Length; c++) {
				Trace.WriteLine(c+" ["+B64Text[c]+","+test[c]+"]");
				Assert.AreEqual(B64Text[c],test[c]);
			}
		}

		[TestMethod]
		public void Test64_Decode_1()
		{

		}

		[TestMethod]
		public void Test85_Encode_1()
		{
			var s = new MemoryStream(Encoding.ASCII.GetBytes(OrigText));
			var enu = TuckBytes.Encode(s,CodecBase85.Self);
			string test = String.Concat(enu);
			Assert.AreEqual(B85Text.Length,test.Length);
			for(int c=0; c<B85Text.Length; c++) {
				Trace.WriteLine(c+" ["+B85Text[c]+","+test[c]+"]");
				Assert.AreEqual(B85Text[c],test[c]);
			}
		}
	}
}
