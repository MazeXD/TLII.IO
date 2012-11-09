namespace TLII.IO.Formats.Adm
{
    public abstract class AdmField
    {
        public int NameHash;

        public abstract AdmFieldType Type { get; }

        public abstract object Value { get; set; }
    }
}
