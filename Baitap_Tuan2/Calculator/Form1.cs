using System;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        string currentInput = "";
        double resultValue = 0;
        string operationPerformed = "";
        bool operationPending = false;
        bool buttonNow = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void click_button(Object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Text == "." && currentInput.Contains("."))
                return;

            currentInput += button.Text;
            textshow.Text = currentInput;

            if (operationPending)
            {
                textshow.Text = resultValue + " " + operationPerformed + " " + currentInput;
            }
            else
            {
                textshow.Text = currentInput;
            }
        }

        private void click_operator(Object sender, EventArgs e)
        {
            Button button = (Button)sender;
            operationPerformed = button.Text;

            if (!string.IsNullOrEmpty(currentInput))
            {
                if (operationPending)
                {
                    PerformCalculation(); 
                }

                resultValue = double.Parse(currentInput);
                currentInput = "";
                operationPending = true;

                textshow.Text = resultValue + " " + operationPerformed;
            }
        }

        private void clear_operator(Object sender, EventArgs e)
        {
            ResetCalculator();
        }

        private void buttonCE_Click(object sender, EventArgs e)
        {
            if (currentInput.Length > 0)
            {
                currentInput = currentInput.Remove(currentInput.Length - 1);

                if (string.IsNullOrEmpty(currentInput))
                {
                    textshow.Text = operationPending ? $"{resultValue} {operationPerformed}" : "0";
                }
                else
                {
                    textshow.Text = operationPending ? $"{resultValue} {operationPerformed} {currentInput}" : currentInput;
                }
            }
        }


        private void click_equal(Object sender, EventArgs e)
        {
            if (operationPending)
            {
                PerformCalculation();

                if (textshow.Text == "ERROR")
                {
                    return;
                }

                textshow.Text = resultValue.ToString();
                operationPending = false;
            }
        }

        private void ResetCalculator()
        {
            currentInput = "";
            resultValue = 0;
            operationPerformed = "";
            operationPending = false;
            textshow.Text = "0";
        }

        private void PerformCalculation()
        {
            double secondNumber;
            if (double.TryParse(currentInput, out secondNumber))
            {
                switch (operationPerformed)
                {
                    case "+":
                        resultValue += secondNumber;
                        break;
                    case "-":
                        resultValue -= secondNumber;
                        break;
                    case "*":
                        resultValue *= secondNumber;
                        break;
                    case "/":
                        if (secondNumber != 0)
                        {
                            resultValue /= secondNumber;
                        }
                        else
                        {
                            textshow.Text = "ERROR";
                            return;
                        }
                        break;
                }
            }
            else
            {
                textshow.Text = "Invalid Input";
                ResetCalculator();
            }

            currentInput = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textshow_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
