using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Token
{
    /**
     * <summary>
     * This class creates tokens that will be the translation of the expression of the user
     * </summary>
     */
    public abstract class Token
    {
        /**
         * <summary>
         * The char that represents the operator or the operand
         * </summary>
         */
        public string Value { get; }

        /**
         * <summary>
         * String that contains all the possible char for this dot
         * </summary>
         */
        public abstract string AllowedChars { get; }

        /**
         * <summary>If the token is not a variable or an operand, number is null. Else, it is just the number of variable or the operand</summary>
         */
        public int number;

        /**
         * <summary>If the token is not a function, nbOperand is null. Else, it is just the number of operand needed for a function</summary>
         */
        public int nbOperand;

        /**
         * <summary>If the token is not an operator, precedence is null. Else, it is the precedence of the operator</summary>
         */
        public int precedence;


        /**
     * <summary>Create a new Token from the char.</summary>
     * Check if the char is allowed
     * <param  name = "c">The char that define the token</param>
     * <exception cref="InvalidToken">The char is not allowed</exception>
     */
        protected Token(string c)
        {
            Value = AllowedChars is null
                ? throw new ArgumentException($"{c} is unvalid char")
                : c;
            if (this is TokenOperator)
            {
                if (c is "+" || c is "-") precedence = 1;
                else
                {
                    if (c is "*" || c is "/" || c is "%") precedence = 2;
                    else precedence = 3;

                }
            }

            if (this is TokenFunctions)
            {
                if (this.Value == "sqrt" || this.Value =="facto" || this.Value =="isprime" || this.Value =="fibo") this.nbOperand = 1;
                else this.nbOperand = 2;
            }

            if (this is TokenOperand)
            {
                Console.WriteLine($"Number du token actuel (value {this.Value}) : {this.Value}");
                try
                {
                    this.number = Int32.Parse(this.Value);
                }
                catch (Exception) {
                    this.number = (int)Math.Round(Double.Parse(this.Value), 0);
                }

            }

            if (this is TokenVariable)
            {
                this.number = Int32.MaxValue;
            }
        }
    }
}
