namespace TL2MH.IO.Packaging
{
    public enum FileType : byte
    {
        Dat = 0x00,
        Layout = 0x01,
        Mesh = 0x02,
        Skeleton = 0x03,
        Dds = 0x04,
        Png = 0x05,
        Ogg = 0x06,
        Directory = 0x07,
        Material = 0x08,
        Raw = 0x09,
        ImageSet = 0x0B,
        Ttf = 0x0C,
        Font = 0x0D,
        Animation = 0x10,
        Hie = 0x11,
        Scheme = 0x12,
        LookNFeel = 0x13,
        Mpd = 0x14,
    }
}
