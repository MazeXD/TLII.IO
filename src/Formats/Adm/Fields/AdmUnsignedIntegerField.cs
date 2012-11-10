namespace TLII.IO.Formats.Adm.Fields
{
    public class AdmUnsignedIntegerField : AdmField
    {
        private uint _value;

        public override AdmFieldType Type
        {
            get { return AdmFieldType.UnsignedInteger; }
        }

        public override object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value is uint)
                {
                    _value = (uint)value;
                }
            }
        }
    }
}
