using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Log.Common
{
    [ProtoContract]
    internal class LogMessage
    {
        [ProtoMember(1)]
        public string Appkey
        {
            get;
            set;
        }

        [ProtoMember(5)]
        public List<LogContent> Body
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public string ComputerName
        {
            get;
            set;
        }

        [ProtoMember(2)]
        public string IP
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public string Mac
        {
            get;
            set;
        }



        [ProtoMember(6)]
        public string Version
        {
            get
            {
                return "1.4";
            }
            set
            {
            }
        }
    }
}
