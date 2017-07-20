using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Cache.SDK
{
    internal class ParamValidate
    {
        public static bool CheckValue(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key) ? true : key.Length < 1)
            {
                throw new ArgumentException("Invalid cache key");
            }
            return true;
        }
        public static bool CheckKeyValue<T>(string key, T value)
        {
            CheckValue(key);
            CheckValue<T>(value);
            return true;
        }

        public static bool CheckValue<T>(T value)
        {
            bool flag = true;
            if (!(typeof(T) != typeof(string) ? true : value == null))
            {
                string str = value.ToString();
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str) ? true : str.Length < 1)
                {
                    flag= false;
                }
            }
            else if (value == null)
            {
                flag = false;
            }

            if (!flag)
            {
                throw new ArgumentException("Invalid cache value.");
            }
            return true;
        }
    }
}
