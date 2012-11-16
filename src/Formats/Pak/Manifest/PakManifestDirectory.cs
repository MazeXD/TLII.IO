using System.Collections.Generic;

namespace TLII.IO.Formats.Pak.Manifest
{
    class PakManifestDirectory
    {
        public string Name;
        public List<PakManifestFile> Files = new List<PakManifestFile>();
    }
}
