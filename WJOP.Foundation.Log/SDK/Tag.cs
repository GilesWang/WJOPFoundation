using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Log.SDK
{
    [ProtoContract]
    public class Tag
    {
        [ProtoMember(3)]
        private float? FloatValue
        {
            get;
            set;
        }

        [ProtoMember(1)]
        private int? IntValue
        {
            get;
            set;
        }

        [ProtoMember(2)]
        private string StringValue
        {
            get;
            set;
        }

        internal object Dump()
        {
            object stringValue;
            if (!string.IsNullOrEmpty(this.StringValue))
            {
                stringValue = this.StringValue;
            }
            else if (!this.IntValue.HasValue)
            {
                stringValue = !this.FloatValue.HasValue ? null : this.FloatValue;
            }
            else
            {
                stringValue = this.IntValue.Value;
            }
            return stringValue;

        }

        public static implicit operator Tag(string value)
        {
            return new Tag()
            {
                StringValue = value
            };
        }

        public static implicit operator Tag(int value)
        {
            return new Tag()
            {
                IntValue = new int?(value)
            };
        }

        public static implicit operator Tag(float value)
        {
            return new Tag()
            {
                FloatValue = new float?(value)
            };
        }
    }
}
