namespace LemuRivolta.ExportValidation;

using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class ValidateNonNullAttribute : NodeValidationBaseAttribute
{
    public override void Validate(ValidationInfo validationInfo)
    {
        if(validationInfo.Value == null)
        {
            throw new Exception($"cannot be null");
        }
    }
}
