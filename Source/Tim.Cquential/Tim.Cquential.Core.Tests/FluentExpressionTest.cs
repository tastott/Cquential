using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tim.Cquential.Core.Expressions.Fluent
{
    [TestClass]
    public class FluentExpressionTest
    {
        [TestMethod]
        public void TestMethod1()
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
