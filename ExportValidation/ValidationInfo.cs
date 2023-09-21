namespace LemuRivolta.ExportValidation;

using System;
using System.Diagnostics;
using System.Reflection;

using Godot;

public partial class ValidationInfo
{
    private readonly MemberInfo memberInfo;
    private readonly Node node;

    public MemberInfo MemberInfo => memberInfo;
    public Node Node => node;

    public ValidationInfo(MemberInfo memberInfo, Node node)
    {
        this.memberInfo = memberInfo;
        this.node = node;
    }

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
            throw new InvalidOperationException();
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
