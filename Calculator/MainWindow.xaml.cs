using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calculator calc;
        public MainWindow()
        {
            InitializeComponent();
            calc = new Calculator();
            calc.DidUpdateValue += Calc_DidUpdate;
            calc.InputError += Calc_Error;
            calc.CalculationError += Calc_Error;
        }

        private void Calc_Error(object sender, string e)
        {
            MessageBoxResult result = MessageBox.Show("Calculator Error");
        }

        private void Calc_DidUpdate(Calculator sender, double value, int precision)
        {
            if (precision > 0)
                output.Text = String.Format("{0:F" + precision + "}", value);
            else
                output.Text = $"{value}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int digit = -1;
            object tag = (sender as Button).Tag;
       

            if (int.TryParse(button.Content.ToString(), out digit))
            {
                calc.AddDigit(digit);
            }
            else
            {
                switch (tag)
                {
                    case "decimal":
                        calc.AddDecimalPoint();
                        break;
                    case "evaluate":
                        calc.Compute();
                        break;
                    case "addition":
                        calc.AddOperation(Operation.Add);
                        break;
                    case "substraction":
                        calc.AddOperation(Operation.Sub);
                        break;
                    case "multiplication":
                        calc.AddOperation(Operation.Mul);
                        break;
                    case "division":
                        calc.AddOperation(Operation.Div);
                        break;
                    case "degree":
                        calc.AddOperation(Operation.Pow);
                        break;
                    case "reverse":
                        calc.AddOperation(Operation.Drob);
                        break;
                    case "radical":
                        calc.AddOperation(Operation.Sqrt);
                        break;
                    case "Cos":
                        calc.AddOperation(Operation.Cos);
                        break;
                    case "Sin":
                        calc.AddOperation(Operation.Sin);
                        break;
                    case "clear":
                        calc.Clear();
                        break;
                    case "reset":
                        calc.Reset();
                        break;
                    case "earse":
                        calc.Earse();
                        break;
                }
            }
        }
    }
}
