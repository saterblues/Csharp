using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.CsharpExtension
{
    public static partial class ICollectionExtension
    {
        public static bool IsEmpty<T>(this ICollection<T> collection) {
            return collection.Count == 0;
        }
    }
}
