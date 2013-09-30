using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Expressions.NumericToNumericOperations
{
    public abstract class AbstractNumericsToNumericOperationExpression<T> : IExpression<T>
    {
        protected IExpression<T> _left;
        protected IExpression<T> _right;
        protected Func<double, double, double> _operation;
        protected string _operator;

        public AbstractNumericsToNumericOperationExpression(IExpression<T> left, IExpression<T> right, 
            Func<double, double, double> operation, string @operator)
        {
            _left = left;
            _right = right;
            _operation = operation;
            _operator = @operator;
        }

        public Type ReturnType { get { return typeof(bool); } }

        public bool GetBoolValue(IMatchCandidate<T> context)
        {
            throw new InvalidOperationException("This operation does not have a boolean value.");      
        }

        public double GetNumericValue(IMatchCandidate<T> context)
        {
            return _operation(_left.GetNumericValue(context), _right.GetNumericValue(context));
        }

        public NumericMutability GetNumericMutability()
        {
            int leftMutability = (int)_left.GetNumericMutability();
            int rightMutability = (int)_right.GetNumericMutability();

            return (NumericMutability)Operators.GetNumericOperatorMutabilities(_operator)[leftMutability, rightMutability];
        }

        public bool IsBooleanMutable(IMatchCandidate<T> context)
        {
            throw new InvalidOperationException("This operation does not have a boolean mutability.");  
        }
    }
}
