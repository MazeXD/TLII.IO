namespace TLII.IO.Formats.Adm.Fields
{
    public class AdmFloatField : AdmField
    {
        private float _value;

        public override AdmFieldType Type
        {
            get { return AdmFieldType.Float; }
        }

        public override object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value.GetType() == typeof(float))
                {
                    _value = (float)value;
                }
            }
        }
    }
}
