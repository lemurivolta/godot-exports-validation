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
        this.min = min;
        this.comparative = comparative;
    }

    public override ValidationError? Validate(ValidationInfo validationInfo)
    {
        Type memberType = validationInfo.MemberType;
        if (!memberType.IsAssignableTo(typeof(float)) &&
            !memberType.IsAssignableTo(typeof(double)) &&
            !memberType.IsAssignableTo(typeof(int)) &&
            !memberType.IsAssignableTo(typeof(decimal))
            )
        {
            return new WrongTypeValidationError(memberType);
        }

        var value = ToDouble(validationInfo.Value);
        return Inclusive && multiplier * value < multiplier * min ||
            !Inclusive && multiplier * value <= multiplier * min
            ? new OutsideRangeValidationError(comparative, min)
            : (ValidationError?)null;
    }

    private static double ToDouble(object value) => value switch
    {
        float f => f,
        int i => i,
        decimal d => (double)d,
        double dd => dd,
        _ => throw new ArgumentException("Unknown type", nameof(value)),
    };

    internal class WrongTypeValidationError : ValidationError
    {
        public WrongTypeValidationError(Type type) :
            base($"Can check range only of float, double int and decimals, not of {type.Name}")
        { }
    }

    internal class OutsideRangeValidationError : ValidationError
    {
        public OutsideRangeValidationError(string comparative, double extreme) :
            base($"must be {comparative} or equal than {extreme}")
        { }
    }
}