using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Core.Expressions
{
    public abstract class BinaryOperationExpression<T>: IExpression<T>
    {
        private Func<object, object, object> _operationFunc;
        private Type _operandType;
        protected IExpression<T> _left;
        protected IExpression<T> _right;

        public BinaryOperationExpression(Type returnType, Type operandType, Func<object, object, object> operationFunc,
            IExpression<T> left, IExpression<T> right)
        {
            _operationFunc = operationFunc;
            _operandType = operandType;
            _left = left;
            _right = right;

            ReturnType = returnType;
        }

        public Type ReturnType { get; private set; }

        public bool GetBoolValue(IMatchCandidate<T> context)
        {
            if (ReturnType != typeof(bool)) throw new Exception("Operation does not return a bool");
            else return GetValue<bool>(context);
        }

        public double GetNumericValue(IMatchCandidate<T> context)
        {
            if (ReturnType != typeof(double)) throw new Exception("Operation does not return a double");
            else return GetValue<double>(context);
        }

        public TReturn GetValue<TReturn>(IMatchCandidate<T> context)
        {
            var left = _operandType == typeof(bool) ? (object)_left.GetBoolValue(context) : (object)_left.GetNumericValue(context);
            var right = _operandType == typeof(bool) ? (object)_right.GetBoolValue(context) : (object)_right.GetNumericValue(context);
            return (TReturn)_operationFunc(left, right);
        }

        public abstract NumericMutability GetNumericMutability();

        public abstract bool IsBooleanMutable(IMatchCandidate<T> context);
    }
}
