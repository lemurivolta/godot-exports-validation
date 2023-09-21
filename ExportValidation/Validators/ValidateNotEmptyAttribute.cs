namespace LemuRivolta.ExportValidation;

using System;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public partial class ValidateNotEmptyAttribute : NodeValidationBaseAttribute
{
    public bool NoWhiteSpace { get; set; } = true;

    public override ValidationError? Validate(ValidationInfo validationInfo) =>
        validationInfo.Value == null ? new("is null") :
        validationInfo.Value is not string s ? new("can only check strings") :
        string.IsNullOrEmpty(s) ? new("is empty") :
        (NoWhiteSpace && string.IsNullOrWhiteSpace(s)) ? new("is whitespace") :
        null;

}
