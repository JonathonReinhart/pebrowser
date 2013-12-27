using System;
using System.IO;

namespace PELib
{
    /// <summary>
    /// Restores the position of an associated stream upon disposal.
    /// Designed for use in a 'using' block.
    /// </summary>
    internal class StreamKeeper : IDisposable
    {
        private readonly Stream m_stream;
        private readonly long m_initPos;

        public StreamKeeper(Stream stream) {
            m_stream = stream;
            m_initPos = stream.Position;

            if (!stream.CanSeek)
                throw new Exception("StreamKeeper can only be used with seekable streams.");
        }

        public void Dispose() {
            m_stream.Position = m_initPos;
        }
    }
}
