using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Language.Fluent
{
    using Core;
    using Core.Aggregation;

    public interface IFluentOperatorBuilder<T>
    {
        IFluentOperandBuilder<T> IsGreaterThan();
        IFluentOperandBuilder<T> And();
        IFluentOperandBuilder<T> Times();

        IQuery<T> ToQuery();
    }



    public interface IFluentOperandBuilder<T>
    {
        IFluentOperatorBuilder<T> StaticItem(int itemIndex, System.Linq.Expressions.Expression<Func<T, double>> memberExpression);
        IFluentOperatorBuilder<T> Constant(double value);
        IFluentOperatorBuilder<T> Max(System.Linq.Expressions.Expression<Func<T, double>> memberExpression);
        IFluentOperatorBuilder<T> Min(System.Linq.Expressions.Expression<Func<T, double>> memberExpression);
    }

    public static class FluentExpression
    {
        public static FluentExpressionBuilder<T> Make<T>()
        {
            return new FluentExpressionBuilder<T>();
        }
    }

    public class FluentExpressionBuilder<T> : IFluentOperandBuilder<T>, IFluentOperatorBuilder<T>
    {
        private AggregatingExpressionFactory<T> _expFactory;

        public FluentExpressionBuilder()
        {
            _expFactory = new AggregatingExpressionFactory<T>();
        }

        public IQuery<T> ToQuery()
        {
            return null;
        }

        public IFluentOperandBuilder<T> IsGreaterThan()
        {
            return this;
        }

        public IFluentOperandBuilder<T> And()
        {
            return this;
        }

        public IFluentOperandBuilder<T> Times()
        {
            return this;
        }

        public IFluentOperatorBuilder<T> StaticItem(int itemIndex, System.Linq.Expressions.Expression<Func<T, double>> memberExpression)
        {
            return this;
        }

        public IFluentOperatorBuilder<T> Constant(double value)
        {
            return this;
        }

        public IFluentOperatorBuilder<T> Max(System.Linq.Expressions.Expression<Func<T, double>> memberExpression)
        {
            return this;
        }

        public IFluentOperatorBuilder<T> Min(System.Linq.Expressions.Expression<Func<T, double>> memberExpression)
        {
            return this;
        }
    }
}
