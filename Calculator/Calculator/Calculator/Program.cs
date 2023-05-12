using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    internal static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        public static string Compute(string args)
        {
            Console.WriteLine($"The input is {args}");
            Console.WriteLine($"test {rpn.ComputeRpn(rpn.TranslateRpn("3 3 sqrt sqrt *"))}");
            Console.WriteLine($"test2 {rpn.ComputeRpn(rpn.TranslateRpn("3 sqrt 3 sqrt *"))}");
            try
            {
                return $"{rpn.ComputeRpn(Ast.ShuntingYard(Ast.TranslateArithmetic(args)))}";
            }

            //Incorrect argument
            catch (ArgumentException e)
            {
                return e.Message;
            }
            //Various error
            catch (InvalidOperationException e)
            {
                return e.Message;
            }
            catch (SyntaxErrorException e)
            {
                return e.Message;
            }
            //Arithmetic error
            catch (DivideByZeroException e)
            {
                return e.Message;
            }
            catch (AuthenticationException e)
            {
                return e.Message;
            }
            catch (InvalidExpressionException e)
            {
                return e.Message;
            }
        }
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

