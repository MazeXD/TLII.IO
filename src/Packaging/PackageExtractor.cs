/* Thanks to adidishen for his 
 * Torchlight II Plugin for
 * his Package Manager Salad
 * http://forums.runicgames.com/viewtopic.php?f=48&t=44052
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Text;

namespace TL2MH.IO.Packaging
{
    public class PackageExtractor
    {
        public static Package Open(string strPackPath)
        {
            Package pakPackage = null;
            
            if (strPackPath != null)
            {
                if (!File.Exists(strPackPath))
                {
                    strPackPath = null;
                }

                string strManifestPath = Path.ChangeExtension(strPackPath, ".PAK.MAN");

                if (strPackPath != null && !File.Exists(strManifestPath))
                {
                    MessageBox.Show("Unable to locate the coresponding\r\n" + 
                                    "manifest for this package",
                                    "TL2MH",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);

                    strManifestPath = null;
                }

                if (strPackPath != null && strManifestPath != null)
                {
                    List<PackageEntry> lstEntries = ReadManifest(strManifestPath);

                    Dictionary<string, FolderEntry> dctMap = new Dictionary<string, FolderEntry>();
                    FolderEntry feRoot =
                        new FolderEntry(Path.GetDirectoryName(lstEntries[0].Name), lstEntries[0]);
                    dctMap[lstEntries[0].Name] = feRoot;

                    for (int nCurrEntry = 1; nCurrEntry < lstEntries.Count; nCurrEntry++)
                    {
                        PackageEntry entCurr = lstEntries[nCurrEntry];

                        if (entCurr.Type == FileType.Directory)
                        {
                            FolderEntry feSubFolder = new FolderEntry(Path.GetDirectoryName(entCurr.Name), entCurr);

                            dctMap[entCurr.GroupName].SubFolders.Add(feSubFolder);

                            dctMap[entCurr.GroupName + entCurr.Name] = feSubFolder;
                        }
                        else
                        {
                            FileEntry feFile = new FileEntry(Path.GetFileName(entCurr.Name), entCurr);

                            dctMap[entCurr.GroupName].Files.Add(feFile);
                        }
                    }

                    pakPackage = new Package(strPackPath, feRoot);
                }
            }

            return pakPackage;
        }

        private static List<PackageEntry> ReadManifest(string strManifestPath)
        {
            List<PackageEntry> packageEntries = null;

            using (FileStream fsManifest = File.OpenRead(strManifestPath))
            {
                using (BinaryReader brManifest = new BinaryReader(fsManifest, Encoding.Unicode))
                {
                    brManifest.ReadUInt16();
                    string strRootName = ReadUnicodeString(brManifest);
                    int nTotalEntries = brManifest.ReadInt32();
                    int nEntryGroups = brManifest.ReadInt32();

                    packageEntries = new List<PackageEntry>(nTotalEntries + 1);

                    PackageEntry entry = new PackageEntry();
                    entry.Name = strRootName;
                    entry.Type = FileType.Directory;
                    packageEntries.Add(entry);

                    for (int nCurrGroup = 0; nCurrGroup < nEntryGroups; nCurrGroup++)
                    {
                        string strGroupName = ReadUnicodeString(brManifest);
                        int nGroupEntries = brManifest.ReadInt32();

                        for (int nCurrEntry = 0; nCurrEntry < nGroupEntries; nCurrEntry++)
                        {
                            entry = new PackageEntry();
                            entry.GroupName = strGroupName;
                            entry.Checksum = brManifest.ReadUInt32();
                            entry.Type = (FileType)brManifest.ReadByte();
                            entry.Name = ReadUnicodeString(brManifest);
                            entry.Offset = brManifest.ReadUInt32();
                            entry.UncompressedSize = brManifest.ReadUInt32();
                            entry.Timestamp = brManifest.ReadUInt64();
                            packageEntries.Add(entry);
                        }
                    }
                }
            }

            return packageEntries;
        }

        private static string ReadUnicodeString(BinaryReader brReader)
        {
            ushort nLength = brReader.ReadUInt16();

            return (new string(brReader.ReadChars(nLength)));
        }
    }
}
