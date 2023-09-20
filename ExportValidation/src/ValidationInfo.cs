namespace LemuRivolta.ExportValidation;

using Godot;

using System;
using System.Reflection;

public partial class ValidationInfo(MemberInfo memberInfo, Node node)
{
#pragma warning disable CA1822 // Mark members as static
    public MemberInfo MemberInfo => memberInfo;
    public Node Node => node;
#pragma warning restore CA1822 // Mark members as static

    public object Value => Switch(
        fieldInfo => fieldInfo.GetValue(Node)!,
        propertyInfo => propertyInfo.GetValue(Node)!);

    public string MemberName => MemberInfo.Name;

    public string NodePath => Node.GetPath().ToString();

    public Type MemberType => Switch(
        fieldInfo => fieldInfo.FieldType,
        propertyInfo => propertyInfo.PropertyType);

    private T Switch<T>(
        Func<FieldInfo, T> fieldInfoAction,
        Func<PropertyInfo, T> propertyInfoAction,
        Func<MemberInfo, T>? memberInfoAction = null)
    {
        static T DefaultMemberInfoAction(MemberInfo memberInfo)
        {
            throw new ValidationFailedException($"Unknown member info type ${memberInfo.GetType().Name}");
        };
        if (MemberInfo is FieldInfo fieldInfo)
        {
            return fieldInfoAction(fieldInfo);
        }
        else if (MemberInfo is PropertyInfo propertyInfo)
        {
            return propertyInfoAction(propertyInfo);
        }
        else
        {
            return (memberInfoAction ?? DefaultMemberInfoAction)(MemberInfo);
        }
    }
}
