namespace LemuRivolta.ExportValidation;

using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public partial class ValidateRange : NodeValidationBaseAttribute
{
    public double Min { get; set; } = double.MinValue;
    public bool MinInclusive { get; set; } = true;
    public double Max { get; set; } = double.MaxValue;
    public bool MaxInclusive { get; set; } = true;

    public override ValidationError? Validate(ValidationInfo validationInfo)
    {
        if (Min >= Max)
        {
            return new("Min must be less than Max");
        }

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
        if (MinInclusive && value < Min)
        {
            return new($"must be greater or equal than {Min}");
        }
        if (!MinInclusive && value <= Min)
        {
            return new($"must be greater than {Min}");
        }
        if (MaxInclusive && value > Max)
        {
            return new($"must be less or equal than {Max}");
        }
        if (!MaxInclusive && value >= Max)
        {
            return new($"must be less than {Max}");
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
