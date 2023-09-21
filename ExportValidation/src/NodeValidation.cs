namespace LemuRivolta.ExportValidation;

using System.Collections.Generic;
using System.Reflection;

using Godot;

public static class NodeValidation
{
    public static void Validate(this Node node)
    {
        List<string> exceptions = new();
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
                    exceptions.Add($"{validationInfo.NodePath} - {validationInfo.MemberName}: {e.Message}");
                }
            }
        }

        if (exceptions.Count > 0)
        {
            throw new ValidationFailedException(
                "\n" + string.Join('\n', exceptions));
        }
    }
}
