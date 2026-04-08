using System;

namespace CatCalculate.Logic
{
    /// <summary>
    /// Core calculator logic supporting basic arithmetic operations,
    /// percentage, sign toggle, and clear functionality.
    /// </summary>
    public class Calculator
    {
        private double _currentValue;
        private double _previousValue;
        private string? _pendingOperation;
        private bool _isNewInput;
        private bool _hasDecimal;
        private int _decimalPlaces;

        public Calculator()
        {
            Clear();
        }

        /// <summary>Gets the current display value.</summary>
        public double CurrentValue => _currentValue;

        /// <summary>Gets the pending operation, or null if none.</summary>
        public string? PendingOperation => _pendingOperation;

        private void ResetInputState()
        {
            _isNewInput = true;
            _hasDecimal = false;
            _decimalPlaces = 0;
        }

        /// <summary>Clears all state and resets the calculator.</summary>
        public void Clear()
        {
            _currentValue = 0;
            _previousValue = 0;
            _pendingOperation = null;
            ResetInputState();
        }

        /// <summary>
        /// Inputs a single digit (0–9) into the current value.
        /// </summary>
        /// <param name="digit">A digit between 0 and 9.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="digit"/> is not in [0, 9].
        /// </exception>
        public void InputDigit(int digit)
        {
            if (digit < 0 || digit > 9)
                throw new ArgumentOutOfRangeException(nameof(digit), "Digit must be between 0 and 9.");

            if (_isNewInput)
            {
                _currentValue = digit;
                _hasDecimal = false;
                _decimalPlaces = 0;
                _isNewInput = false;
            }
            else if (_hasDecimal)
            {
                _decimalPlaces++;
                double fractionalPart = digit / Math.Pow(10, _decimalPlaces);
                _currentValue = _currentValue < 0
                    ? _currentValue - fractionalPart
                    : _currentValue + fractionalPart;
            }
            else
            {
                _currentValue = _currentValue < 0
                    ? _currentValue * 10 - digit
                    : _currentValue * 10 + digit;
            }
        }

        /// <summary>
        /// Begins entering digits after a decimal point.
        /// Has no effect if a decimal has already been entered.
        /// </summary>
        public void InputDecimal()
        {
            if (_hasDecimal)
                return;

            if (_isNewInput)
            {
                _currentValue = 0;
                _isNewInput = false;
            }

            _hasDecimal = true;
        }

        /// <summary>
        /// Sets the pending arithmetic operation (+, -, *, /).
        /// If a previous operation is pending and new input has been provided,
        /// the pending operation is evaluated first (chaining).
        /// </summary>
        /// <param name="operation">One of "+", "-", "*", "/".</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="operation"/> is not a recognised operator.
        /// </exception>
        public void SetOperation(string operation)
        {
            if (operation != "+" && operation != "-" && operation != "*" && operation != "/")
                throw new ArgumentException($"Unsupported operation: '{operation}'.", nameof(operation));

            // Chain operations: evaluate before storing the new one.
            if (_pendingOperation != null && !_isNewInput)
            {
                Calculate();
            }

            _previousValue = _currentValue;
            _pendingOperation = operation;
            ResetInputState();
        }

        /// <summary>
        /// Evaluates the pending operation using the stored previous value and
        /// the current value, then updates <see cref="CurrentValue"/> with the result.
        /// Returns the current value unchanged when there is no pending operation.
        /// </summary>
        /// <returns>The result of the calculation.</returns>
        /// <exception cref="DivideByZeroException">
        /// Thrown when dividing by zero.
        /// </exception>
        public double Calculate()
        {
            if (_pendingOperation == null)
                return _currentValue;

            double result;
            switch (_pendingOperation)
            {
                case "+":
                    result = _previousValue + _currentValue;
                    break;
                case "-":
                    result = _previousValue - _currentValue;
                    break;
                case "*":
                    result = _previousValue * _currentValue;
                    break;
                case "/":
                    if (_currentValue == 0)
                        throw new DivideByZeroException("Cannot divide by zero.");
                    result = _previousValue / _currentValue;
                    break;
                default:
                    return _currentValue;
            }

            _currentValue = result;
            _previousValue = 0;
            _pendingOperation = null;
            ResetInputState();
            return result;
        }

        /// <summary>
        /// Converts the current value to its percentage equivalent (divides by 100).
        /// </summary>
        public void Percentage()
        {
            _currentValue /= 100;
            ResetInputState();
        }

        /// <summary>
        /// Toggles the sign of the current value (positive ↔ negative).
        /// </summary>
        public void ToggleSign()
        {
            _currentValue = -_currentValue;
            ResetInputState();
        }

        /// <summary>
        /// Directly sets the current value.
        /// Used by the UI for backspace and clear-entry operations.
        /// </summary>
        /// <param name="value">The value to set as the current operand.</param>
        public void SetCurrentValue(double value)
        {
            _currentValue = value;
            _hasDecimal = value != Math.Floor(value);
            _decimalPlaces = 0;
            _isNewInput = false;
        }
    }
}
