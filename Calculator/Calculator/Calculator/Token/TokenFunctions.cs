using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Token;

namespace Calculator
{
    /**
     * <summary>
     * This class is for functions
     * </summary>
     *
     */
    public class TokenFunctions : Token.Token
    {
        public override string AllowedChars => "sqrtmaxminfactoisprimefibogcd";
        public TokenFunctions(string c) : base(c)
        {
        }

        public static TokenOperand Sqrt(Token.Token a)
        {
            if (a.number < 0) throw new InvalidExpressionException("Invalid operation.");
            return new TokenOperand(Math.Sqrt(a.number).ToString());
        }
        public static TokenOperand max(Token.Token a, Token.Token b)
        {
            return new TokenOperand(Math.Max(a.number, b.number).ToString());
        }
        public static TokenOperand min(Token.Token a, Token.Token b)
        {
            return new TokenOperand(Math.Min(a.number, b.number).ToString());
        }
        public static TokenOperand facto(Token.Token a)
        {

            if (a.number < 0) throw new InvalidExpressionException("Invalid operation.");
            if (a.number == 0 || a.number == 1) return new TokenOperand("1");
            double result = 1;
            double save = a.number;
            while (save > 0)
            {
                result *= save;
                save--;
            }

            return new TokenOperand(result.ToString());
        }
        public static TokenOperand isprime(Token.Token a)
        {
            if (a.number < 0) throw new InvalidExpressionException("Invalid operation.");
            if (a.number == 1) return new TokenOperand("1");
            if (a.number % 2 == 0) return new TokenOperand("0");
            int d = 3;
            double save = a.number;
            while (d <= Math.Sqrt(save) && save % d != 0)
            {
                d++;
            }
            if (d > Math.Sqrt(save)) return new TokenOperand("1");
            return new TokenOperand("0");

        }
        public static TokenOperand fibo(Token.Token a)
        {
            if (a.number < 0) throw new InvalidExpressionException("Invalid operation.");
            if (a.number == 0) return new TokenOperand("0");
            if (a.number == 1) return new TokenOperand("1");
            int fi = 1;
            int fi_1 = 0;
            for (int i = 2; i <= a.number; i++)
            {
                (fi, fi_1) = (fi + fi_1, fi);
            }

            return new TokenOperand(fi.ToString());
        }

        private static TokenOperand Euclide(Token.Token a, Token.Token b)
        {
            if (b.number == 0) return new TokenOperand(a.Value);
            return Euclide(b, new TokenOperand((a.number % b.number).ToString()));

        }
        public static TokenOperand gcd(Token.Token a, Token.Token b)
        {
            if (b.number == 0) return new TokenOperand(a.Value);
            if (a.number == 0) return new TokenOperand(b.Value);
            if (a.number > b.number) return Euclide(a, b);
            return Euclide(b, a);
        }


    }
}
