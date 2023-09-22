namespace LemuRivolta.ExportValidation;

using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public partial class ValidateRangeAttribute : NodeValidationBaseAttribute
{
    private readonly double min;
    private readonly bool minInclusive;
    private readonly double max;
    private readonly bool maxInclusive;

    public ValidateRangeAttribute(double min = double.MinValue,
        bool minInclusive = true,
        double max = double.MaxValue,
        bool maxInclusive = true)
    {
        if (min >= max)
        {
            throw new ArgumentException("min must be less than max",
                nameof(min));
        }

        this.min = min;
        this.max = max;
        this.minInclusive = minInclusive;
        this.maxInclusive = maxInclusive;
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

        var value = ToDouble(validationInfo.Value);
        if (minInclusive && value < min)
        {
            return new($"must be greater or equal than {min}");
        }
        if (!minInclusive && value <= min)
        {
            return new($"must be greater than {min}");
        }
        if (maxInclusive && value > max)
        {
            return new($"must be less or equal than {max}");
        }
        if (!maxInclusive && value >= max)
        {
            return new($"must be less than {max}");
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
