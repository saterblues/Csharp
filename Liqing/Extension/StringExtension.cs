using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Csharp.Liqing.Extension
{
    public static partial class StringExtension
    {
        /// <summary>
        ///  使用详细请查看函数实现 bool ToBoolean(this string value) 
        /// </summary>
        public static HashSet<string> trueValues = new HashSet<string>() { "yes", "true", "on", "right", "1" };
        public static HashSet<string> falseValues = new HashSet<string>() { "no", "not", "false", "null", "off", "wrong", "0" };
        public static void SetAsTrue(this string value) {trueValues.Add(value); }
        public static void SetAsFalse(this string value) {falseValues.Add(value);}
        public static void RemoveFromTrue(this string value) { trueValues.Remove(value); }
        public static void RemoveFromFalse(this string value) { falseValues.Remove(value); }

        public static string[] FindMid(this string value, string leftFlag, string rightFlag) {
            List<string> list = new List<string>();
            int leftIndex = -1;
            int rightIndex = -1;
            leftIndex = value.IndexOf(leftFlag);
            while (leftIndex != -1) {
                rightIndex = value.IndexOf(rightFlag, leftIndex + leftFlag.Length);
                if (rightIndex == -1) { break; }
                list.Add(value.Substring(leftIndex + leftFlag.Length, rightIndex - leftIndex - rightFlag.Length));
                leftIndex = value.IndexOf(leftFlag, rightIndex + rightFlag.Length);
            }
            return list.Count == 0 ? null : list.ToArray();
        }
        public static string[] FindMid(this string value, char leftFlag, char rightFlag) {
            return value.FindMid(leftFlag.ToString(), rightFlag.ToString());
        }

        public static long ToInt64(this string value) {
            return Convert.ToInt64(value); ;
        }
        public static int ToInt32(this string value) {
            return Convert.ToInt32(value);
        }
        public static Int16 ToInt16(this string value)
        {
            return Convert.ToInt16(value);
        }
        public static ulong ToUInt64(this string value)
        {
            return Convert.ToUInt64(value);
        }
        public static uint ToUInt32(this string value)
        {
            return Convert.ToUInt32(value);
        }
        public static UInt16 ToUInt16(this string value)
        {
            return Convert.ToUInt16(value);
        }
        public static float ToFloat(this string value) {
            return value.ToSingle();
        }
        public static float ToSingle(this string value) {
            return Convert.ToSingle(value);
        }
        public static Double ToDouble(this string value)
        {
            return Convert.ToDouble(value);
        }
        public static Decimal ToDecimal(this string value) {
            return Convert.ToDecimal(value);
        }
        public static byte ToByte(this string value) {
            return Convert.ToByte(value);
        }
        public static sbyte ToSByte(this string value)
        {
            return Convert.ToSByte(value);
        }
        public static bool ToBoolean(this string value)
        {
            foreach (var item in trueValues)
            {
                if (item.EqualsIgnoreCase(value)) { return true; }
            }
            foreach (var item in falseValues)
            {
                if (item.EqualsIgnoreCase(value)) { return false; }
            }
            return Convert.ToBoolean(value);
        }
        public static string TrimSqlQuotation(this string value) {
            return value.Trim('\'', '’', '‘');
        }
        public static DateTime ToDatetime(this string value)
        {
            DateTime result;
            DateTime.TryParse(value.TrimSqlQuotation(), out result);
            return result;
        }
        public static string FindKeyInCollectionIgnoreCase(this string value, IEnumerable<string> collection) {
            foreach (var item in collection)
            {
                if (value.EqualsIgnoreCase(item)) { return item; }
            }
            return null;
        }
        public static string FindKeyInDictonaryIgnoreCase<T>(this string value,Dictionary<string,T> collection) {
            foreach (var item in collection)
            {
                if (value.EqualsIgnoreCase(item.Key)) { return item.Key; }
            }
            return null;
        }
        public static bool EqualsIgnoreCase(this string value, string compare,bool ignore = true) {
            return ignore ? value.Equals(compare, StringComparison.OrdinalIgnoreCase) : value.Equals(compare);
        }

    }
}
