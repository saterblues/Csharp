using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Csharp.Liqing.Utils
{
    /// <summary>
    /// automatic increase pool
    /// if stack is empty , create a new T() instance to pop
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pool<T>  where T : new ()
    {
        #region private member
        private object m_lock = new object();
        private HashSet<T> set = new HashSet<T>();
        private ConcurrentStack<T> stack = new ConcurrentStack<T>(); 
        #endregion

        #region private functions
        private T StackPop()
        {
            T t;
            stack.TryPop(out t);
            return t;
        }

        private T GetOneInstace()
        {
            if (!stack.IsEmpty) { return StackPop(); }
            T result = CreateDataInstance();
            SetAddValue(result);
            return result;
        }

        private T CreateDataInstance() { return new T(); }

        private void SetAddValue(T t) { set.Add(t); } 
        #endregion

        #region public functions
        public void Clear()
        {
            set.Clear();
            stack.Clear();
        }

        public IEnumerable<T> GetList() { return set; }

        public int GetStackCount() { return stack.Count; }

        public int GetTotalCount() { return set.Count; }

        public T Pop()
        {
            lock (m_lock)
            {
                return GetOneInstace();
            }
        }

        public void Push(T t)
        {
            lock (m_lock)
            {
                if (!set.Contains(t) || stack.Contains(t)) { return; }
                stack.Push(t);
            }
        } 
        #endregion
    }
}
