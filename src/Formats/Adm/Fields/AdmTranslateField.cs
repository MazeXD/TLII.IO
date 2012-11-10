namespace TLII.IO.Formats.Adm.Fields
{
    public class AdmTranslateField : AdmField
    {
        private string _value;

        public override AdmFieldType Type
        {
            get { return AdmFieldType.Translate; }
        }

        public override object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value is string)
                {
                    _value = (string)value;
                }
            }
        }
    }
}
