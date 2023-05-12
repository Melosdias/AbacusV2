using System;
using System.Windows.Forms;
using Markdig; 

namespace Calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            computeButton.Enabled = (textBox.Text != ""); 
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void computeButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"Button2 clicked and calculation {textBox.Text}");
            label2.Text = Program.Compute(textBox.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string readMe = "**How to use this calculator ?**\n" +
                "Basically write a calculation in the text box, for example 5 + 3\n" +
                "If you want to use special functions, here are the available function . \n" +
                "sqrt : Return the square root (syntax : sqrt(a))\n" +
                "max : Return the greatest number between  and b (syntax : max(a,b))\n" +
                "min : Return the lowest number between a and b\n" +
                "facto : Return the factorial of a number\n" +
                "isprime : Return 1 if the number is prime, 0 otherwise\n" +
                "fibo : Return the n-th term of the Fibonacci sequence\n" +
                "gcd : Return the greatest common divisor between a and b";
            var result = Markdown.ToPlainText(readMe);
            //MessageBox.Show(result);
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }
    }
}
