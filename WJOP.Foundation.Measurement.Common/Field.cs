using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Measurement.Common
{
    public class Field
    {
        public int IntValue { get; set; }
        public string Name { get; set; }
        public string StrValue { get; set; }
        public float Value { get; set; }
        public FieldValueType ValueType { get; set; }
        public static implicit operator Field(string value)
        {
            return new Field()
            {
                StrValue = value,
                ValueType = FieldValueType.String
            };
        }
        public static implicit operator Field(int value)
        {
            return new Field() {
                Value=(float)value,
                ValueType=FieldValueType.Int
            };
        }
        public static implicit operator Field(float value)
        {
            return new Field()
            {
                Value = value,
                ValueType = FieldValueType.Float
            };
        }
    }
}
