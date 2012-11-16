namespace TL2MH.IO.Packaging
{
    public class FileEntry
    {
        public readonly string Name;
        public readonly object Tag;

        public FileEntry(string fileName, object tag)
        {
            Name = fileName;
            Tag = tag;
        }
    }
}
