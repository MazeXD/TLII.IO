using System.Collections.Generic;

namespace TL2MH.IO.Packaging
{
    public class FolderEntry
    {
        public readonly string Name;
        public readonly object Tag;

        public List<FolderEntry> SubFolders = new List<FolderEntry>();
        public List<FileEntry> Files = new List<FileEntry>();


        public FolderEntry(string dirName, object tag){
            Name = dirName;
            Tag = tag;
        }
    }
}
