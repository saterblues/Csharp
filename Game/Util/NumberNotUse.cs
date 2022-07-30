using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    /// <summary>
    /// 自建Number类,已经放弃使用
    /// </summary>
    public class NumberNotUse
    {
        public static readonly NumberNotUse NotNumber = new NumberNotUse(float.MinValue);
        public NumberNotUse(float value) {
            this._value = value;
        }
        private float _value {get;set;}
        public float Value { 
            get {
                return _value;
            }
            set {
                this._value = value;
            }
        }

        public override bool Equals(object obj)
        {
            return _value.Equals(obj);
        }
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
        public static bool operator !=(NumberNotUse left, NumberNotUse right)
        {
            return left._value != right._value;
        }
        public static bool operator <(NumberNotUse left, NumberNotUse right) {
            return left._value < right._value;
        }
        public static bool operator <=(NumberNotUse left, NumberNotUse right) {
            return left._value <= right._value;
        }
        public static bool operator ==(NumberNotUse left, NumberNotUse right) {
            return left._value == right._value;
        }
        public static bool operator >(NumberNotUse left, NumberNotUse right) {
            return left._value > right._value;
        }
        public static bool operator >=(NumberNotUse left, NumberNotUse right) {
            return left._value >= right._value;
        }
        public static NumberNotUse operator +(NumberNotUse left, NumberNotUse right) {
            return new NumberNotUse(left._value + right.Value);
        }
        public static NumberNotUse operator -(NumberNotUse left, NumberNotUse right)
        {
            return new NumberNotUse(left._value - right.Value);
        }
        public static NumberNotUse operator *(NumberNotUse left, NumberNotUse right)
        {
            return new NumberNotUse(left._value * right.Value);
        }
        public static NumberNotUse operator /(NumberNotUse left, NumberNotUse right)
        {
            return new NumberNotUse(left._value / right.Value);
        }
        public static implicit operator float(NumberNotUse number) { return number._value; }
        public static implicit operator NumberNotUse(float value) { return new NumberNotUse(value); }
        public static implicit operator int(NumberNotUse number) { return (int)number._value; }
        public static implicit operator NumberNotUse(int value) { return new NumberNotUse(value); }
        public static implicit operator double(NumberNotUse number) { return (double)number._value; }
        public static implicit operator NumberNotUse(double value) { return new NumberNotUse((float)value); }
    }
}
