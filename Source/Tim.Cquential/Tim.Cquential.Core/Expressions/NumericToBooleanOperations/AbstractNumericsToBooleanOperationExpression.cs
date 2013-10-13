using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Expressions.NumericToBooleanOperations
{
    public abstract class AbstractNumericsToBooleanOperationExpression<T> : IExpression<T>
    {
        protected IExpression<T> _left;
        protected IExpression<T> _right;
        protected Func<double, double, bool> _operation;
        protected string _operator;

        public AbstractNumericsToBooleanOperationExpression(IExpression<T> left, IExpression<T> right, 
            Func<double, double, bool> operation, string @operator)
        {
            _left = left;
            _right = right;
            _operation = operation;
            _operator = @operator;
        }

        public Type ReturnType { get { return typeof(bool); } }

        public bool GetBoolValue(IMatchCandidate<T> context)
        {
            return _operation(_left.GetNumericValue(context), _right.GetNumericValue(context));
        }

        public double GetNumericValue(IMatchCandidate<T> context)
        {
            throw new InvalidOperationException("This operation does not have a numeric value.");
        }

        public NumericMutability GetNumericMutability()
        {
            throw new InvalidOperationException("This operation does not have a numeric mutability.");
        }

        public bool IsBooleanMutable(IMatchCandidate<T> context)
        {
            int leftMutability = (int)_left.GetNumericMutability();
            int rightMutability = (int)_right.GetNumericMutability();
            int currentValue = GetBoolValue(context) ? 1 : 0;

            return Operators.GetBooleanOperatorMutabilities(_operator)[leftMutability, rightMutability, currentValue] == 1;
        }


        public Tuple<bool, bool> GetBoolStatus(IMatchCandidate<T> context)
        {
            int leftMutability = (int)_left.GetNumericMutability();
            int rightMutability = (int)_right.GetNumericMutability();
            int currentValue = GetBoolValue(context) ? 1 : 0;

            var isMutable =  Operators.GetBooleanOperatorMutabilities(_operator)[leftMutability, rightMutability, currentValue] == 1;

            return Tuple.Create(currentValue == 1, isMutable);
        }
    }
}
