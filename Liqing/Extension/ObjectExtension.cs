using System;
using System.Collections.Generic;
using System.Reflection;

namespace Csharp.Liqing.Extension
{
    public static class ObjectExtension
    {
        private static Dictionary<Func<PropertyInfo, bool>,Func<string, object>> ConvertConditionAndFunctions = InitConvertMatches();

        public static Dictionary<string,string> GetObejctKV(this Object obj) {
            Dictionary<string, string> kv = new Dictionary<string, string>();
            Type type = obj.GetType();
            PropertyInfo[]  infos = type.GetProperties();
            foreach (var item in infos)
            {
                object value = item.GetValue(obj, null);
                kv[item.Name] = value == null ? "Null" : value.ToString();
            }
            return kv;
        }
        public static void SetObjectValueByKV(this Object obj,Dictionary<string,string> keyValuePairs,bool ignoreSqlQuota = false)
        {
            Type type = obj.GetType();
            PropertyInfo[] infos = type.GetProperties();
            foreach (var info in infos)
            {
                SetPropertyValue(obj,info,keyValuePairs,ignoreSqlQuota);
            }
        }
        public static bool SetPropertyStringValue(Object obj, PropertyInfo info, Dictionary<string, string> kv, string key,bool ignoreSqlQuota) {
            if (!PropertyTypeMatch(info, "System.String")) { return false; }
            string value = kv[key];
            if (ignoreSqlQuota) { value = value.TrimSqlQuotation(); }
            info.SetValue(obj, value, null);
            return true;
        }
        public static void SetPropertyValue(Object obj, PropertyInfo info, Dictionary<string, string> kv,bool ignoreSqlQuota) {
            string key = info.Name.FindKeyInDictonaryIgnoreCase(kv);
            if (null == key) { return; }
            if (SetPropertyStringValue(obj, info, kv,key,ignoreSqlQuota)) { return; }
            SetPropertyValue(obj, info, kv, key);
        }
        public static void SetPropertyValue(Object obj, PropertyInfo info, Dictionary<string, string> kv, string key) {
            foreach (var ConditionAndFunction in ConvertConditionAndFunctions)
            {
                if (false == ConditionAndFunction.Key(info)) { continue; }
                info.SetValue(obj, ConditionAndFunction.Value(kv[key]), null);
                break;
            }
        }
        /// <summary>
        /// 不包含字符串转化
        /// </summary>
        /// <returns></returns>
        private static Dictionary<Func<PropertyInfo, bool>, Func<string, object>> InitConvertMatches() {
            if (ConvertConditionAndFunctions != null) { return ConvertConditionAndFunctions; }
            return new Dictionary<Func<PropertyInfo, bool>, Func<string, object>>() {
                { (info)=>{ return PropertyTypeMatch(info,"System.Int64"); },(value)=>{ return value.ToInt64(); }},
                { (info)=>{ return PropertyTypeMatch(info,"System.Int32"); },(value)=>{ return value.ToInt32(); }},
                { (info)=>{ return PropertyTypeMatch(info,"System.Int16"); },(value)=>{ return value.ToInt16(); }},
                { (info)=>{ return PropertyTypeMatch(info,"System.UInt64"); },(value)=>{ return value.ToUInt64(); }},
                { (info)=>{ return PropertyTypeMatch(info,"System.UInt32"); },(value)=>{ return value.ToUInt32(); }},
                { (info)=>{ return PropertyTypeMatch(info,"System.UInt16"); },(value)=>{ return value.ToUInt16(); }},
                { (info)=>{ return PropertyTypeMatch(info,"System.Byte"); },(value)=>{ return value.ToByte(); }},
                { (info)=>{ return PropertyTypeMatch(info,"System.SByte"); },(value)=>{ return value.ToSByte(); }},
                { (info)=>{ return PropertyTypeMatch(info,"System.Decimal"); },(value)=>{ return value.ToDecimal(); }},
                { (info)=>{ return PropertyTypeMatch(info,"System.Double"); },(value)=>{ return value.ToDouble(); }},
                { (info)=>{ return PropertyTypeMatch(info,"System.Single"); },(value)=>{ return value.ToSingle(); }},
                { (info)=>{ return PropertyTypeMatch(info,"System.Boolean"); },(value)=>{ return value.ToBoolean(); }},
                { (info)=>{ return PropertyTypeMatch(info,"System.DateTime"); },(value)=>{ return value.ToDatetime(); }}
            };
        }
        private static bool PropertyTypeMatch(PropertyInfo info,string text) {
            return info.PropertyType.ToString().EqualsIgnoreCase(text) || info.PropertyType.FullName.IndexOf(text) >= 0;
        }
    }
}
