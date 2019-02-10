using System;

namespace TuckBytesInCode
{
	public enum Base {
		None = 0,
		Base10,
		Base16,
		Base64,
		Base85,
		Base91,
		Base94,
		Base14k,
		Base256
	}

	public static class BaseMap
	{
		public static IBaseInfo GetInfo(Base which)
		{
			switch(which)
			{
			case Base.Base10:  return CodecBase10.Self;
			case Base.Base14k: return CodecBase14k.Self;
			case Base.Base16:  return CodecBase16.Self;
			case Base.Base64:  return CodecBase64.Self;
			case Base.Base85:  return CodecBase85.Self;
			// case Base.Base91:  return CodecBase91.Self;
			case Base.Base94:  return CodecBase94.Self;
			case Base.Base256: return CodecBase256.Self;
			}

			return null;
		}

		public static ICharLookup GetInstance(Base which)
		{
			//by convention codecs implement both IBaseInfo and ICharLookup
			return (ICharLookup)GetInfo(which);
		}
	}
}