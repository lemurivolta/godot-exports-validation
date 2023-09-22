namespace LemuRivolta.ExportValidation;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Godot;

public static class NodeValidation
{
    public static void Validate(this Node node)
    {
        List<ValidationFailureInfo> info = new();
        var allMembers = ((MemberInfo[])node.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            .Concat(node.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
        foreach (MemberInfo member in allMembers)
        {
            foreach (var attribute in member.GetCustomAttributes<NodeValidationBaseAttribute>())
            {
                var validationInfo = ValidationInfo.Create(member, node);
                var validationError = attribute.Validate(validationInfo);
                if (validationError != null)
                {
                    info.Add(new(
                        validationInfo.NodePath,
                        validationInfo.MemberName,
                        validationError.Message));
                }
            }
        }

        if (info.Count > 0)
        {
            throw new FullValidationException(info);
        }
    }
}
