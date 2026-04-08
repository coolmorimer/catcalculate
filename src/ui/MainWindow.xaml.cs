using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CatCalculate.Logic;

namespace CatCalculate
{
    public partial class MainWindow : Window
    {
        private readonly Calculator _calculator = new Calculator();

        // Current number string being typed by the user
        private string _displayBuffer = "0";

        // Expression shown above the main result (e.g. "5 +")
        private string _expressionText = "";

        // True right after = was pressed, so next digit starts fresh
        private bool _justCalculated = false;

        public MainWindow()
        {
            InitializeComponent();
            UpdateDisplay();
        }

        // ---------------------------------------------------------------
        // Window chrome
        // ---------------------------------------------------------------

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

        // ---------------------------------------------------------------
        // Display helper
        // ---------------------------------------------------------------

        private void UpdateDisplay()
        {
            ResultDisplay.Text = _displayBuffer;
            ExpressionDisplay.Text = _expressionText;
        }

        // ---------------------------------------------------------------
        // Digit input
        // ---------------------------------------------------------------

        private void Digit_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            string digit = btn.Content?.ToString() ?? "";

            if (_justCalculated)
            {
                _calculator.Clear();
                _displayBuffer = "0";
                _expressionText = "";
                _justCalculated = false;
            }

            if (_displayBuffer == "0" && digit == "0") return;

            if (_displayBuffer == "0")
                _displayBuffer = digit;
            else if (_displayBuffer == "-0")
                _displayBuffer = "-" + digit;
            else
                _displayBuffer += digit;

            SyncBufferToCalculator();
            UpdateDisplay();
        }

        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            if (_justCalculated)
            {
                _calculator.Clear();
                _displayBuffer = "0";
                _expressionText = "";
                _justCalculated = false;
            }

            if (_displayBuffer.Contains('.')) return;

            _displayBuffer += ".";
            UpdateDisplay();
        }

        // ---------------------------------------------------------------
        // Operators
        // ---------------------------------------------------------------

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            string opSymbol = btn.Tag?.ToString() ?? btn.Content?.ToString() ?? "";
            string opChar = SymbolToChar(opSymbol);

            SyncBufferToCalculator();

            _expressionText = FormatNumber(_calculator.CurrentValue) + " " + opSymbol;
            _calculator.SetOperation(opChar);
            _displayBuffer = "0";
            _justCalculated = false;

            UpdateDisplay();
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            SyncBufferToCalculator();

            try
            {
                string pendingExpr = string.IsNullOrEmpty(_expressionText)
                    ? _displayBuffer
                    : _expressionText + " " + _displayBuffer;
                _expressionText = pendingExpr + " =";

                double result = _calculator.Calculate();
                _displayBuffer = FormatNumber(result);
                _justCalculated = true;
            }
            catch (DivideByZeroException)
            {
                _displayBuffer = "Error";
                _expressionText = "Cannot divide by zero";
                _calculator.Clear();
                _justCalculated = true;
            }

            UpdateDisplay();
        }

        // ---------------------------------------------------------------
        // Function buttons
        // ---------------------------------------------------------------

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _calculator.Clear();
            _displayBuffer = "0";
            _expressionText = "";
            _justCalculated = false;
            UpdateDisplay();
        }

        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            _displayBuffer = "0";
            _justCalculated = false;
            _calculator.SetCurrentValue(0);
            UpdateDisplay();
        }

        private void ToggleSign_Click(object sender, RoutedEventArgs e)
        {
            if (_displayBuffer == "0") return;

            _displayBuffer = _displayBuffer.StartsWith('-')
                ? _displayBuffer.Substring(1)
                : "-" + _displayBuffer;

            SyncBufferToCalculator();
            _justCalculated = false;
            UpdateDisplay();
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (_justCalculated) return;

            if (_displayBuffer.Length <= 1
                || (_displayBuffer.Length == 2 && _displayBuffer[0] == '-'))
            {
                _displayBuffer = "0";
            }
            else
            {
                _displayBuffer = _displayBuffer.Substring(0, _displayBuffer.Length - 1);
                if (_displayBuffer == "-")
                    _displayBuffer = "0";
            }

            SyncBufferToCalculator();
            UpdateDisplay();
        }

        private void Percent_Click(object sender, RoutedEventArgs e)
        {
            SyncBufferToCalculator();
            _calculator.Percentage();
            _displayBuffer = FormatNumber(_calculator.CurrentValue);
            _justCalculated = false;
            UpdateDisplay();
        }

        // ---------------------------------------------------------------
        // Helpers
        // ---------------------------------------------------------------

        /// <summary>Parses _displayBuffer and writes it into the calculator.</summary>
        private void SyncBufferToCalculator()
        {
            if (double.TryParse(_displayBuffer,
                                NumberStyles.Any,
                                CultureInfo.InvariantCulture,
                                out double val))
            {
                _calculator.SetCurrentValue(val);
            }
        }

        private static string SymbolToChar(string symbol) => symbol switch
        {
            "÷" => "/",
            "×" => "*",
            "−" => "-",
            "+" => "+",
            _ => symbol
        };

        private static string FormatNumber(double value)
        {
            if (double.IsNaN(value)) return "NaN";
            if (double.IsPositiveInfinity(value)) return "∞";
            if (double.IsNegativeInfinity(value)) return "-∞";

            // Whole numbers within safe integer range → no decimal point
            if (value == Math.Floor(value) && Math.Abs(value) < 1e15)
                return ((long)value).ToString(CultureInfo.InvariantCulture);

            return value.ToString("G10", CultureInfo.InvariantCulture);
        }
    }
}
