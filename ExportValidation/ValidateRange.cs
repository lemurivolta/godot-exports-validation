namespace LemuRivolta.ExportValidation;

using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public partial class ValidateRange : NodeValidationBaseAttribute
{
    public double Min { get; set; } = double.MinValue;
    public bool MinInclusive { get; set; } = true;
    public double Max { get; set; } = double.MaxValue;
    public bool MaxInclusive { get; set; } = true;

    public override void Validate(ValidationInfo validationInfo)
    {
        if (Min >= Max)
        {
            throw new ValidationFailedException("Min must be less than Max");
        }

        var memberType = validationInfo.MemberType;
        if (!memberType.IsAssignableTo(typeof(float)) &&
            !memberType.IsAssignableTo(typeof(double)) &&
            !memberType.IsAssignableTo(typeof(int)) &&
            !memberType.IsAssignableTo(typeof(decimal))
            )
        {
            throw new ValidationFailedException(
                $"Can check range only of float, double int and decimals, not of {memberType.Name}");
        }

        var value = ToDouble(validationInfo.Value);
        if (MinInclusive && value < Min)
        {
            throw new ValidationFailedException($"must be greater or equal than {Min}");
        }
        if (!MinInclusive && value <= Min)
        {
            throw new ValidationFailedException($"must be greater than {Min}");
        }
        if (MaxInclusive && value > Max)
        {
            throw new ValidationFailedException($"must be less or equal than {Max}");
        }
        if (!MaxInclusive && value >= Max)
        {
            throw new ValidationFailedException($"must be less than {Max}");
        }
    }

    private static double ToDouble(object value)
    {
        if (value is float f)
        {
            return f;
        }
        else if (value is int i)
        {
            return i;
        }
        else if (value is decimal d)
        {
            return (double)d;
        }
        else if (value is double dd)
        {
            return dd;
        }
        else
        {
            throw new ArgumentException("Unknown type", nameof(value));
        }
    }
}
