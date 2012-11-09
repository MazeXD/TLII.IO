namespace TLII.IO.Formats.Adm.Fields
{
    public class AdmBooleanField : AdmField
    {
        private bool _value;

        public override AdmFieldType Type
        {
            get { return AdmFieldType.Boolean; }
        }

        public override object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value.GetType() == typeof(bool))
                {
                    _value = (bool)value;
                }
            }
        }
    }
}
