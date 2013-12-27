using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PELib
{
    internal static class ExtensionMethods
    {
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

        public static string ReadNullTerminatedString(this Stream s, long position, Encoding encoding) {
            using (new StreamKeeper(s)) {
                s.Position = position;
                var br = new BinaryReader(s, encoding);

                return br.ReadNullTerminatedString();
            }
        }
    }
}
