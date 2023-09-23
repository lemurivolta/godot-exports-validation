namespace LemuRivolta.ExportsValidation;

using System;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public partial class ValidateNonEmptyAttribute : NodeValidationBaseAttribute
{
    public bool NoWhiteSpace { get; set; } = true;

    public override ValidationError? Validate(ValidationInfo validationInfo) =>
        validationInfo.Value == null ? new CannotBeNullValidationError() :
        validationInfo.Value is not string s ? new MustBeStringValidationError() :
        string.IsNullOrEmpty(s) ? new CannotBeEmptyValidationError() :
        (NoWhiteSpace && string.IsNullOrWhiteSpace(s)) ? new CannotBeWhitespaceValidationError() :
        null;

    internal class CannotBeNullValidationError : ValidationError
    {
        public CannotBeNullValidationError() : base("cannot be null") { }
    }

    internal class MustBeStringValidationError : ValidationError
    {
        public MustBeStringValidationError() : base("must be a string") { }
    }

    internal class CannotBeEmptyValidationError : ValidationError
    {
        public CannotBeEmptyValidationError() : base("cannot be empty") { }
    }

    internal class CannotBeWhitespaceValidationError : ValidationError
    {
        public CannotBeWhitespaceValidationError() : base("cannot be only white spaces") { }
    }
}
