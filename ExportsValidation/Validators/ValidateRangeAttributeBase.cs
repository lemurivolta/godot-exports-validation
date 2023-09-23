namespace LemuRivolta.ExportsValidation;

using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class ValidateRangeAttributeBase : NodeValidationBaseAttribute
{
    public bool Inclusive { get; set; } = true;
    private readonly int multiplier;
    private readonly double min;
    private readonly string comparative;

    public ValidateRangeAttributeBase(int multiplier, double min, string comparative)
    {
        this.multiplier = multiplier;
        this.min = multiplier * min;
        this.comparative = comparative;
    }

    public override ValidationError? Validate(ValidationInfo validationInfo)
    {
        var memberType = validationInfo.MemberType;
        if (!memberType.IsAssignableTo(typeof(float)) &&
            !memberType.IsAssignableTo(typeof(double)) &&
            !memberType.IsAssignableTo(typeof(int)) &&
            !memberType.IsAssignableTo(typeof(decimal))
            )
        {
            return new(
                $"Can check range only of float, double int and decimals, not of {memberType.Name}");
        }

        var value = multiplier * ToDouble(validationInfo.Value);
        if (Inclusive && value < min)
        {
            return new($"must be {comparative} or equal than {min}");
        }
        else if (!Inclusive && value <= min)
        {
            return new($"must be {comparative} than {min}");
        }
        return null;
    }

    private static double ToDouble(object value) => value switch
    {
        float f => f,
        int i => i,
        decimal d => (double)d,
        double dd => dd,
        _ => throw new ArgumentException("Unknown type", nameof(value)),
    };
}