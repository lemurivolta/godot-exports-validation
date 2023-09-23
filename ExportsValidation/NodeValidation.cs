namespace LemuRivolta.ExportsValidation;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Godot;

public static class NodeValidation
{
    /// <summary>
    /// Binding flags used to retrieve fields and properties we're looking for
    /// </summary>
    private const BindingFlags bindingFlags =
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    public static void Validate(this Node node)
    {
        var allMembers = ((MemberInfo[])node.GetType().GetFields(bindingFlags))
            .Concat(node.GetType().GetProperties(bindingFlags));

        var info = (
            from member in allMembers
            let validationInfo = ValidationInfo.Create(member, node)
            from attribute in member.GetCustomAttributes<NodeValidationBaseAttribute>()
            let validationError = attribute.Validate(validationInfo)
            where validationError != null
            select new ValidationFailureInfo(
                validationInfo.NodePath,
                validationInfo.MemberName,
                validationError.Message)
            ).ToList();

        if (info.Count > 0)
        {
            throw new FullValidationException(info);
        }
    }
}
