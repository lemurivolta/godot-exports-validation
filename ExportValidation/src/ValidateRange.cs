namespace LemuRivolta.ExportValidation;

using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public partial class ValidateRange : NodeValidationBaseAttribute
{
    public double Min = double.MinValue;
    public bool MinInclusive = true;
    public double Max = double.MaxValue;
    public bool MaxInclusive = true;

    public override void Validate(ValidationInfo validationInfo)
    {
        if (Min >= Max)
        {
            throw new Exception("Min must be less than Max");
        }

        var memberType = validationInfo.MemberType;
        if (!memberType.IsAssignableTo(typeof(float)) &&
            !memberType.IsAssignableTo(typeof(double)) &&
            !memberType.IsAssignableTo(typeof(int)) &&
            !memberType.IsAssignableTo(typeof(decimal))
            )
        {
            throw new Exception($"Can check range only of float, double int and decimals, not of {memberType.Name}");
        }

        var value = ToDouble(validationInfo.Value);
        if (MinInclusive && value < Min)
        {
            throw new Exception($"must be greater or equal than {Min}");
        }
        if (!MinInclusive && value <= Min)
        {
            throw new Exception($"must be greater than {Min}");
        }
        if (MaxInclusive && value > Max)
        {
            throw new Exception($"must be less or equal than {Max}");
        }
        if (!MaxInclusive && value >= Max)
        {
            throw new Exception($"must be less than {Max}");
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
            throw new Exception();
        }
    }
}
