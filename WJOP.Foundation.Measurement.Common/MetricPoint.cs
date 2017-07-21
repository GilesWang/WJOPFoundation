using System;
using System.Collections.Generic;

namespace WJOP.Foundation.Measurement.Common
{
    public class MetricPoint
    {
        public string DB { get; set; }
        public string Metric { get; set; }
        public string RP { get { return CombineDBRP(DB, RP); } private set { } }
        public List<Tag> Tags { get; private set; }
        public string TimeStamp { get; set; }
        public string Version { get { return MMConstant.SDKVersion; } }
        public bool IsValid
        {
            get
            {
                return ((string.IsNullOrEmpty(this.DB) || string.IsNullOrEmpty(this.RP) || string.IsNullOrEmpty(this.Metric) ? true : this.Fields.Count <= 0) ? false : true);
            }
        }
        public List<Field> Fields { get; private set; }
        public event Action<MetricPoint> OnSubmit;

        public static string CombineDBRP(string dB, string rP)
        {
            return string.Format("{0}.{1}", dB, rP);
        }
        public MetricPoint()
        {
            Init();
        }
        public MetricPoint(string metric, string db, string rp = "default")
        {
            Init();
            Metric = metric;
            DB = db;
            RP = rp;
        }
        public MetricPoint AddField(string fieldName, int fieldValue)
        {
            Field field = new Field()
            {
                Name = fieldName,
                Value = (float)fieldValue,
                ValueType = FieldValueType.Int
            };
            Fields.Add(field);
            return this;
        }
        public MetricPoint AddField(string fieldName, float fieldValue)
        {
            Field field = new Field()
            {
                Name = fieldName,
                Value = fieldValue,
                ValueType = FieldValueType.Float
            };
            Fields.Add(field);
            return this;
        }

        public MetricPoint AddField(string fieldName, double fieldValue)
        {
            Field field = new Field()
            {
                Name = fieldName,
                Value = (float)fieldValue,
                ValueType = FieldValueType.Float
            };
            Fields.Add(field);
            return this;
        }

        public MetricPoint AddField(string fieldName, string fieldValue)
        {
            Field field = new Field()
            {
                Name = fieldName,
                StrValue = fieldValue,
                ValueType = FieldValueType.String
            };
            Fields.Add(field);
            return this;
        }
        public MetricPoint AddTag(string tagName, string tagValue)
        {
            Tag tag = new Tag()
            {
                Name = tagName,
                Value = tagValue
            };
            Tags.Add(tag);
            return this;
        }
        public MetricPoint At(DateTime date)
        {
            TimeStamp = date.ToString("yyyy/MM/dd HH:mm:ss.ffffff");
            return this;
        }
        private void Init()
        {
            Fields = new List<Field>();
            Tags = new List<Tag>();
            TimeStamp = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss.ffffff");
        }
        public static string[] ParseDBRP(string dbrp)
        {
            string[] strArrays = new string[] { "", "" };
            if (!string.IsNullOrEmpty(dbrp))
            {
                string[] strArrays1 = dbrp.Split(".".ToCharArray());
                if ((int)strArrays1.Length == 2)
                {
                    strArrays[0] = strArrays1[0];
                    strArrays[1] = strArrays1[1];
                }
            }
            return strArrays;
        }
        public void Submit()
        {
            if (OnSubmit != null)
            {
                OnSubmit(this);
            }
        }
    }
}
