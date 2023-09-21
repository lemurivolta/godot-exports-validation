namespace LemuRivolta.ExportValidation;

using System;
using System.Collections.Generic;

public abstract class NodeValidationBaseAttribute : Attribute
{
    /// <summary>
    /// Validate the given node member.
    /// </summary>
    /// <param name="validationInfo">Information about the node member.</param>
    /// <returns>The problem found, or <c>null</c> if there was no problem.</returns>
    public abstract ValidationError? Validate(ValidationInfo validationInfo);
}
