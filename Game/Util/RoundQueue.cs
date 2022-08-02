using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.CsharpExtension;

namespace Game.Util
{
    /// <summary>
    /// 按照顺序的查询结构,当Pop查询至末尾时,再次Pop查询元素，又会从第一个开始查询
    /// 只有Remove会移除元素
    /// Pop只会移动内部current至下一个位置,不会移除元素
    /// </summary>
    public class RoundQueue<T>
    {
        private List<T> _round = new List<T>();
        private int _currentIndex = 0;
        
        /// <summary>
        /// Pop不会将数据移除，只是将Current移动到下一个位置
        /// </summary>
        /// <returns></returns>
        public T Pop() {
            if (_currentIndex >= _round.Count) { _currentIndex = 0; }
            T result = _round[_currentIndex];
            _currentIndex++;
            return result;
        }

        public T PeekCurrent() {
            if (_round.IsEmpty()) { return default(T); }
            return _round[_currentIndex];
        }

        public IEnumerable<T> PeekNextRange(int count) {
            if (_round.IsEmpty()) { return null; }

            List<T> result = new List<T>();

            int tmpIndex = _currentIndex;
            int range = _round.Count - tmpIndex;
            while (count > range) { 
                for(;tmpIndex < _round.Count;tmpIndex++){
                    result.Add(_round[tmpIndex]);
                }
                count -= range;
                tmpIndex = 0;
                range = _round.Count;
            }

            for (; tmpIndex < count; tmpIndex++) {
                result.Add(_round[tmpIndex]);
            }

            return result;
        }

        public T PeekNext() {
            if (_round.IsEmpty()) {  return default(T);}
            return _currentIndex < _round.Count - 1 ? _round[_currentIndex + 1] : _round[0];
        }

        public void Remove(T t) {
            _round.Remove(t);
        }

        public void Add(T t) {
            _round.Add(t);
        }
    }
}
