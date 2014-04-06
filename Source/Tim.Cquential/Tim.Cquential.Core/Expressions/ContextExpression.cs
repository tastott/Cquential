using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Tim.Cquential.Core.Expressions
{
    using Matching;

    public class ContextExpression<T> : IExpression<T>
    {
        private Func<IMatchCandidate<T>, double> _valueFunc;
        private NumericMutability _mutability;

        public ContextExpression(Func<IMatchCandidate<T>, double> valueFunc, NumericMutability mutability)
        {
            _valueFunc = valueFunc;
            _mutability = mutability;
        }

        public Type ReturnType { get { return typeof(double); } }

        

        public NumericMutability GetNumericMutability()
        {
            return _mutability;
        }

        public bool GetBoolValue(IMatchCandidate<T> context)
        {
            throw new NotImplementedException();
        }

        public double GetNumericValue(IMatchCandidate<T> context)
        {
            return _valueFunc(context);
        }

        public bool IsBooleanMutable(IMatchCandidate<T> context)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            var that = obj as ContextExpression<T>;

            if (that != null) return this._valueFunc.Equals(that._valueFunc);
            else return false;
        }

        protected static Func<T, double> GetMemberFunction(string memberName)
        {
            var memberInfo = typeof(T).GetMember(memberName)[0];

            var parameterExp = Expression.Parameter(typeof(T));
            var memExp = Expression.MakeMemberAccess(parameterExp, memberInfo);
            var lambdaExp = Expression.Lambda<Func<T, double>>(memExp, parameterExp);

            return lambdaExp.Compile();
        }


        public Tuple<bool, bool> GetBoolStatus(IMatchCandidate<T> context)
        {
            throw new NotImplementedException();
        }
    }
}
