namespace LemuRivolta.ExportValidation;

using System;

public abstract class NodeValidationBaseAttribute : Attribute
{
    public abstract void Validate(ValidationInfo validationInfo);
}
