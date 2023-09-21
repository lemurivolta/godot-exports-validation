namespace LemuRivolta.ExportValidation;

using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public partial class ValidateNotEmptyAttribute : NodeValidationBaseAttribute
{
    public override void Validate(ValidationInfo validationInfo)
    {
        if (validationInfo.Value == null)
        {
            throw new ValidationFailedException("is null");
        }
        if (validationInfo.Value is not string s)
        {
            throw new ValidationFailedException("can only check strings");
        }
        if (string.IsNullOrEmpty(s))
        {
            throw new ValidationFailedException("is empty");
        }
    }
}
