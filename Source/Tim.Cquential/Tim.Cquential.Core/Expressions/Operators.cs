using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Core.Expressions
{
    using Utilities;
    using System.Globalization;

    public static class Operators
    {
        private static Lazy<IDictionary<string, int[,]>> _numericMutabilities = new Lazy<IDictionary<string, int[,]>>(LoadNumericMutabilities);
        private static Lazy<IDictionary<string, int[,,]>> _booleanMutabilities = new Lazy<IDictionary<string, int[,,]>>(LoadBooleanMutabilities);

        public static Type GetReturnType(string @operator)
        {
            switch (@operator)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                case "^":
                    return typeof(double);
                case "<":
                case ">":
                case "=":
                case "!=":
                case "<=":
                case ">=":
                case "AND":
                case "OR":
                    return typeof(bool);
                default:
                    throw new Exception(String.Format("Unrecognised operator: '{0}'", @operator));
            }
        }

        public static Type GetOperandType(string @operator)
        {
            switch (@operator)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                case "<":
                case ">":
                case "=":
                case "!=":
                case "<=":
                case ">=":
                case "^":
                    return typeof(double);
                case "AND":
                case "OR":
                    return typeof(bool);
                default:
                    throw new Exception(String.Format("Unrecognised operator: '{0}'", @operator));
            }
        }

        public static bool IsLeftAssociative(string @operator)
        {
            bool isLeftAssociative = false;

            switch (@operator)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                case "<":
                case ">":
                case "=":
                case "!=":
                case "<=":
                case ">=":
                case "AND":
                case "OR":
                    isLeftAssociative = true;
                    break;
                case "^":
                    isLeftAssociative = false;
                    break;
                default:
                    throw new Exception(String.Format("Unrecognised operator: '{0}'", @operator));
            }

            return isLeftAssociative;
        }

        public static int GetPrecendence(string @operator)
        {
            int precedence = 0;

            switch (@operator)
            {
                case "^":
                    precedence = 4;
                    break;
                case "/":
                case "*":
                    precedence = 3;
                    break;
                case "+":
                case "-":
                    precedence = 2;
                    break;
                case "<":
                case ">":
                case "=":
                case "!=":
                case "<=":
                case ">=":
                    precedence = 1;
                    break;
                case "AND":
                case "OR":
                    precedence = 0;
                    break;
                default:
                    throw new Exception(String.Format("Unrecognised operator: '{0}'", @operator));
            }

            return precedence;
        }

        public static Func<object, object, object> GetOperationFunc(string @operator)
        {
            var returnType = GetOperandType(@operator);

            if (returnType == typeof(double)) return (l, r) => GetNumericOperationFunc(@operator)((double)l, (double)r);
            else if (returnType == typeof(bool)) return (l, r) => GetBooleanOperationFunc(@operator)((bool)l, (bool)r);
            else throw new Exception(String.Format("Unrecognised operation type: '{0}'", @operator));
        }

        public static Func<double, double, object> GetNumericOperationFunc(string @operator)
        {
            switch (@operator)
            {
                case "+": return (l, r) => l + r;
                case "-": return (l, r) => l - r;
                case "*": return (l, r) => l * r;
                case "/": return (l, r) => l / r;
                case "^": return (l, r) => Math.Pow(l, r);
                case "<": return (l, r) => l < r;
                case ">": return (l, r) => l > r;
                case "=": return (l, r) => l == r;
                case "!=": return (l, r) => l != r;
                case "<=": return (l, r) => l <= r;
                case ">=": return (l, r) => l >= r;
                default:
                    throw new Exception(String.Format("Unrecognised operator: '{0}'", @operator));
            }
        }

        public static Func<bool, bool, object> GetBooleanOperationFunc(string @operator)
        {
            switch (@operator)
            {
                case "AND": return (l, r) => l && r;
                case "OR": return (l, r) => l || r;
                default:
                    throw new Exception(String.Format("Unrecognised operator: '{0}'", @operator));
            }
        }

        public static int[,] GetNumericOperatorMutabilities(string @operator)
        {
            return _numericMutabilities.Value[@operator];
        }

        public static int[, ,] GetBooleanOperatorMutabilities(string @operator)
        {
            return _booleanMutabilities.Value[@operator];
        }

        private static IDictionary<string, int[,]> LoadNumericMutabilities()
        {
            var dict = new Dictionary<string, int[,]>();

            dict.AddMany(new string[] { "+", "*" }, Mutability<NumericMutability, NumericMutability>(4)
                        .AddDiagonals()
                        .AddCommutative(NumericMutability.Fixed, NumericMutability.Increasable, NumericMutability.Increasable)
                        .AddCommutative(NumericMutability.Fixed, NumericMutability.Decreaseable, NumericMutability.Decreaseable)
                        .AddCommutative(NumericMutability.Fixed, NumericMutability.CanIncreaseOrDecrease, NumericMutability.CanIncreaseOrDecrease)
                        .AddCommutative(NumericMutability.Increasable, NumericMutability.Decreaseable, NumericMutability.CanIncreaseOrDecrease)
                        .AddCommutative(NumericMutability.Increasable, NumericMutability.CanIncreaseOrDecrease, NumericMutability.CanIncreaseOrDecrease)
                        .AddCommutative(NumericMutability.Decreaseable, NumericMutability.CanIncreaseOrDecrease, NumericMutability.CanIncreaseOrDecrease)
                        .Get());

            dict.AddMany(new string[] { "-", "/" }, Mutability<NumericMutability, NumericMutability>(4)
                        .AddForAllLeft(NumericMutability.CanIncreaseOrDecrease, NumericMutability.CanIncreaseOrDecrease)
                        .AddForAllRight(NumericMutability.CanIncreaseOrDecrease, NumericMutability.CanIncreaseOrDecrease)
                        .Add(NumericMutability.Fixed, NumericMutability.Fixed, NumericMutability.Fixed)
                        .Add(NumericMutability.Fixed, NumericMutability.Increasable, NumericMutability.Decreaseable)
                        .Add(NumericMutability.Fixed, NumericMutability.Decreaseable, NumericMutability.Increasable)
                        .Add(NumericMutability.Increasable, NumericMutability.Fixed, NumericMutability.Increasable)
                        .Add(NumericMutability.Increasable, NumericMutability.Increasable, NumericMutability.CanIncreaseOrDecrease)
                        .Add(NumericMutability.Increasable, NumericMutability.Decreaseable, NumericMutability.Increasable)
                        .Add(NumericMutability.Decreaseable, NumericMutability.Fixed, NumericMutability.Decreaseable)
                        .Add(NumericMutability.Decreaseable, NumericMutability.Increasable, NumericMutability.Decreaseable)
                        .Add(NumericMutability.Decreaseable, NumericMutability.Decreaseable, NumericMutability.CanIncreaseOrDecrease)
                        .Get());

            return dict;
        }

        private static IDictionary<string, int[,,]> LoadBooleanMutabilities()
        {
            var dict = new Dictionary<string, int[,,]>();

            var lessThan = Mutability<NumericMutability, bool>(4)
                       .AddForAllLeft(NumericMutability.CanIncreaseOrDecrease, true)
                       .AddForAllRight(NumericMutability.CanIncreaseOrDecrease, true)
                       .Add(NumericMutability.Fixed, NumericMutability.Fixed, false)
                       .Add(NumericMutability.Fixed, NumericMutability.Increasable, false)
                       .Add(NumericMutability.Fixed, NumericMutability.Decreaseable, true)
                       .AddForAllRight(NumericMutability.Increasable, true)
                       .Add(NumericMutability.Decreaseable, NumericMutability.Fixed, false)
                       .Add(NumericMutability.Decreaseable, NumericMutability.Increasable, false)
                       .Add(NumericMutability.Decreaseable, NumericMutability.Decreaseable, true);

            dict.AddMany(new string[] { "<", "<=" }, lessThan.ToBooleanMutability());
            dict.AddMany(new string[] { ">", ">=" }, lessThan.Flip().ToBooleanMutability());

            var equals = Mutability<NumericMutability, bool>(4)
                            .AddCommutative(NumericMutability.Increasable, NumericMutability.Increasable, 
            return dict;
        }

        private static MutabilityMaker<TIn, TOut> Mutability<TIn, TOut>(int size)
            where TIn : struct, IConvertible
            where TOut : struct, IConvertible
        {
            return new MutabilityMaker<TIn, TOut>(size);
        }
    }

    internal class MutabilityMaker<TIn, TOut> 
        where TIn : struct, IConvertible
        where TOut : struct, IConvertible
    {
        private int[,] _mutabilities;
        private int _size;

        internal MutabilityMaker(int size)
        {
            _mutabilities = new int[size,size];
            _size = size;
        }

        protected MutabilityMaker(int[,] mutabilities, int size)
        {
            _mutabilities = mutabilities;
            _size = size;
        }

        internal MutabilityMaker<TIn, TOut> Add(TIn left, TIn right, TOut output)
        {
            _mutabilities[ToInt(left),ToInt(right)] = ToInt(output);

            return this;
        }

        internal MutabilityMaker<TIn, TOut> AddCommutative(TIn left, TIn right, TOut output)
        {
            _mutabilities[ToInt(left), ToInt(right)] = ToInt(output);
            _mutabilities[ToInt(right), ToInt(left)] = ToInt(output);

            return this;
        }

        internal MutabilityMaker<TIn, TOut> AddDiagonals()
        {
            for (int i = 0; i < _size; i++) _mutabilities[i,i] = i;

            return this;
        }

        internal MutabilityMaker<TIn, TOut> AddForAllLeft(TIn right, TOut output)
        {
            for (int i = 0; i < _size; i++) _mutabilities[i, ToInt(right)] = ToInt(output);

            return this;
        }

        internal MutabilityMaker<TIn, TOut> AddForAllRight(TIn left, TOut output)
        {
            for (int i = 0; i < _size; i++) _mutabilities[ToInt(left), i] = ToInt(output);

            return this;
        }

        internal int[,] Get()
        {
            return _mutabilities;
        }

        internal MutabilityMaker<TIn, TOut> Flip()
        {
            var flipped = new int[_size, _size];

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    flipped[i, j] = _mutabilities[j, i];
                }
            }

            return new MutabilityMaker<TIn, TOut>(flipped, _size);
        }

        internal int[, ,] ToBooleanMutability()
        {
            var result = new int[_size, _size, 2];
            var flipped = Flip().Get();

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    result[i, j, 1] = _mutabilities[i, j];
                    result[i, j, 0] = flipped[i, j];
                }
            }

            return result;
        }

        private int ToInt(TIn enumValue)
        {
            return enumValue.ToInt32(CultureInfo.InvariantCulture);
        }

        private int ToInt(TOut enumValue)
        {
            return enumValue.ToInt32(CultureInfo.InvariantCulture);
        }
    }
}
