using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Token
{
    /**
     * <summary>A class for special characters like parenthesis and comas</summary>
     */
    public class TokenParenthesis : Token
    {
        public override string AllowedChars => "(),;";

        public TokenParenthesis(string c) : base(c)
        {

        }
    }
}
