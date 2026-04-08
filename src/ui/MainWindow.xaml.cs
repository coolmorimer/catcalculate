using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using CatCalculate.Logic;

namespace CatCalculate
{
    public partial class MainWindow : Window
    {
        private readonly Calculator _calculator = new();

        // Tracks whether the display currently shows an error message.
        private bool _hasError;

        public MainWindow()
        {
            InitializeComponent();
            UpdateDisplay();
        }

        // ----------------------------------------------------------------
        // Window chrome
        // ----------------------------------------------------------------

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // ----------------------------------------------------------------
        // Display helpers
        // ----------------------------------------------------------------

        private void UpdateDisplay(string? expression = null)
        {
            double value = _calculator.CurrentValue;

            // Show up to 12 significant digits to avoid floating-point noise.
            string text = value == Math.Floor(value) && Math.Abs(value) < 1e12
                ? value.ToString("0", CultureInfo.InvariantCulture)
                : value.ToString("G12", CultureInfo.InvariantCulture);

            ResultDisplay.Text = text;
            ExpressionDisplay.Text = expression ?? string.Empty;
        }

        private void ShowError(string message)
        {
            ResultDisplay.Text = message;
            ExpressionDisplay.Text = string.Empty;
            _hasError = true;
        }

        // ----------------------------------------------------------------
        // Button handlers – utility row
        // ----------------------------------------------------------------

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _hasError = false;
            _calculator.Clear();
            UpdateDisplay();
        }

        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError)
            {
                _hasError = false;
                _calculator.Clear();
                UpdateDisplay();
                return;
            }

            // Reset only the current entry by pushing 0.
            _calculator.Clear();
            UpdateDisplay(ExpressionDisplay.Text);
        }

        private void ToggleSign_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError) return;
            _calculator.ToggleSign();
            UpdateDisplay(ExpressionDisplay.Text);
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError)
            {
                _hasError = false;
                _calculator.Clear();
                UpdateDisplay();
                return;
            }

            string current = ResultDisplay.Text;
            if (current.Length <= 1 || (current.Length == 2 && current[0] == '-'))
            {
                _calculator.SetCurrentValue(0);
                ResultDisplay.Text = "0";
                return;
            }

            string trimmed = current[..^1].TrimEnd('.');

            if (double.TryParse(trimmed, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsed))
            {
                _calculator.SetCurrentValue(parsed);
                ResultDisplay.Text = trimmed;
            }
            else
            {
                _calculator.SetCurrentValue(0);
                ResultDisplay.Text = "0";
            }
        }

        // ----------------------------------------------------------------
        // Button handlers – digits and decimal
        // ----------------------------------------------------------------

        private void Digit_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError) return;

            if (sender is System.Windows.Controls.Button btn &&
                btn.Tag is string tagStr &&
                int.TryParse(tagStr, out int digit))
            {
                _calculator.InputDigit(digit);
                UpdateDisplay(ExpressionDisplay.Text);
            }
        }

        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError) return;
            _calculator.InputDecimal();

            // Show a trailing dot while the user is entering the decimal part.
            string current = ResultDisplay.Text;
            if (!current.Contains('.'))
                ResultDisplay.Text = current + ".";
        }

        // ----------------------------------------------------------------
        // Button handlers – operators
        // ----------------------------------------------------------------

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError) return;

            if (sender is not System.Windows.Controls.Button btn) return;
            string symbol = btn.Tag as string ?? btn.Content as string ?? string.Empty;

            // Map display symbols to internal operator strings.
            string op = symbol switch
            {
                "÷" => "/",
                "×" => "*",
                "−" => "-",
                "+" => "+",
                _ => symbol
            };

            try
            {
                _calculator.SetOperation(op);
                ExpressionDisplay.Text = $"{ResultDisplay.Text} {symbol}";
                UpdateDisplay($"{ResultDisplay.Text} {symbol}");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void Percent_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError) return;
            _calculator.Percentage();
            UpdateDisplay(ExpressionDisplay.Text);
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            if (_hasError) return;

            string expression = ExpressionDisplay.Text.TrimEnd() + " " + ResultDisplay.Text + " =";

            try
            {
                double result = _calculator.Calculate();
                UpdateDisplay(expression);
            }
            catch (DivideByZeroException)
            {
                ShowError("Cannot ÷ 0");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
    }
}
