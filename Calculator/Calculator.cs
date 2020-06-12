using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfApp3
{
    enum Operation { Add, Div, Sub, Mul, Sqrt, Pow, Drob, Sin, Cos }

    delegate void CalculatorDidUpdateOutput(Calculator sender, double value, int precision);

    class Calculator
    {
        double? left = null;
        double? right = null;
        Operation? currentOp = null;
        bool decimalPoint = false;
        int precision = 0;

        public event CalculatorDidUpdateOutput DidUpdateValue;
        public event EventHandler<string> InputError;
        public event EventHandler<string> CalculationError;

        public void AddDigit(int digit)
        {
            if (left.HasValue && Math.Log10(left.Value) > 10)
            {
                InputError?.Invoke(this, "Input overflow");
                return;
            }

            if (precision > 10)
            {
                InputError?.Invoke(this, "Input overflow");
                return;
            }

            if (!decimalPoint)
            {
                left = (left ?? 0) * 10 + digit;
            }
            else
            {
                precision += 1;
                left = left + (Math.Pow(0.1, precision) * digit);
            }

            DidUpdateValue?.Invoke(this, left.Value, precision);
        }

        public void AddDecimalPoint()
        {
            decimalPoint = true;
            DidUpdateValue?.Invoke(this, left.Value, precision);
        }

        public void AddOperation(Operation op)
        {
            if (left.HasValue && currentOp.HasValue)
            {
                decimalPoint = false;
                Compute();
            }
            if (!right.HasValue)
            {
                right = left;
                left = 0;
                precision = 0;
                decimalPoint = false;
                Compute();
                DidUpdateValue.Invoke(this, left.Value, precision);
            }

            currentOp = op;
        }

        public void Compute()
        {
            switch (currentOp)
            {
                case Operation.Add:
                    right = left + right;
                    left = null;
                    break;
                case Operation.Sub:
                    right -= left;
                    left = null;
                    break;
                case Operation.Mul:
                    right = left * right;
                    left = null;
                    break;
                case Operation.Div:
                    if (left == 0)
                    {
                        CalculationError?.Invoke(this, "Division by 0!");
                        return;
                    }
                    right /= left;
                    left = null;
                    break;
                case Operation.Pow:
                    right *= right;
                    left = null;
                    break;
                case Operation.Sqrt:
                    right = Math.Sqrt((double)right);
                    left = null;
                    break;
                case Operation.Drob:
                    right = 1 / right;
                    left = null;
                    break;
                case Operation.Sin:
                    right = Math.Sin((double)right);
                    left = null;
                    break;
                case Operation.Cos:
                    right = Math.Cos((double)right);
                    left = null;
                    break;

            }
            DidUpdateValue?.Invoke(this, right.Value, precision);
        }
        public void Clear()
        {
            left = 0;
            DidUpdateValue?.Invoke(this, left.Value, precision);
        }
        public void Reset()
        {
            precision = 0;
            right = null;
            left = 0;
            decimalPoint = false;
            DidUpdateValue?.Invoke(this, left.Value, precision);
            currentOp = null;
        }
        public void Earse()
        {
            if (left.HasValue)
            {
                if (precision > 0)
                {
                    left *= Math.Pow(10, precision);
                    left = (left ?? 0) / 10 - (left ?? 0) % 10 / 10;
                    precision--;
                    left /= Math.Pow(10, precision);
                    DidUpdateValue?.Invoke(this, left.Value, precision);

                }
                else
                {
                    left = (left ?? 0) / 10 - (left ?? 0) % 10 / 10;
                    decimalPoint = false;
                    DidUpdateValue?.Invoke(this, left.Value, precision);

                }
            }
        }
    }
}
