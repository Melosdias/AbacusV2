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
    * This class creates the AST of the input of the user
    * </summary>
    */
    public class Ast
    {
        private Token.Token node; //Node of the AST
        private Ast left; //The left son
        private Ast right; //The right son
        public Token.Token Node => node;
        public Ast Left => left;
        public Ast Right => right;


        public Ast(Token.Token node, Ast left, Ast right)
        {
            this.node = node;
            this.left = left;
            this.right = right;
        }

        /**
         * <summary>Check the syntax of the fun</summary>
         * <example>input = "max(2 3)" return false whereas input = "max(2,3) return true</example>
         * <returns>A couple (int, bool). The first item is for the recursion part of the functions,
         * th second item says if the syntax is correct or not</returns>
         */
        private static (int, bool) checkSyntaxeFun(List<Token.Token> input, int index)
        {
            bool test = true;
            bool coma = false;
            if (input[index + 1].Value != "(") return (0, false);
            TokenFunctions fun = (TokenFunctions)input[index];
            index += 2;
            while (index <= input.Count && input[index].Value != ")" && test)
            {

                if (input[index].Value == "(")
                {
                    (index, test) = checkSyntaxeFun(input, index - 1);
                }

                if (input[index].Value == ",")
                {
                    coma = true;
                }

                index++;
            }

            if (!coma && fun.nbOperand == 1) coma = true;
            return (index, index < input.Count && input[index].Value == ")" && coma && test);
        }
        /**
         * <summary>Translate the input of the user</summary>
         * <param name="input">The input of the user</param>
         * <returns>The list of token corresponding to the input</returns>
         * <exception cref="ArgumentException">If there is an invalid char in the input or too much operator (for example "3 +++ 4" is incorrect)</exception>
         */
        public static List<Token.Token> TranslateArithmetic(string input)
        {
            List<Token.Token> tokens = new List<Token.Token>();
            Token.Token testOperand = new TokenOperand("1");
            Token.Token testOperator = new TokenOperator("+");
            Token.Token testParenthesis = new TokenParenthesis("(");
            string numb = "";
            char unitaire = '\0'; //Manage unit operator
            char lastOperator = '\0'; //To help with unit operator
            int index = 0;
            while (index < input.Length)
            {
                char car = input[index];
                #region void case
                if (car == ' ' || car == '\t' || car == '\n')
                {
                    if (numb == "")
                    {
                        index++;
                        continue;
                    }

                    if (tokens.Count > 0 && (tokens[tokens.Count - 1] is TokenOperand)) throw new SyntaxErrorException("Syntax error.");
                    if (tokens.Count > 0 && (tokens[tokens.Count - 1] is TokenVariable || tokens[tokens.Count - 1].Value == ")")) tokens.Add(new TokenOperator("*"));
                    if (unitaire != '\0')
                    {
                        numb = unitaire + numb;
                    }
                    TokenOperand number = new TokenOperand(numb);
                    tokens.Add(number);
                    unitaire = '\0';
                    lastOperator = '\0';
                    numb = "";
                    index++;
                    continue;
                }
                #endregion
                #region Operator case
                if (testOperator.AllowedChars.Contains(car))
                {
                    if (numb != "")
                    {
                        if (tokens.Count > 0 && (tokens[tokens.Count - 1] is TokenVariable || tokens[tokens.Count - 1].Value == ")")) tokens.Add(new TokenOperator("*"));
                        if (unitaire != '\0')
                        {
                            numb = unitaire + numb;
                        }
                        TokenOperand number = new TokenOperand(numb);
                        tokens.Add(number);
                        unitaire = '\0';
                        lastOperator = '\0';
                        numb = "";
                    }
                    if (lastOperator != '\0' || index == 0 || tokens[tokens.Count - 1].Value == "(" || tokens[tokens.Count - 1].Value == "," || tokens[tokens.Count - 1].Value == ";") //We want to avoid cases like "3**2" but allow cases like "3++2"
                    {
                        if (car == '-' || car == '+')
                        {
                            unitaire = car;
                            index++;
                            continue;
                        }
                        else throw new SyntaxErrorException("Syntax error.");
                    }

                    if (car == '=') //We want to avoid cases like 5 = 2 or + = 6
                    {
                        if (!(tokens[tokens.Count - 1] is TokenVariable)) throw new SyntaxErrorException("Syntax error.");
                    }
                    tokens.Add(new TokenOperator(car.ToString()));
                    lastOperator = car;
                    index++;
                    continue;
                }
                #endregion
                #region Operand case
                if (testOperand.AllowedChars.Contains(car) && car != ',')
                {
                    numb += car;
                    lastOperator = '\0';
                    index++;
                    continue;
                }
                #endregion
                #region case Parenthesis

                if (testParenthesis.AllowedChars.Contains(car))
                {
                    if (numb != "")
                    {
                        if (tokens.Count > 0 && (tokens[tokens.Count - 1] is TokenVariable || tokens[tokens.Count - 1].Value == ")"))
                            tokens.Add(new TokenOperator("*"));
                        if (unitaire != '\0')
                        {
                            numb = unitaire + numb;
                        }
                        TokenOperand number = new TokenOperand(numb);
                        tokens.Add(number);
                        unitaire = '\0';
                        numb = "";
                    }

                    if (car == '(' && tokens.Count > 0)
                    {
                        if (tokens[tokens.Count - 1] is TokenOperand || tokens[tokens.Count - 1] is TokenVariable || tokens[tokens.Count-1].Value == ")") tokens.Add(new TokenOperator("*")); //Mutiplication implicite
                        if (tokens[tokens.Count - 1].Value == "-")
                        {
                            tokens.Remove(tokens[tokens.Count - 1]);
                            tokens.Add(new TokenOperand("-1"));
                            tokens.Add(new TokenOperator("*"));
                        }
                    }

                    if (car == ')')
                    {
                        if (tokens.Count == 0 || tokens[tokens.Count - 1].Value == "(")
                            throw new SyntaxErrorException("Syntax error.");
                    }
                    tokens.Add(new TokenParenthesis(car.ToString())); //We will manage parenthesis problem later
                    index++;
                }
                #endregion
                #region Otherwise
                else
                {
                    if (numb != "")
                    {
                        if (tokens[tokens.Count - 1] is TokenVariable || tokens[tokens.Count - 1].Value == ")") tokens.Add(new TokenOperator("*"));
                        tokens.Add(new TokenOperand(numb));
                        numb = "";
                    }

                    //If it is not a function, it is a variable 
                    string nameVar = "";

                    while (index < input.Length && input[index] != ' '
                                                && !testOperator.AllowedChars.Contains(input[index]) //a*b <=> a * b so a*b is not a variable
                                                && !testParenthesis.AllowedChars.Contains(input[index])) //a(...) <=> a(...) si a(...) is not a variable
                    {
                        nameVar += input[index];
                        index++;
                    }
                    lastOperator = '\0';
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
                if (unitaire != '\0')
                {
                    numb = unitaire + numb;
                }
                TokenOperand number = new TokenOperand(numb);
                tokens.Add(number);
            }

            index = 0;
            bool test = true;
            while (index < tokens.Count)
            {

                if (tokens[index] is TokenFunctions)
                {
                    (index, test) = checkSyntaxeFun(tokens, index);
                    if (!test) throw new SyntaxErrorException("Syntax error.");
                }

                else index++;
            }
            return tokens;
        }
        /**
         * <summary>Return the AST of the input</summary>
         * <param name="input">The input of the user<Token></param>
         * <returns>An AST</returns>
         */
        public static List<Token.Token> ShuntingYard(List<Token.Token> input)
        {
            List<Token.Token> output = new List<Token.Token>();
            Stack<Token.Token> stack = new Stack<Token.Token>();
            foreach (Token.Token token in input)
            {
                if (token is TokenOperand) //If this is a number
                {
                    output.Add((TokenOperand)token);
                }
                else if (token is TokenVariable)
                {
                    output.Add((TokenVariable)token);
                }
                else if (token is TokenOperator) //If this is an operator
                {
                    while (stack.Count > 0 && stack.Peek() is TokenOperator &&
                           token.precedence < stack.Peek().precedence && stack.Peek().Value != "=")
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Push((TokenOperator)token);
                }
                else if (token is TokenFunctions) //if this is a function
                {

                    stack.Push((TokenFunctions)token);
                }
                else if (token is TokenParenthesis)
                {
                    if (token.Value == "(")
                    {
                        stack.Push(token);
                    }
                    else if (token.Value == ",")
                    {
                        while(stack.Count > 0 && stack.Peek().Value!= "(")
                        {
                            output.Add(stack.Pop());
                        }
                    }
                    else if (token.Value == ";") 
                    {
                        while (stack.Count > 0)
                        {
                            output.Add(stack.Pop());
                        }
                        output.Add((TokenParenthesis)token);
                    }
                    else
                    {
                        while (stack.Count > 0 && stack.Peek().Value != "(")
                        {
                            output.Add(stack.Pop());
                        }

                        stack.Pop(); //Normally, this is a parenthesis
                        if (stack.Count > 0 &stack.Count > 0 && stack.Peek() is TokenFunctions)
                        {
                            output.Add(stack.Pop());
                        }
                    }
                    /*else
                    {
                        throw new SyntaxErrorException("Unbalanced parentheses");
                    }*/
                }
            }


            while (stack.Count > 0)
            {
                if (stack.Peek() is TokenOperand)
                    throw new Exception("Why there is an operand in the operator stack ???");
                if (stack.Peek().Value == "(")
                {
                    throw new SyntaxErrorException("Unbalanced parentheses");
                }
                else
                {
                    output.Add(stack.Pop());
                }
            }
            return output;
        }
    }
}
