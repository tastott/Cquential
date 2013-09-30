using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tim.Cquential.Language.Fluent
{
    [TestClass]
    public class FluentExpressionTests
    {
        [TestMethod]
        public void FluentExpressionTest()
        {
            FluentExpression.Make<DateTime>()
                .StaticItem(0, d => d.Day)
                .IsGreaterThan()
                .Constant(20)
                .And()
                .Max(d => d.Year)
                .IsGreaterThan()
                .Min(d => d.Year)
                .Times()
                .Constant(2)
                .ToQuery();
        }
    }
}
