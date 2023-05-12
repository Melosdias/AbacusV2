using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Token
{
    public class TokenVariable : Token
    {
        public override string AllowedChars => "_azertyuiopqsdfghjklmwxcvbnAZERTYUIOPQSDFGHJKLMWXCVBN0123456789";

        public TokenVariable(string value) : base(value)
        {
        }
    }
}