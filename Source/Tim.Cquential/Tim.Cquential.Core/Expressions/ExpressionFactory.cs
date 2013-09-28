using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Core.Expressions
{
    using System.Linq.Expressions;
    using Core;
    using Matching;

    public static class ExpressionFactory
    {
        public static IExpression<T> Constant<T>(double value)
        {
            return new ConstantExpression<T>(value);
        }

        public static IExpression<T> Operation<T>(string @operator, IExpression<T> left, IExpression<T> right)
        {
            var returnType = Operators.GetReturnType(@operator);
            var operandType = Operators.GetOperandType(@operator);

            if (returnType == typeof(double) && operandType == typeof(double))
                return new NumericToNumericOperationExpression<T>(Operators.GetOperationFunc(@operator),
                    left, right, Operators.GetNumericOperatorMutabilities(@operator));
            else if (returnType == typeof(bool) && operandType == typeof(double))
                return new NumericToBooleanOperationExpression<T>(Operators.GetOperationFunc(@operator),
                    left, right, Operators.GetBooleanOperatorMutabilities(@operator));
            else if (returnType == typeof(bool) && operandType == typeof(bool))
            {
                switch (@operator)
                {
                    case "AND":
                        return new AndExpression<T>(left, right);
                    case "OR":
                        return new OrExpression<T>(left, right);
                    default:
                        throw new Exception(String.Format("Unrecognised boolean to boolean operator: '{0}'", @operator));
                }
            }
            else throw new Exception(String.Format("Invalid combination of return and operand types: '{0}' and '{1}'", returnType.Name, operandType.Name));
        }

        public static IExpression<T> StaticLegProperty<T>(int itemIndex, string propertyName)
        {
            var propertyFunc = GetMemberFunc<T>(propertyName);

            return new ContextExpression<T>(c => propertyFunc(c.Sequence.ToList()[itemIndex]), NumericMutability.Fixed);
        }

        public static IExpression<T> AllTrue<T>(string @operator, int leftLegOffset, string leftLegProperty, int rightLegOffset, string rightLegProperty)
        {
            var operationFunc = Operators.GetOperationFunc(@operator);
            var leftLegPropertyFunc = GetMemberFunc<T>(leftLegProperty);
            var rightLegPropertyFunc = GetMemberFunc<T>(rightLegProperty);

            if (leftLegOffset != 0 || rightLegOffset != -1) throw new Exception("Offsets not supported");

            Func<MatchCandidate<T>, double[]> leftValuesFunc = c => c.Sequence.Skip(1).Select(l => leftLegPropertyFunc(l)).ToArray();
            Func<MatchCandidate<T>, double[]> rightValuesFunc = c => c.Sequence.Take(c.Sequence.Count() - 1).Select(l => rightLegPropertyFunc(l)).ToArray();

            return new AllExpression<T>(operationFunc, leftValuesFunc, rightValuesFunc);
        }

        public static IExpression<T> Aggregate<T>(string functionName, string propertyName)
        {
            var propertyFunc = GetMemberFunc<T>(propertyName);

            switch (functionName)
            {
                case "MAX":
                    return new ContextExpression<T>(c => c.Sequence.Max(l => propertyFunc(l)), NumericMutability.Increasable); 

                case "MIN":
                    return new ContextExpression<T>(c => c.Sequence.Min(l => propertyFunc(l)), NumericMutability.Decreaseable);

                case "AVG":
                    return new ContextExpression<T>(c => c.Sequence.Average(l => propertyFunc(l)), NumericMutability.CanIncreaseOrDecrease);

                case "COUNT":
                    return new ContextExpression<T>(c => c.Sequence.Count(), NumericMutability.Increasable);

                default:
                    throw new Exception(String.Format("Unrecognised function name: '{0}'", functionName));
            }
        }

        private static Func<T, double> GetMemberFunc<T>(string name)
        {
            //TODO: Make this less shit.
            //switch (name)
            //{
            //    case "Speed":
            //        return l => l.Speed;
            //    case "StartElevation":
            //        return l => l.StartElevation;
            //    default:
            //        throw new Exception(String.Format("Member name not recognised: {0}", name));
            //}

            var thing = typeof(T).GetMember(name)[0];

            var parameterExpression = Expression.Parameter(typeof(T));
            var memExp = Expression.MakeMemberAccess(parameterExpression, thing);
            var lambdaExp = Expression.Lambda<Func<T, double>>(memExp, parameterExpression);

            return lambdaExp.Compile();

        }
    }
}
