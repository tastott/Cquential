using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Core.Expressions
{
    using System.Linq.Expressions;
    using Core;
    using Matching;
    using Expressions.NumericToBooleanOperations;
    using Expressions.NumericToNumericOperations;
    using Expressions.BooleanToBooleanOperations;
    using Expressions.ContextExpressions;

    public class ExpressionFactory<T>
    {
        public IExpression<T> Constant(double value)
        {
            return new ConstantExpression<T>(value);
        }

        #region Boolean to Boolean

        public IExpression<T> And(IExpression<T> left, IExpression<T> right)
        {
            return new AndExpression<T>(left, right);
        }

        public IExpression<T> Or(IExpression<T> left, IExpression<T> right)
        {
            return new OrExpression<T>(left, right);
        }

        #endregion

        #region Numeric to Boolean 

        public IExpression<T> GreaterThan(IExpression<T> left, IExpression<T> right)
        {
            return new GreaterThanExpression<T>(left, right);
        }

        public IExpression<T> LessThan(IExpression<T> left, IExpression<T> right)
        {
            return new LessThanExpression<T>(left, right);
        }

        public IExpression<T> LessThanOrEqualTo(IExpression<T> left, IExpression<T> right)
        {
            return new LessThanOrEqualToExpression<T>(left, right);
        }

        public IExpression<T> GreaterThanOrEqualTo(IExpression<T> left, IExpression<T> right)
        {
            return new GreaterThanOrEqualToExpression<T>(left, right);
        }

        #endregion

        #region Numeric to Numeric

        public IExpression<T> Plus(IExpression<T> left, IExpression<T> right)
        {
            return new PlusExpression<T>(left, right);
        }

        public IExpression<T> Divide(IExpression<T> left, IExpression<T> right)
        {
            return new DivideExpression<T>(left, right);
        }

        public IExpression<T> Times(IExpression<T> left, IExpression<T> right)
        {
            return new TimesExpression<T>(left, right);
        }

        #endregion

        #region Aggregate

        public virtual IExpression<T> Max(string memberName)
        {
            return new MaxExpression<T>(memberName);
        }

        public virtual IExpression<T> Min(string memberName)
        {
            return new MinExpression<T>(memberName);
        }

        public virtual IExpression<T> Average(string memberName)
        {
            return new AverageExpression<T>(memberName);
        }

        #endregion

        #region Other

        public IExpression<T> Operation(string @operator, IExpression<T> left, IExpression<T> right)
        {
            switch (@operator)
            {
                case "+":
                    return Plus(left, right);
                case "/":
                    return Divide(left, right);
                case "<":
                    return LessThan(left, right);
                case "<=":
                    return LessThanOrEqualTo(left, right);
                case ">":
                    return GreaterThan(left, right);
                case ">=":
                    return GreaterThanOrEqualTo(left, right);
                case "*":
                    return Times(left, right);
                case "AND":
                    return And(left, right);
                default:
                    throw new ArgumentException(String.Format("Operator '{0}' is not supported.", @operator));
            }
            //var returnType = Operators.GetReturnType(@operator);
            //var operandType = Operators.GetOperandType(@operator);

            //if (returnType == typeof(double) && operandType == typeof(double))
            //    return new NumericToNumericOperationExpression<T>(Operators.GetOperationFunc(@operator),
            //        left, right, Operators.GetNumericOperatorMutabilities(@operator));
            //else if (returnType == typeof(bool) && operandType == typeof(double))
            //    return new NumericToBooleanOperationExpression<T>(Operators.GetOperationFunc(@operator),
            //        left, right, Operators.GetBooleanOperatorMutabilities(@operator));
            //else if (returnType == typeof(bool) && operandType == typeof(bool))
            //{
            //    switch (@operator)
            //    {
            //        case "AND":
            //            return new AndExpression<T>(left, right);
            //        case "OR":
            //            return new OrExpression<T>(left, right);
            //        default:
            //            throw new Exception(String.Format("Unrecognised boolean to boolean operator: '{0}'", @operator));
            //    }
            //}
            //else throw new Exception(String.Format("Invalid combination of return and operand types: '{0}' and '{1}'", returnType.Name, operandType.Name));
        }

        public virtual IExpression<T> FirstItemMember(string memberName)
        {
            return new FirstItemMemberExpression<T>(memberName);
        }

        public virtual IExpression<T> StaticItemMember(int itemIndex, string memberName)
        {
            var memberFunc = GetMemberFunc(memberName);

            return new ContextExpression<T>(c => memberFunc(c.Sequence.ToList()[itemIndex]), NumericMutability.Fixed);
        }

        public IExpression<T> AllTrue(string @operator, int leftItemOffset, string leftItemMember, int rightItemOffset, string rightItemMember)
        {
            var operationFunc = Operators.GetOperationFunc(@operator);
            var leftItemMemberFunc = GetMemberFunc(leftItemMember);
            var rightItemMemberFunc = GetMemberFunc(rightItemMember);

            if (leftItemOffset != 0 || rightItemOffset != -1) throw new Exception("Offsets not supported");

            Func<MatchCandidate<T>, double[]> leftValuesFunc = c => c.Sequence.Skip(1).Select(l => leftItemMemberFunc(l)).ToArray();
            Func<MatchCandidate<T>, double[]> rightValuesFunc = c => c.Sequence.Take(c.Sequence.Count() - 1).Select(l => rightItemMemberFunc(l)).ToArray();

            return new AllExpression<T>(operationFunc, leftValuesFunc, rightValuesFunc);
        }

        //public IExpression<T> Aggregate(string functionName, string memberName)
        //{
        //    var memberFunc = GetMemberFunc(memberName);

        //    switch (functionName)
        //    {
        //        //case "MAX":
        //        //    return new ContextExpression<T>(c => c.Sequence.Max(l => memberFunc(l)), NumericMutability.Increasable);

        //        //case "MIN":
        //        //    return new ContextExpression<T>(c => c.Sequence.Min(l => memberFunc(l)), NumericMutability.Decreaseable);

        //        //case "AVG":
        //        //    return new ContextExpression<T>(c => c.Sequence.Average(l => memberFunc(l)), NumericMutability.CanIncreaseOrDecrease);

        //        //case "COUNT":
        //        //    return new ContextExpression<T>(c => c.Sequence.Count(), NumericMutability.Increasable);

        //        default:
        //            throw new Exception(String.Format("Unrecognised function name: '{0}'", functionName));
        //    }
        //}

        #endregion

        protected static Func<T, double> GetMemberFunc(string name)
        {
            var memberInfo = typeof(T).GetMember(name)[0];

            var parameterExp = Expression.Parameter(typeof(T));
            var memExp = Expression.MakeMemberAccess(parameterExp, memberInfo);
            var lambdaExp = Expression.Lambda<Func<T, double>>(memExp, parameterExp);

            return lambdaExp.Compile();

        }
    }
}
