namespace TLII.IO.Formats.Adm.Fields
{
    public class AdmIntegerField : AdmField
    {
        private int _value;

        public override AdmFieldType Type
        {
            get { return AdmFieldType.Integer; }
        }

        public override object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value.GetType() == typeof(int))
                {
                    _value = (int)value;
                }
            }
        }
    }
}
