using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Expressions
{
    using Core;

    public interface IExpression<T>
    {
        Type ReturnType { get; }

        bool GetBoolValue(IMatchCandidate<T> context);
        double GetNumericValue(IMatchCandidate<T> context);

        /// <summary>
        /// Indicates whether or not the numeric value of this expression is liable to change,
        /// given a different context.
        /// </summary>
        /// <returns></returns>
        NumericMutability GetNumericMutability();

        /// <summary>
        /// Indicates whether or not the boolean value of this expression is liable to change from its value
        /// within the supplied context, if given a different context.
        /// </summary>
        /// <returns></returns>
        bool IsBooleanMutable(IMatchCandidate<T> context);

        Tuple<bool, bool> GetBoolStatus(IMatchCandidate<T> context);
    }

    public enum NumericMutability
    {
        Fixed = 0,
        Increasable = 1,
        Decreaseable = 2,
        CanIncreaseOrDecrease = 3
    }
}
