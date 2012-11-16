namespace TLII.IO.Formats.Pak.Manifest
{
    class PakManifestFile
    {
        public string Name;
        public PakFileType Type;
        //public PakManifestDirectory Parent;

        public uint Offset;
        public uint UncompressedSize;
        public uint Checksum;
        public ulong Timestamp;
    }
}
