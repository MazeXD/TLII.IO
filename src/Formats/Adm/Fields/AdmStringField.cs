namespace TLII.IO.Formats.Adm.Fields
{
    public class AdmStringField : AdmField
    {
        private string _value;

        public override AdmFieldType Type
        {
            get { return AdmFieldType.String; }
        }

        public override object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value.GetType() == typeof(string))
                {
                    _value = (string)value;
                }
            }
        }
    }
}
