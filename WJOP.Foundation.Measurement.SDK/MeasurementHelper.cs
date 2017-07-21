using System;
using System.Collections.Generic;
using WJOP.Foundation.Common.Utility;
using WJOP.Foundation.Measurement.Common;

namespace WJOP.Foundation.Measurement.SDK
{
    public sealed class MeasurementHelper
    {
        private string _db;

        private string _rp;

        private string _metric;

        public MeasurementHelper(string metric)
        {
            ParamterUtil.CheckEmptyString("metric", metric);
            this.Init(metric, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public MeasurementHelper(string metric, string db, string rp = "default")
        {
            ParamterUtil.CheckEmptyString("metric", metric);
            ParamterUtil.CheckEmptyString("db", db);
            this.Init(metric, db, rp);
        }

        private static void BuildFields(List<Field> fieldList, string key, float value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                Field field = new Field()
                {
                    Name = key,
                    Value = value,
                    ValueType = FieldValueType.Float
                };
                fieldList.Add(field);
            }
        }

        private static void BuildFields(List<Field> fieldList, string key, string strValue)
        {
            if (!string.IsNullOrEmpty(key))
            {
                Field field = new Field()
                {
                    Name = key,
                    StrValue = strValue,
                    ValueType = FieldValueType.String
                };
                fieldList.Add(field);
            }
        }

        private static void BuildTags(List<Tag> tagList, string tagKey, string tagValue)
        {
            if ((string.IsNullOrEmpty(tagKey) ? false : !string.IsNullOrEmpty(tagValue)))
            {
                Tag tag = new Tag()
                {
                    Name = tagKey,
                    Value = tagValue
                };
                tagList.Add(tag);
            }
        }

        private void Init(string metric, string db, string rp)
        {
            this._db = db;
            this._rp = rp;
            this._metric = metric;
        }

        public MetricPoint MakePoint()
        {
            MetricPoint metricPoint = MakePoint(this._metric, this._db, this._rp);
            return metricPoint;
        }

        public static MetricPoint MakePoint(string metric)
        {
            string measurementDB = AppContext.MeasurementDB;
            return MakePoint(metric, measurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static MetricPoint MakePoint(string metric, string db, string rp = "default")
        {
            MetricPoint metricPoint = new MetricPoint(metric, db, rp);
            metricPoint.OnSubmit += new Action<MetricPoint>(WritePoint);
            return metricPoint;
        }

        public static MeasurementScope NewScope(string methodName, IDictionary<string, string> tags = null)
        {
            return new MeasurementScope(methodName, tags);
        }

        public static List<MetricSerie> ReadPoints(string query)
        {
            ParamterUtil.CheckEmptyString("query", query);
            string measurementDB = AppContext.MeasurementDB;
            if (string.IsNullOrEmpty(measurementDB))
            {
                throw new ArgumentException("MeasurementDB missing from config file.");
            }
            return ReadPoints(measurementDB, query);
        }

        public static List<MetricSerie> ReadPoints(string db, string query)
        {
            ParamterUtil.CheckEmptyString("db", db);
            ParamterUtil.CheckEmptyString("query", query);
            List<MetricSerie> metricSeries = null;
            try
            {
                metricSeries = MeasurementHost.Instance.PerformQuery(db, query);
                if (metricSeries == null)
                {
                    metricSeries = new List<MetricSerie>();
                }
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
            }
            return metricSeries;
        }

        public static void WritePoint(MetricPoint point)
        {
            if (AppContext.MeasurementEnabled)
            {
                MeasurementHost.Instance.Submit(point);
            }
        }

        public static void WritePoint(string metric, string key, float value)
        {
            Dictionary<string, float> strs = new Dictionary<string, float>()
            {
                { key, value }
            };
            WritePoint(metric, strs, null, DateTime.UtcNow, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, string key, string value)
        {
            Dictionary<string, string> strs = new Dictionary<string, string>()
            {
                { key, value }
            };
            WritePoint(metric, strs, null, DateTime.UtcNow, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, string key, float value, string tagKey, string tagValue)
        {
            Dictionary<string, float> strs = new Dictionary<string, float>()
            {
                { key, value }
            };
            Dictionary<string, string> strs1 = new Dictionary<string, string>()
            {
                { tagKey, tagValue }
            };
            WritePoint(metric, strs, strs1, DateTime.UtcNow, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, string key, string value, string tagKey, string tagValue)
        {
            Dictionary<string, string> strs = new Dictionary<string, string>()
            {
                { key, value }
            };
            Dictionary<string, string> strs1 = new Dictionary<string, string>()
            {
                { tagKey, tagValue }
            };
            WritePoint(metric, strs, strs1, DateTime.UtcNow, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, string key, float value, IDictionary<string, string> tags)
        {
            Dictionary<string, float> strs = new Dictionary<string, float>()
            {
                { key, value }
            };
            WritePoint(metric, strs, tags, DateTime.UtcNow, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, string key, string value, IDictionary<string, string> tags)
        {
            Dictionary<string, string> strs = new Dictionary<string, string>()
            {
                { key, value }
            };
            WritePoint(metric, strs, tags, DateTime.UtcNow, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, IDictionary<string, float> fields)
        {
            WritePoint(metric, fields, null, DateTime.UtcNow, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, IDictionary<string, string> fields)
        {
            WritePoint(metric, fields, null, DateTime.UtcNow, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, IDictionary<string, object> fields)
        {
            WritePoint(metric, fields, null, DateTime.UtcNow, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, IDictionary<string, float> fields, string tagKey, string tagValue)
        {
            Dictionary<string, string> strs = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(tagKey))
            {
                strs.Add(tagKey, tagValue);
            }
            WritePoint(metric, fields, strs, DateTime.UtcNow, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, IDictionary<string, string> fields, string tagKey, string tagValue)
        {
            Dictionary<string, string> strs = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(tagKey))
            {
                strs.Add(tagKey, tagValue);
            }
            WritePoint(metric, fields, strs, DateTime.UtcNow, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, IDictionary<string, float> fields, IDictionary<string, string> tags)
        {
            WritePoint(metric, fields, tags, DateTime.UtcNow, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, IDictionary<string, string> fields, IDictionary<string, string> tags)
        {
            WritePoint(metric, fields, tags, DateTime.UtcNow, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, IDictionary<string, float> fields, IDictionary<string, string> tags, DateTime timestampInUTC)
        {
            WritePoint(metric, fields, tags, timestampInUTC, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, IDictionary<string, string> fields, IDictionary<string, string> tags, DateTime timestampInUTC)
        {
            WritePoint(metric, fields, tags, timestampInUTC, AppContext.MeasurementDB, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, IDictionary<string, float> fields, IDictionary<string, string> tags, DateTime timestampInUTC, string db)
        {
            WritePoint(metric, fields, tags, timestampInUTC, db, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, IDictionary<string, string> fields, IDictionary<string, string> tags, DateTime timestampInUTC, string db)
        {
            WritePoint(metric, fields, tags, timestampInUTC, db, AppContext.MeasurementDBRetentionPolicy);
        }

        public static void WritePoint(string metric, IDictionary<string, float> fields, IDictionary<string, string> tags, DateTime timestampInUTC, string db, string rp)
        {
            ParamterUtil.CheckEmptyString("metric", metric);
            ParamterUtil.CheckEmptyString("db", db);
            ParamterUtil.CheckEmptyString("rp", rp);
            if (AppContext.MeasurementEnabled)
            {
                if ((fields == null ? false : fields.Count > 0))
                {
                    MetricPoint metricPoint = new MetricPoint(metric, db, rp);
                    foreach (KeyValuePair<string, float> field in fields)
                    {
                        BuildFields(metricPoint.Fields, field.Key, field.Value);
                    }
                    if ((tags == null ? false : tags.Count > 0))
                    {
                        foreach (KeyValuePair<string, string> tag in tags)
                        {
                            BuildTags(metricPoint.Tags, tag.Key, tag.Value);
                        }
                    }
                    metricPoint.TimeStamp = timestampInUTC.ToString("yyyy/MM/dd HH:mm:ss.ffffff");
                    MeasurementHost.Instance.Submit(metricPoint);
                }
            }
        }

        public static void WritePoint(string metric, IDictionary<string, string> fields, IDictionary<string, string> tags, DateTime timestampInUTC, string db, string rp)
        {
            ParamterUtil.CheckEmptyString("metric", metric);
            ParamterUtil.CheckEmptyString("db", db);
            ParamterUtil.CheckEmptyString("rp", rp);
            if (AppContext.MeasurementEnabled)
            {
                if ((fields == null ? false : fields.Count > 0))
                {
                    MetricPoint metricPoint = new MetricPoint(metric, db, rp);
                    foreach (KeyValuePair<string, string> field in fields)
                    {
                        BuildFields(metricPoint.Fields, field.Key, field.Value);
                    }
                    if ((tags == null ? false : tags.Count > 0))
                    {
                        foreach (KeyValuePair<string, string> tag in tags)
                        {
                            BuildTags(metricPoint.Tags, tag.Key, tag.Value);
                        }
                    }
                    metricPoint.TimeStamp = timestampInUTC.ToString("yyyy/MM/dd HH:mm:ss.ffffff");
                    MeasurementHost.Instance.Submit(metricPoint);
                }
            }
        }

        public static void WritePoint(string metric, IDictionary<string, object> fields, IDictionary<string, string> tags, DateTime timestampInUTC, string db, string rp)
        {
            ParamterUtil.CheckEmptyString("metric", metric);
            ParamterUtil.CheckEmptyString("db", db);
            ParamterUtil.CheckEmptyString("rp", rp);
            if (AppContext.MeasurementEnabled)
            {
                if ((fields == null ? false : fields.Count > 0))
                {
                    MetricPoint metricPoint = new MetricPoint(metric, db, rp);
                    foreach (KeyValuePair<string, object> field in fields)
                    {
                        if (field.Value == null)
                        {
                            BuildFields(metricPoint.Fields, field.Key, "TGOPInfluxDBNull");
                        }
                        else if (field.Value is string)
                        {
                            BuildFields(metricPoint.Fields, field.Key, field.Value as string);
                        }
                        else if (field.Value.GetType() == typeof(float))
                        {
                            BuildFields(metricPoint.Fields, field.Key, (float)field.Value);
                        }
                        else if (field.Value.GetType() == typeof(int))
                        {
                            BuildFields(metricPoint.Fields, field.Key, (float)((int)field.Value));
                        }
                        else if (field.Value.GetType() == typeof(double))
                        {
                            BuildFields(metricPoint.Fields, field.Key, (float)((double)field.Value));
                        }
                    }
                    if ((tags == null ? false : tags.Count > 0))
                    {
                        foreach (KeyValuePair<string, string> tag in tags)
                        {
                            BuildTags(metricPoint.Tags, tag.Key, tag.Value);
                        }
                    }
                    metricPoint.TimeStamp = timestampInUTC.ToString("yyyy/MM/dd HH:mm:ss.ffffff");
                    MeasurementHost.Instance.Submit(metricPoint);
                }
            }
        }
    }
}
