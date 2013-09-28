using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Language.Tokens
{
    public class TokenTreeNode : Token
    {
        private static TokenTreeNode[] NoChildren = new TokenTreeNode[0];

        public TokenTreeNode[] Children { get; private set; }

        public TokenTreeNode(Token token, TokenTreeNode[] children)
            :base(token.Type, token.Value)
        {
            Children = children;
        }

        public TokenTreeNode(Token token, TokenTreeNode onlyChild)
            : this(token, new TokenTreeNode[] { onlyChild }) { }

        public TokenTreeNode(Token token, TokenTreeNode leftChild, TokenTreeNode rightChild)
            : this(token, new TokenTreeNode[] { leftChild, rightChild }) { }

        public TokenTreeNode(Token token)
            : this(token, NoChildren) { }

        
    }
}
