using System;
using System.IO;
using System.Text;

namespace PELib.ExtensionMethods
{
    public static class StreamExtensionMethods
    {
        /// <summary>
        /// Seeks to an aligned offset in the stream, with respect to a given position.
        /// </summary>
        /// <param name="s">This stream to seek.</param>
        /// <param name="align">The alignment. Must be a power of two.</param>
        /// <param name="wrt">The offset to align with-respect-to.</param>
        /// <returns></returns>
        public static bool SeekAlign(this Stream s, long align, long wrt = 0)
        {
            long newPos = (s.Position - wrt).Align(align) + wrt;
            return s.Seek(newPos, SeekOrigin.Begin) == newPos;
        }

        public static void SaveToFile(this Stream stream, string filename, int length)
        {
            var buf = new byte[length];
            if (stream.Read(buf, 0, length) != length)
                throw new Exception("Short read of stream.");

            using (var outStream = new FileStream(filename, FileMode.Create)) {
                outStream.Write(buf, 0, buf.Length);
            }
        }

        /// <summary>
        /// Reads a null-terminated string from a Stream using a BinaryReader.
        /// The encoding of the string is determined by that of the BinaryReader
        /// (passed to its constructor).
        /// </summary>
        /// <returns>A string read from the Stream.</returns>
        public static string ReadNullTerminatedString(this BinaryReader br)
        {
            var sb = new StringBuilder();
            while (true) {
                var c = br.ReadChar();
                if (c == 0) break;
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string ReadNullTerminatedString(this Stream s, long position, Encoding encoding)
        {
            using (new StreamKeeper(s)) {
                s.Position = position;
                var br = new BinaryReader(s, encoding);

                return br.ReadNullTerminatedString();
            }
        }

    }
}
