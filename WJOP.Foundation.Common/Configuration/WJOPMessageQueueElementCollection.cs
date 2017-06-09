using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Common.Configuration
{
    public class WJOPMessageQueueElementCollection : ConfigurationElementCollection
    {
        public WJOPMessageQueueElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as WJOPMessageQueueElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        public new WJOPMessageQueueElement this[string name]
        {
            get
            {
                return (WJOPMessageQueueElement)base.BaseGet(name);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new WJOPMessageQueueElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WJOPMessageQueueElement)element).ConnectionName;
        }
    }
}
