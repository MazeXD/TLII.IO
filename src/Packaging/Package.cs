/* Thanks to adidishen for his 
 * Torchlight II Plugin for
 * his Package Manager Salad
 * http://forums.runicgames.com/viewtopic.php?f=48&t=44052
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;

namespace TL2MH.IO.Packaging
{
    public class Package : IDisposable
    {
        public readonly FolderEntry RootFolder;

        private FileStream _packageStream;
        private string _fileName;

        public Package(string fileName, FolderEntry rootFolder)
        {
            this.RootFolder = rootFolder;

            this._fileName = fileName;
            this._packageStream = File.Open(this._fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
        }

        public byte[] GetContent(FileEntry fileEntry)
        {
            var entry = (PackageEntry)fileEntry.Tag;

            this._packageStream.Seek(entry.Offset, SeekOrigin.Begin);

            byte[] sizeBytes = new byte[sizeof(int)]; // Uncompressed size

            this._packageStream.Read(sizeBytes, 0, sizeof(int));

            if (BitConverter.ToInt32(sizeBytes, 0) > 0)
            {
                this._packageStream.Seek(entry.Offset + 10, SeekOrigin.Begin);

                byte[] content = new byte[0];

                using (DeflateStream dfsDecompressor =
                    new DeflateStream(this._packageStream, CompressionMode.Decompress, true))
                {
                    byte[] buffer = new byte[4096];

                    int read = dfsDecompressor.Read(buffer, 0, 4096);

                    int pos;
                    while (read > 0)
                    {
                        pos = content.Length;

                        Array.Resize(ref content, content.Length + read);
                        Array.Copy(buffer, 0, content, pos, read);

                        read = dfsDecompressor.Read(buffer, 0, 4096);
                    }
                }

                return content;
            }

            return new byte[0];
        }

        public void Dispose()
        {
            this._packageStream.Close();
        }
    }
}
