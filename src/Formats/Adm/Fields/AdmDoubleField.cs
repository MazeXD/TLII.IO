namespace TLII.IO.Formats.Adm.Fields
{
    public class AdmDoubleField : AdmField
    {
        private double _value;

        public override AdmFieldType Type
        {
            get { return AdmFieldType.Double; }
        }

        public override object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value is double)
                {
                    _value = (double)value;
                }
            }
        }
    }
}
