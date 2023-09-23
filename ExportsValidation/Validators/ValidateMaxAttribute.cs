using System;

namespace LemuRivolta.ExportsValidation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class ValidateMaxAttribute : ValidateRangeAttributeBase
{
    public ValidateMaxAttribute(int max) : base(-1, max, "lesser") { }
}
