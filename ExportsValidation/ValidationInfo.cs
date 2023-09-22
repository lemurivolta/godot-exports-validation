namespace LemuRivolta.ExportsValidation;

using System;
using System.Diagnostics;
using System.Reflection;

using Godot;

public abstract class ValidationInfo
{
    private readonly string memberName;
    private readonly Node node;

    protected ValidationInfo(MemberInfo memberInfo, Node node)
    {
        this.node = node;
        memberName = memberInfo.Name;
    }

    public static ValidationInfo Create(MemberInfo memberInfo, Node node) =>
        memberInfo switch
        {
            FieldInfo fieldInfo => new FieldValidationInfo(node, fieldInfo),
            PropertyInfo propertyInfo => new PropertyValidationInfo(node, propertyInfo),
            _ => throw new ArgumentException("member info is neither field nor property", nameof(memberInfo))
        };

    public Node Node => node;

    public string NodePath => node.GetPath().ToString();

    public string MemberName => memberName;

    public abstract object Value { get; }

    public abstract Type MemberType { get; }
}
