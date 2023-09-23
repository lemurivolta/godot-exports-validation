namespace LemuRivolta.ExportsValidation;

using System;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class ValidateNonNullAttribute : NodeValidationBaseAttribute
{
    public override ValidationError? Validate(ValidationInfo validationInfo) =>
        validationInfo.Value == null ? new CannotBeNullValidationError() : null;

    internal class CannotBeNullValidationError : ValidationError
    {
        public CannotBeNullValidationError() : base("cannot be null") { }
    }
}
