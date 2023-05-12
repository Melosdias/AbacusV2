using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Token
{
    /**
* <summary>Create the token for operators</summary>
*/
    public class TokenOperator : Token
    {
        public override string AllowedChars => "=+*-/^%";
        public int Precedence;


        public TokenOperator(string c) : base(c)
        {
            if (c == "+" || c == "-") Precedence = 1;
            else
            {
                if (c == "*" || c == "/" || c == "%") Precedence = 2;
                else
                {
                    if (c is "=") Precedence = 0;
                    else Precedence = 3;
                }

            }
        }

        /**
     * <summary>Simply compute nb1 with nb2 </summary>
     * <param name="nbLeft">The first number, the left one</param>
     * <param name="nbRight">The second number, the right one</param>
     * <returns>The results</returns>
     * <exception cref="Forbiden operation">The operations is not allowed (example : division by zero)</exception>
     */
        public int Compute(int nbRight, int nbLeft = 0)
        {
            Console.WriteLine($"Operator : {this.Value}, nbRight = {nbRight}, nbLeft = {nbLeft}");
            switch (Value)
            {
                case "+": return nbLeft + nbRight;
                case "-": return nbLeft - nbRight;
                case "*": return nbLeft * nbRight;
                case "^": return (int)Math.Pow(nbLeft, nbRight);
                case "/":
                    if (nbRight != 0) return nbLeft / nbRight;
                    else
                    {
                        throw new DivideByZeroException("Division by zero.");
                    }

                case "%":
                    if (nbRight != 0) return nbLeft % nbRight;
                    else
                    {
                        throw new DivideByZeroException();
                    }
            }

            return 0; 
        }
    }
}
