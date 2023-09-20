namespace LemuRivolta.ExportValidation;

using System;
using System.Reflection;

public abstract class NodeValidationBaseAttribute : Attribute
{
    public abstract void Validate(ValidationInfo validationInfo);
}
