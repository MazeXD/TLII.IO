using System.Collections.Generic;
using TLII.IO.Formats.Pak.Manifest;

namespace TLII.IO.Formats.Pak
{
    class PakManifest
    {
        public string RootName;
        public List<PakManifestDirectory> Directories = new List<PakManifestDirectory>();
    }
}
