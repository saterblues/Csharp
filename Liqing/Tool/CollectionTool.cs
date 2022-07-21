using System;
using System.Collections.Generic;
using System.Text;

namespace Csharp.Liqing.Tool
{
    public class CollectionTool
    {
        public static Dictionary<KEY, VALUE> MakeDictionary<KEY, VALUE>(IEnumerable<KEY> keys, IEnumerable<VALUE> values) {
            HashSet<KEY> k = new HashSet<KEY>();
            Dictionary<KEY, VALUE> kv = new Dictionary<KEY, VALUE>();
            var key = keys.GetEnumerator();
            var value = values.GetEnumerator();
            while (key.MoveNext() && value.MoveNext()) {
                if (false == k.Add(key.Current)) { throw new Exception(string.Format("key:{0} has more than one", key.Current)); }
                kv.Add(key.Current, value.Current);
            }

            if (key.MoveNext() || value.MoveNext()) { throw new Exception("keys count not equal values count"); }
            return kv;
        }
    }
}
