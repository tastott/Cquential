using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Core.Expressions
{
    public class ConstantExpression<T> : IExpression<T>
    {
        private double _value;

        public ConstantExpression(double value)
        {
            _value = value;
        }

        public Type ReturnType { get { return typeof(double); } }

        public bool GetBoolValue(IMatchCandidate<T> context)
        {
            throw new Exception("ConstantExpression cannot return a value of type bool");
        }

        public double GetNumericValue(IMatchCandidate<T> context)
        {
            return _value;
        }

        public NumericMutability GetNumericMutability()
        {
            return NumericMutability.Fixed;
        }

        public bool IsBooleanMutable(IMatchCandidate<T> context)
        {
            throw new Exception("ConstantExpression cannot return a value of type bool");
        }

        public override bool Equals(object obj)
        {
            var that = obj as ConstantExpression<T>;

            if (that != null) return this._value == that._value;
            else return false;
        }


        public Tuple<bool, bool> GetBoolStatus(IMatchCandidate<T> context)
        {
            throw new Exception("ConstantExpression cannot return a value of type bool");
        }
    }
}
