using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.Cquential.Core.Aggregation;
using Tim.Cquential.Core.Expressions;
using Tim.Cquential.Core.Queries;

namespace Tim.Cquential.Language.Parsers
{
    public class ExpressionWithAggregationsQueryParser<T>: ExpressionQueryParser<T>
    {
        public ExpressionWithAggregationsQueryParser()
            : base(new AggregatingExpressionFactory<T>())
        { }

        public override Core.IQuery<T> Parse(IEnumerable<Token> rpnTokens)
        {
            var tokenTreeRoot = _tokenTreeMaker.MakeTree(rpnTokens);
            IExpression<T> expression = CreateExpressionTree(tokenTreeRoot);

            var aggregatorFactory = ((AggregatingExpressionFactory<T>)_expressions).AggregatorFactory;

            return new ExpressionWithAggregatorsQuery<T>(expression, aggregatorFactory);
        }
    }
}
