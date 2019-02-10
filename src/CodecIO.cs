using System;
using System.IO;

namespace TuckBytesInCode
{
	public interface ICodecIO : IDisposable
	{
		int Read();
		void Write(char c);
	}

	public class CodecIO : ICodecIO, IDisposable
	{
		public enum Mode { Read = 0, Write = 1 }

		public CodecIO(Mode mode, string fileName, ICharLookup codec)
		{
			IOMode = mode;
			if (mode == Mode.Read) {
				Data = File.Open(fileName,FileMode.Open,FileAccess.Read,FileShare.Read);
				if (!codec.TreatAsBinary) {
					Reader = new StreamReader(Data);
				}
			}
			else if (mode == Mode.Write) {
				Data = File.Open(fileName,FileMode.Create,FileAccess.Write,FileShare.Read);
				if (!codec.TreatAsBinary) {
					Writer = new StreamWriter(Data);
				}
			}
		}

		public CodecIO(TextReader reader)
		{
			Reader = reader;
			IOMode = Mode.Read;
		}

		public CodecIO(TextWriter writer)
		{
			Writer = writer;
			IOMode = Mode.Write;
		}

		public int Read()
		{
			if (IOMode != Mode.Read) {
				throw new InvalidOperationException("not in read mode");
			}
			int data;
			if (Reader != null) {
				data = Reader.Read();
			} else {
				data = Data.ReadByte();
			}
			// Log.Debug("IO Read "+data);
			return data;
		}

		public void Write(char c)
		{
			if (IOMode != Mode.Write) {
				throw new InvalidOperationException("not in write mode");
			}
			// Log.Debug("IO Write "+(int)c);
			if (Writer != null) {
				Writer.Write(c);
			} else {
				Data.WriteByte((byte)c);
			}
		}

		public void Dispose()
		{
			if (Writer != null) {
				Writer.Dispose();
			}
			if (Reader != null) {
				Reader.Dispose();
			}
			//only need to dispose data if it was used on it's own
			if (Writer == null && Reader == null && Data != null) {
				Data.Dispose();
			}
		}

		TextReader Reader = null;
		TextWriter Writer = null;
		FileStream Data = null;
		Mode IOMode = Mode.Read;
	}
}