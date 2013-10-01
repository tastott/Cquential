﻿using System;
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

        public override IExpression<T> Average(string memberName)
        {
            var aggregatorKey = String.Format("{0}.{1}", "average", memberName);

            if (!AggregatorFactory.ContainsKey(aggregatorKey))
            {
                AggregatorFactory.Add(aggregatorKey, () => new AverageAggregator<T>(GetMemberFunc(memberName)));
            }

            return new AggregatingContextExpression<T>(aggregatorKey, NumericMutability.CanIncreaseOrDecrease);
        }

        public override IExpression<T> FirstItemMember(string memberName)
        {
            var aggregatorKey = String.Format("{0}[0].{1}", "item", memberName);

            if (!AggregatorFactory.ContainsKey(aggregatorKey))
            {
                AggregatorFactory.Add(aggregatorKey, () => new IndexedItemAggregator<T>(0,GetMemberFunc(memberName)));
            }

            return new AggregatingContextExpression<T>(aggregatorKey, NumericMutability.CanIncreaseOrDecrease);
        }

        public IDictionary<string, Func<IAggregator<T>>> AggregatorFactory { get; private set;}
    }
}
