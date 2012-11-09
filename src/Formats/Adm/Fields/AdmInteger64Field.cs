namespace TLII.IO.Formats.Adm.Fields
{
    public class AdmInteger64Field : AdmField
    {
        private long _value;

        public override AdmFieldType Type
        {
            get { return AdmFieldType.Integer64; }
        }

        public override object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value.GetType() == typeof(long))
                {
                    _value = (long)value;
                }
            }
        }
    }
}
