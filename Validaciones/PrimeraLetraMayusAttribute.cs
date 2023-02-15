using System.ComponentModel.DataAnnotations;

namespace backend.Validaciones
{
    public class PrimeraLetraMayusAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var primeraLetra = value.ToString()[0].ToString(); //Extraigo la primera letra

            if (primeraLetra != primeraLetra.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayuscula");
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}