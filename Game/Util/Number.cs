using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    /// <summary>
    /// 自建Number类,建议所有数值类型都用Number
    /// </summary>
    public class Number
    {
        public static readonly Number NotNumber = new Number(float.MinValue);
        public Number(float value) {
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
        public static bool operator !=(Number left, Number right)
        {
            return left._value != right._value;
        }
        public static bool operator <(Number left, Number right) {
            return left._value < right._value;
        }
        public static bool operator <=(Number left, Number right) {
            return left._value <= right._value;
        }
        public static bool operator ==(Number left, Number right) {
            return left._value == right._value;
        }
        public static bool operator >(Number left, Number right) {
            return left._value > right._value;
        }
        public static bool operator >=(Number left, Number right) {
            return left._value >= right._value;
        }
        public static Number operator +(Number left, Number right) {
            return new Number(left._value + right.Value);
        }
        public static Number operator -(Number left, Number right)
        {
            return new Number(left._value - right.Value);
        }
        public static Number operator *(Number left, Number right)
        {
            return new Number(left._value * right.Value);
        }
        public static Number operator /(Number left, Number right)
        {
            return new Number(left._value / right.Value);
        }
        public static implicit operator float(Number number) { return number._value; }
        public static implicit operator Number(float value) { return new Number(value); }
        public static implicit operator int(Number number) { return (int)number._value; }
        public static implicit operator Number(int value) { return new Number(value); }
        public static implicit operator double(Number number) { return (double)number._value; }
        public static implicit operator Number(double value) { return new Number((float)value); }
    }
}
