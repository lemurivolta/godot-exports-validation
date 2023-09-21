namespace LemuRivolta.ExportValidation;

using System;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class ValidateNonNullAttribute : NodeValidationBaseAttribute
{
    public override ValidationError? Validate(ValidationInfo validationInfo) =>
        validationInfo.Value == null ? new("cannot be null") : null;
}
