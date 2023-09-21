namespace LemuRivolta.ExportValidation;

using System.Collections.Generic;
using System.Reflection;

using Godot;

public static class NodeValidation
{
    public static void Validate(this Node node)
    {
        List<ValidationFailureInfo> info = new();
        foreach (FieldInfo field in node.GetType().GetFields(BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance))
        {
            foreach (var attribute in field.GetCustomAttributes<NodeValidationBaseAttribute>())
            {
                ValidationInfo validationInfo = new(field, node);
                try
                {
                    attribute.Validate(validationInfo);
                }
                catch (ValidationFailedException e)
                {
                    info.Add(new(validationInfo.NodePath, validationInfo.MemberName, e.Message));
                }
            }
        }

        if (info.Count > 0)
        {
            throw new FullValidationException(info);
        }
    }
}
