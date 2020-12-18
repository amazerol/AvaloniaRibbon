using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Styling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AvaloniaUI.Ribbon
{
    /*public enum ArithmeticOperation
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }

    public class DoubleArithmeticConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result = (double)value;
            DoubleArithmetics param = null;
            
            if (parameter is DoubleArithmetics aris)
                param = aris;
            else if (parameter is DoubleArithmetic ari)
            {
                param = new DoubleArithmetics()
                {
                    ari
                };
            }
            else
                param = new DoubleArithmetics();

            foreach (DoubleArithmetic ar in param)
            {
                if (ar.Operation == ArithmeticOperation.Add)
                    result += ar.Value;
                else if (ar.Operation == ArithmeticOperation.Subtract)
                    result -= ar.Value;
                else if (ar.Operation == ArithmeticOperation.Multiply)
                    result *= ar.Value;
                else if (ar.Operation == ArithmeticOperation.Divide)
                    result /= ar.Value;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine("DoubleArithmeticConverter does not implement ConvertBack!");
            throw new NotImplementedException();
        }
    }

    public class DoubleArithmetic : AvaloniaObject
    {
        public static readonly StyledProperty<double> ValueProperty = AvaloniaProperty.Register<DoubleArithmetic, double>(nameof(Value), 0.0);
        public double Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly StyledProperty<ArithmeticOperation> OperationProperty = AvaloniaProperty.Register<DoubleArithmetic, ArithmeticOperation>(nameof(Operation), ArithmeticOperation.Add);
        public ArithmeticOperation Operation
        {
            get => GetValue(OperationProperty);
            set => SetValue(OperationProperty, value);
        }
    }

    public class DoubleArithmetics : List<DoubleArithmetic>{}*/

}