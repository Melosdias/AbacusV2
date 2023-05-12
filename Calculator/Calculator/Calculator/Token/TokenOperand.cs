using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Token
{
    /**
 * <summary>Create the token for operators. For now, only accept int</summary>
 */
    public class TokenOperand : Token
    {
        public override string AllowedChars => "+-0123456789,";
        public int number;


        public TokenOperand(string c) : base(c)
        {
            foreach (char car in c)
            {
                if (!AllowedChars.Contains(car))
                {
                    throw new ArgumentException($"{c} is invalid operand");
                }
            }

            if (c.Contains('-'))
            {
                number = -1 * Int32.Parse(c.Substring(1));
            }
            else
            {
                if (c.Contains('+'))
                {
                    number = Int32.Parse(c.Substring(1));
                }
                else
                {
                    try
                    {
                        number = Int32.Parse(c);
                    }
                    catch (Exception e)
                    {
                        number = (int)Math.Round(Double.Parse(c));
                    }
                }
            }
        }
    }
}
