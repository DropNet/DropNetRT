using System;
using System.IO;

namespace DropNetRT.Helpers
{
    internal static class StreamUtils
    {
        private const int STREAM_BUFFER_SIZE = 128 * 1024; // 128KB

        public static void CopyStream(Stream source, Stream target)
        {
            CopyStream(source, target, new byte[STREAM_BUFFER_SIZE]);
        }

        public static void CopyStream(Stream source, Stream target, byte[] buffer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (target == null) throw new ArgumentNullException("target");

            if (buffer == null) buffer = new byte[STREAM_BUFFER_SIZE];
            int bufferLength = buffer.Length;
            int bytesRead;
            while ((bytesRead = source.Read(buffer, 0, bufferLength)) > 0)
                target.Write(buffer, 0, bytesRead);
        }
    }
}
