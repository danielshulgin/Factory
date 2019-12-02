using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace Factory
{
    class NameValidationRule : ValidationRule
    {
        public bool IsLengthConstrains { get; set; }

        public int MaxLength { get; set; }
        public int MinLength { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string name = value.ToString();

            if (IsLengthConstrains)
            {
                if (name.Length < MinLength)
                {
                    var msg = $"Name must be more than {MinLength} digits long.";
                    return new ValidationResult(false, msg);
                }
                else if (name.Length > MaxLength)
                {
                    var msg = $"Name must be fewer than {MaxLength} digits long.";
                    return new ValidationResult(false, msg);
                }
                
            }

            // Number is valid
            return new ValidationResult(true, null);
        }
    }
}
