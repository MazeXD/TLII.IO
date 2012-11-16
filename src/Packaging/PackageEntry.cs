/* Thanks to adidishen for his 
 * Torchlight II Plugin for
 * his Package Manager Salad
 * http://forums.runicgames.com/viewtopic.php?f=48&t=44052
 */


using System.IO;
using System.Runtime.InteropServices;

namespace TL2MH.IO.Packaging
{
    public class PackageEntry
    {
        public uint Checksum { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }
        public uint Offset { get; set; }
        public ulong Timestamp { get; set; }
        public FileType Type { get; set; }
        public uint UncompressedSize { get; set; }

        internal static FileType GetEntryType(string strFile)
        {
            string strExt = Path.GetExtension(strFile).ToLower();

            FileType etRet;

            switch (strExt)
            {
                case ".layout":
                {
                    etRet = FileType.Layout;
                    break;
                }

                case ".mesh":
                {
                    etRet = FileType.Mesh;
                    break;
                }

                case ".skeleton":
                {
                    etRet = FileType.Skeleton;
                    break;
                }

                case ".dds":
                {
                    etRet = FileType.Dds;
                    break;
                }

                case ".png":
                {
                    etRet = FileType.Png;
                    break;
                }

                case ".ogg":
                {
                    etRet = FileType.Ogg;
                    break;
                }

                case ".material":
                {
                    etRet = FileType.Material;
                    break;
                }

                case ".raw":
                {
                    etRet = FileType.Raw;
                    break;
                }

                case ".imageset":
                {
                    etRet = FileType.ImageSet;
                    break;
                }

                case ".ttf":
                {
                    etRet = FileType.Ttf;
                    break;
                }

                case ".font":
                {
                    etRet = FileType.Font;
                    break;
                }

                case ".animation":
                {
                    etRet = FileType.Animation;
                    break;
                }

                case ".hie":
                {
                    etRet = FileType.Hie;
                    break;
                }

                case ".scheme":
                {
                    etRet = FileType.Scheme;
                    break;
                }

                case ".looknfeel":
                {
                    etRet = FileType.LookNFeel;
                    break;
                }

                case ".mpd":
                case ".mpp":
                {
                    etRet = FileType.Mpd;
                    break;
                }

                default:
                {
                    etRet = FileType.Dat;
                    break;
                }
            }

            return (etRet);
        }
    }
}