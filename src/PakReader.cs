using System;
using System.IO;
using System.Text;
using TLII.IO.Formats.Pak;
using TLII.IO.Formats.Pak.Manifest;
using TLII.IO.Exceptions;

namespace TLII.IO
{
    public class PakReader : IDisposable
    {
        private MemoryStream _stream;
        private BinaryReader _reader;
        private Encoding _encoding = Encoding.GetEncoding(1200);
        private bool _disposed = false;
        private string _archiveFile;

        public PakReader(string fileName)
        {
            string manifestFile = "";

            if (Path.GetExtension(fileName).Remove(0, 1).ToUpperInvariant() == "MAN")
            {
                _archiveFile = Path.GetFileNameWithoutExtension(fileName);
                manifestFile = fileName;
            }
            else
            {
                _archiveFile = fileName;
                manifestFile = fileName + ".man";
            }

            if (!File.Exists(_archiveFile))
            {
                throw new FileNotFoundException("Can't find archive file.", _archiveFile);
            }

            if (!File.Exists(manifestFile))
            {
                throw new FileNotFoundException("Can't find manifest file", manifestFile);
            }

            byte[] data = File.ReadAllBytes(manifestFile);

            SetupStream(data);
        }

        public PakReader(byte[] data)
        {
            SetupStream(data);
        }

        public PakFile Read()
        {
            PakFile data = new PakFile();
            try
            {
                var manifest = ReadManifest();
                var archiveStream = File.OpenRead(_archiveFile);

                foreach (var dir in manifest.Directories)
                {
                    foreach (var file in dir.Files)
                    {
                        if (file.Type != PakFileType.Directory)
                        {
                            data.Entries.Add(new PakEntry(archiveStream, dir.Name, file));
                        }
                    }
                }
            }
            catch(Exception e)
            {
                throw new PakReaderException("Failed to parse manifest!", e);
            }

            return data;
        }

        #region PRIVATE METHODS

        private void SetupStream(byte[] data)
        {
            _stream = new MemoryStream(data);
            _reader = new BinaryReader(_stream);
        }

        private PakManifest ReadManifest()
        {
            PakManifest manifest = new PakManifest();

            _stream.Position += 2;
            manifest.RootName = ReadString();

            _stream.Position += 4;

            uint dirCount = _reader.ReadUInt32();
            for (int i = 0; i < dirCount; i++)
            {
                manifest.Directories.Add(ReadDirectory());
            }

            return manifest;
        }

        private PakManifestDirectory ReadDirectory()
        {
            var directory = new PakManifestDirectory();

            directory.Name = ReadString();

            uint fileCount = _reader.ReadUInt32();
            for (int i = 0; i < fileCount; i++)
            {
                directory.Files.Add(ReadFile());
            }

            return directory;
        }

        private PakManifestFile ReadFile()
        {
            var file = new PakManifestFile();

            file.Checksum = _reader.ReadUInt32();
            
            byte typeByte = _reader.ReadByte();
            PakFileType type;
            if(!Enum.TryParse<PakFileType>(((int)typeByte).ToString(), out type))
            {
                throw new InvalidDataException(string.Format("Invalid file type! (Type: {0})", (int)typeByte));
            }

            file.Type = type;
            file.Name = ReadString();
            file.Offset = _reader.ReadUInt32();
            file.UncompressedSize = _reader.ReadUInt32();
            file.Timestamp = _reader.ReadUInt64();

            return file;
        }

        private string ReadString()
        {
            int length = _reader.ReadInt16() * 2;
            byte[] data = _reader.ReadBytes(length);
            return _encoding.GetString(data);
        }

        #endregion


        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                _reader.Close();
                _stream.Close();
                _reader.Dispose();
                _stream.Dispose();
            }
        }
    }
}
