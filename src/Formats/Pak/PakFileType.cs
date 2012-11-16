namespace TLII.IO.Formats.Pak
{
    public enum PakFileType : int
    {
        DataFile = 0,
        Layout,
        Mesh,
        Skeleton,
        DDS,
        PNG,
        Audio,
        Directory,
        Material,
        Raw,
        Imageset = 11,
        TTF,
        Font,
        Animation = 16,
        Hie,
        Scheme = 19,
        LookNFeel,
        MPP
    }
}
