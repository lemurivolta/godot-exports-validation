namespace LemuRivolta.ExportValidation;

using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public partial class ValidateNotEmptyAttribute : NodeValidationBaseAttribute
{
    public override void Validate(ValidationInfo validationInfo)
    {
        if (validationInfo.Value == null)
        {
            throw new Exception("is null");
        }
        if (validationInfo.Value is not string s)
        {
            throw new Exception($"can only check strings");
        }
        if (string.IsNullOrEmpty(s))
        {
            throw new Exception("is empty");
        }
    }
}
