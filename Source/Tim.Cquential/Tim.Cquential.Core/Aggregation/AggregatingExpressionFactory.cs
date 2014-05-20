using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.Cquential.Core.Aggregation.Aggregators;
using Tim.Cquential.Core.Expressions;

namespace Tim.Cquential.Core.Aggregation
{
    public class AggregatingExpressionFactory<T> : ExpressionFactory<T>
    {
        public AggregatingExpressionFactory()
        {
            AggregatorFactory = new Dictionary<string, Func<IAggregator<T>>>();
        }

        public override IExpression<T> Count()
        {
            var aggregatorKey = "count";

            AddAggregator(aggregatorKey, () => new CountAggregator<T>());

            return new AggregatingContextExpression<T>(aggregatorKey, NumericMutability.Increasable);
        }

        public override IExpression<T> Average(string memberName)
        {
            var aggregatorKey = String.Format("{0}.{1}", "average", memberName);

            AddAggregator(aggregatorKey, () => new AverageAggregator<T>(GetMemberFunc(memberName)));

            return new AggregatingContextExpression<T>(aggregatorKey, NumericMutability.CanIncreaseOrDecrease);
        }

        public override IExpression<T> Max(string memberName)
        {
            var aggregatorKey = String.Format("{0}.{1}", "max", memberName);

            AddAggregator(aggregatorKey, () => new MaxAggregator<T>(GetMemberFunc(memberName)));

            return new AggregatingContextExpression<T>(aggregatorKey, NumericMutability.Increasable);
        }

        public override IExpression<T> Min(string memberName)
        {
            var aggregatorKey = String.Format("{0}.{1}", "min", memberName);

            AddAggregator(aggregatorKey, () => new MinAggregator<T>(GetMemberFunc(memberName)));

            return new AggregatingContextExpression<T>(aggregatorKey, NumericMutability.Increasable);
        }

        public override IExpression<T> FirstItemMember(string memberName)
        {
            var aggregatorKey = String.Format("{0}[0].{1}", "item", memberName);

            AddAggregator(aggregatorKey, () => new IndexedItemAggregator<T>(0,GetMemberFunc(memberName)));

            return new AggregatingContextExpression<T>(aggregatorKey, NumericMutability.CanIncreaseOrDecrease);
        }

        public override IExpression<T> LastItemMember(string memberName)
        {
            var aggregatorKey = String.Format("{0}[n].{1}", "item", memberName);

            AddAggregator(aggregatorKey, () => new LastItemAggregator<T>(GetMemberFunc(memberName)));

            return new AggregatingContextExpression<T>(aggregatorKey, NumericMutability.CanIncreaseOrDecrease);
        }

        public override IExpression<T> AllTrue(string @operator, int leftItemOffset, string leftItemMember, int rightItemOffset, string rightItemMember)
        {
            var operationFunc = Operators.GetNumericOperationFunc(@operator);
            var leftItemMemberFunc = GetMemberFunc(leftItemMember);
            var rightItemMemberFunc = GetMemberFunc(rightItemMember);

            if (leftItemOffset != 0 || rightItemOffset != -1) throw new Exception("Offsets not supported");

            var currentAggregatorKey = String.Format("{0}[x].{1}", "item", leftItemMember);
            var previousAggregatorKey = String.Format("{0}[x-1].{1}", "item", rightItemMember);

            AddAggregator(currentAggregatorKey, () => new LastItemAggregator<T>(leftItemMemberFunc));
            AddAggregator(previousAggregatorKey, () => new PenultimateItemAggregator<T>(rightItemMemberFunc));

            return new CurrentVsPreviousExpression<T>(currentAggregatorKey, previousAggregatorKey, (a,b) => (bool)operationFunc(a,b));
        }

        private void AddAggregator(string key, Func<IAggregator<T>> factory)
        {
            if (!AggregatorFactory.ContainsKey(key))
            {
                AggregatorFactory.Add(key, factory);
            }
        }

        public IDictionary<string, Func<IAggregator<T>>> AggregatorFactory { get; private set;}
    }
}
