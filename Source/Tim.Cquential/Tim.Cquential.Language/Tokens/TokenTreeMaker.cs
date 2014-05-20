using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Language.Tokens
{
    public class TokenTreeMaker
    {
        private ITokenShunter _shunter;

        public TokenTreeMaker(ITokenShunter shunter)
        {
            _shunter = shunter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens">A sequence of tokens in infix notation.</param>
        /// <returns></returns>
        public TokenTreeNode MakeTree(IEnumerable<Token> tokens)
        {
            //Rewrite token sequence in RPN
            var tokensRpn = _shunter.Shunt(tokens);

            //Build tree
            Stack<TokenTreeNode> nodes = new Stack<TokenTreeNode>();
         
            foreach (var token in tokensRpn)
            {
                TokenTreeNode node = null;

                switch (token.Type)
                {
                    case TokenType.Constant:
                    case TokenType.SingleItemMember:
                        node = new TokenTreeNode(token);
                        break;

                    case TokenType.Function: //Pop off parameters as appropriate
                        var parameters = nodes.PopMany(token.ParameterCount);
                        node = new TokenTreeNode(token, parameters);
                        break;

                    case TokenType.Operator:
                        var expRight = nodes.Pop();
                        var expLeft = nodes.Pop();
                        node = new TokenTreeNode(token, expLeft, expRight);
                        break;

                    default:
                        throw new Exception(String.Format("Unrecognised TokenType: '{0}'", token.Type.ToString()));
                }

                nodes.Push(node);
            }

            if (nodes.Count != 1)
                throw new Exception("Invalid RPN token sequence");

            return nodes.Pop();
        }
    }

    public static class StackExtensions
    {
        public static IEnumerable<T> PopMany<T>(this Stack<T> stack, int count)
        {
            var popped = new List<T>(count);

            for (int i = 0; i < count; i++) popped.Add(stack.Pop());

            return popped;
        }
    }
}
