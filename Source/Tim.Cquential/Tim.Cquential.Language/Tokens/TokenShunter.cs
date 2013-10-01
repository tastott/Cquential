using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Language.Tokens
{
    using Core.Expressions;

    public class TokenShunter : ITokenShunter
    {
        public IEnumerable<Token> Shunt(IEnumerable<Token> infixTokens)
        {
            Queue<Token> output = new Queue<Token>();
            Stack<Token> stack = new Stack<Token>();

            foreach (var token in infixTokens)
            {
                if (token.Type == TokenType.Operator)
                {
                    while (stack.Any() && stack.Peek().Type == TokenType.Operator)
                    {
                        var topOperator = stack.Peek().Value;
                        int precedenceDiff = Operators.GetPrecendence(token.Value) - Operators.GetPrecendence(topOperator);

                        if (precedenceDiff < 0 || (precedenceDiff == 0 && Operators.IsLeftAssociative(token.Value))) output.Enqueue(stack.Pop());
                        else break;
                    }

                    stack.Push(token);
                }
                else if (token.Type == TokenType.Constant || token.Type == TokenType.SingleItemMember)
                {
                    output.Enqueue(token);
                }
                else if (token.Type == TokenType.LeftParenthesis || token.Type == TokenType.Function)
                {
                    stack.Push(token);
                }
                else if (token.Type == TokenType.RightParenthesis)
                {
                    while (stack.Any() && stack.Peek().Type != TokenType.LeftParenthesis) output.Enqueue(stack.Pop());

                    if (!stack.Any()) throw new Exception("Missing left parenthesis");
                    
                    stack.Pop(); //Discard left parenthesis

                    if (stack.Peek().Type == TokenType.Function) output.Enqueue(stack.Pop()); //Pop function onto output if present
                }
                else throw new Exception(String.Format("Unrecognised token type: '{0}'", token.Type.ToString()));

            }

            while (stack.Any()) output.Enqueue(stack.Pop());

            return output;
        }
    }
}
