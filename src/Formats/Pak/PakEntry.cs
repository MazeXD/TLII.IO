using System;
using System.IO;
using Ionic.Zlib;
using TLII.IO.Formats.Pak.Manifest;

namespace TLII.IO.Formats.Pak
{
    public class PakEntry
    {
        private readonly Stream _stream;
        private readonly PakManifestFile _manifest;
        private readonly string _directory;
        
        public string ArchivePath
        {
            get
            {
                return Path.Combine(_directory, _manifest.Name);
            }
        }

        public uint Checksum
        {
            get
            {
                return _manifest.Checksum;
            }
        }

        public ulong Timestamp
        {
            get
            {
                return _manifest.Timestamp;
            }
        }

        public PakFileType Type
        {
            get
            {
                return _manifest.Type;
            }
        }

        internal PakEntry(Stream stream, string directory, PakManifestFile manifest)
        {
            _stream = stream;
            _directory = directory;
            _manifest = manifest;
        }

        public byte[] Extract()
        {
            return InternalExtract();
        }

        public void Extract(string path)
        {
            byte[] data = InternalExtract();

            while(Path.GetExtension(path) != "")
            {
                path = Path.GetFileNameWithoutExtension(path);
            }

            string outputPath = Path.Combine(path, ArchivePath);
            string dirName = Path.GetDirectoryName(outputPath);

            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            File.WriteAllBytes(outputPath, data);
        }

        private byte[] InternalExtract()
        {
            _stream.Position = _manifest.Offset;

            var sizeBytes = new byte[4];
            _stream.Read(sizeBytes, 0, 4);

            int uncompressedSize = BitConverter.ToInt32(sizeBytes, 0);
            if (uncompressedSize <= 0)
            {
                return new byte[0];
            }

            _stream.Position += 4;

            var buffer = new byte[_manifest.UncompressedSize];

            var zlibStream = new ZlibStream(_stream, CompressionMode.Decompress, true);

            zlibStream.Read(buffer, 0, buffer.Length);

            zlibStream.Close();
            zlibStream.Dispose();

            return buffer;
        }
    }
}
