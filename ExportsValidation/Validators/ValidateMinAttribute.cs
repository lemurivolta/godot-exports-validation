using System;

namespace LemuRivolta.ExportsValidation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class ValidateMinAttribute : ValidateRangeAttributeBase
{
    public ValidateMinAttribute(int min) : base(1, min, "lesser") { }
}
