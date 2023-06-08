using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Token;

namespace Calculator
{
    internal class rpn
    {
        /**
 * <summary>This is the class for rpn operations</summary>
 */
        /**
     * <summary>First, we need to translate the string</summary>
     * <param name="input">The string of the user</param>
     * 
     */
        public static List<Token.Token> TranslateRpn(string input)
        {
            List<Token.Token> tokens = new List<Token.Token>();
            Token.Token testOperand = new TokenOperand("1");
            Token.Token testOperator = new TokenOperator("+");

            string numb = "";
            if (input.Length == 1)
            {
                if (testOperand.AllowedChars.Contains(input))
                {
                    tokens.Add(new TokenOperand(input));
                    return tokens;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            int index = 0;
            while (index < input.Length)
            {
                char car = input[index];
                #region Void
                if (car == ' ' || car == '\n' || car == '\t')
                {
                    if (numb != "")
                    {
                        tokens.Add(new TokenOperand(numb));
                        numb = "";
                    }
                    index++;
                    continue;
                }
                #endregion
                #region operator
                if (testOperator.AllowedChars.Contains(car))
                {
                    if (numb != "")
                    {
                        tokens.Add(new TokenOperand(numb));
                        numb = "";
                        tokens[tokens.Count - 1].number = Int32.Parse(tokens[tokens.Count - 1].Value);
                    }
                    tokens.Add(new TokenOperator(car.ToString()));

                    index++;
                    continue;
                }
                #endregion
                #region operand
                if (testOperand.AllowedChars.Contains(car))
                {
                    numb += car;

                    index++;
                }
                #endregion
                #region Otherwise
                else
                {
                    if (numb != "")
                    {
                        tokens.Add(new TokenOperand(numb));
                        numb = "";
                    }
                    string nameVar = "";
                    while (index < input.Length && input[index] != ' '
                                                && !testOperator.AllowedChars.Contains(input[index])) //a* <=> a * so a* is not a variable
                    {
                        nameVar += input[index];
                        index++;
                    }

                    switch (nameVar)
                    {
                        case "gcd":
                            tokens.Add(new TokenFunctions("gcd"));
                            continue;
                        case "max":
                            tokens.Add(new TokenFunctions("max"));
                            continue;
                        case "min":
                            tokens.Add(new TokenFunctions("min"));
                            continue;
                        case "fibo":
                            tokens.Add(new TokenFunctions("fibo"));
                            continue;
                        case "facto":
                            tokens.Add(new TokenFunctions("facto"));
                            continue;
                        case "sqrt":
                            tokens.Add(new TokenFunctions("sqrt"));
                            continue;
                        case "isprime":
                            tokens.Add(new TokenFunctions("isprime"));
                            continue;
                        default:
                            TokenVariable var = new TokenVariable(nameVar);
                            if (index == input.Length)
                            {
                                bool exist = false;
                                foreach (var token in tokens)
                                {
                                    if (token is TokenVariable)
                                    {
                                        if (token.Value == nameVar)
                                        {
                                            exist = true;
                                            var = (TokenVariable)token;
                                            break;
                                        }
                                    }
                                }
                                if (!exist) throw new InvalidExpressionException("Unbound variable.");
                            }
                            tokens.Add(new TokenVariable(nameVar));
                            break;
                    }
                }
                #endregion
            }

            if (numb != "")
            {
                TokenOperand number = new TokenOperand(numb);
                tokens.Add(number);
            }
            return tokens;
        }

        /**
     * Compute the token list
         * <param name="input">The user's string converts to a list. Is in rpn</param>
         * <returns>The result of the expression</returns>
     */
        public static double ComputeRpn(List<Token.Token> input)
        {
            Stack<Token.Token> operand = new Stack<Token.Token>();
            foreach (Token.Token token in input)
            {
                if (token is TokenOperand)
                {
                    operand.Push((Token.Token)token);
                    continue;
                }
                if (token is TokenVariable)
                {
                    bool test = true;
                    foreach (var tok in operand)
                    {
                        if (tok is TokenVariable)
                        {
                            if (tok.Value == token.Value)
                            {
                                token.number = tok.number;
                                test = false;
                                break;
                            }
                        }
                    }
                    if (test)
                    {
                        operand.Push((Token.Token)token);
                    }
                    continue;
                }
                if (token is TokenOperator)
                {
                    if (operand.Count < 2)
                    {
                        throw new SyntaxErrorException("Syntax error.");
                    }
                    if (token.Value == "=")
                    {
                        Token.Token tok = operand.Pop();
                        if (tok is TokenVariable)
                        {
                            if (tok.number == Int32.MaxValue) throw new InvalidExpressionException("Unbound variable.");
                        }
                        int value = tok.number;
                        try
                        {
                            TokenVariable var = (TokenVariable)operand.Pop();
                            var.number = value;
                            operand.Push((Token.Token)var);
                            continue;
                        }
                        catch (Exception e)
                        {
                            throw new SyntaxErrorException("Syntax error.");
                        }

                    }
                    else
                    {
                        if (operand.Peek() is TokenVariable && operand.Peek().number == Int32.MaxValue) throw new InvalidExpressionException("Unbound variable.");
                        int nb1 = operand.Pop().number;
                        if (operand.Peek() is TokenVariable && operand.Peek().number == Int32.MaxValue) throw new InvalidExpressionException("Unbound variable.");
                        int nb2 = operand.Pop().number;
                        string result = ((TokenOperator)token).Compute(nb1, nb2).ToString();
                        operand.Push(new TokenOperand(result));
                        continue;
                    }
                }
                if (token is TokenFunctions)
                {
                    if (operand.Count == 0) throw new SyntaxErrorException("Syntax error.");

                    switch (token.Value)
                    {
                        //Needs one parameter
                        case "sqrt":
                            if (operand.Peek() is TokenVariable && operand.Peek().number == Int32.MaxValue) throw new InvalidExpressionException("Unbound variable.");
                            operand.Push(TokenFunctions.Sqrt(operand.Pop()));
                            break;
                        case "facto":
                            if (operand.Peek() is TokenVariable && operand.Peek().number == Int32.MaxValue) throw new InvalidExpressionException("Unbound variable.");
                            operand.Push(TokenFunctions.facto(operand.Pop()));
                            break;
                        case "isprime":
                            if (operand.Peek() is TokenVariable && operand.Peek().number == Int32.MaxValue) throw new InvalidExpressionException("Unbound variable.");
                            operand.Push(TokenFunctions.isprime(operand.Pop()));
                            break;
                        case "fibo":
                            if (operand.Peek() is TokenVariable && operand.Peek().number == Int32.MaxValue) throw new InvalidExpressionException("Unbound variable.");
                            operand.Push((TokenFunctions.fibo(operand.Pop())));
                            break;
                        default:
                            //Needs two parameters
                            if (operand.Count == 1) throw new SyntaxErrorException("Syntax error.");
                            switch (token.Value)
                            {
                                case "max":
                                    if (operand.Peek() is TokenVariable && operand.Peek().number == Int32.MaxValue) throw new InvalidExpressionException("Unbound variable.");
                                    Token.Token a = operand.Pop();
                                    if (operand.Peek() is TokenVariable && operand.Peek().number == Int32.MaxValue) throw new InvalidExpressionException("Unbound variable.");
                                    Token.Token b = operand.Pop();
                                    operand.Push(TokenFunctions.max(a, b));
                                    break;
                                case "min":
                                    if (operand.Peek() is TokenVariable && operand.Peek().number == Int32.MaxValue) throw new InvalidExpressionException("Unbound variable.");
                                    Token.Token nb1 = operand.Pop();
                                    if (operand.Peek() is TokenVariable && operand.Peek().number == Int32.MaxValue) throw new InvalidExpressionException("Unbound variable.");
                                    Token.Token nb2 = operand.Pop();
                                    operand.Push(TokenFunctions.min(nb1, nb2));
                                    break;
                                case "gcd":
                                    if (operand.Peek() is TokenVariable && operand.Peek().number == Int32.MaxValue) throw new InvalidExpressionException("Unbound variable.");
                                    Token.Token a1 = operand.Pop();
                                    if (operand.Peek() is TokenVariable && operand.Peek().number == Int32.MaxValue) throw new InvalidExpressionException("Unbound variable.");
                                    Token.Token a2 = operand.Pop();
                                    operand.Push(TokenFunctions.gcd(a1, a2));
                                    break;
                                default: throw new SyntaxErrorException("Syntax error.");
                            }
                            break;
                    }
                }
            }
            if (operand.Count > 1) throw new SyntaxErrorException("Syntax error.");
            Token.Token resultat = operand.Pop();
            return resultat.number;
        }
    }
}

