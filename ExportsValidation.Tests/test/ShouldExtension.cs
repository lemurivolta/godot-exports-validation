using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Godot;

using LemuRivolta.ExportsValidation;

namespace Shouldly;

[DebuggerStepThrough]
[ShouldlyMethods]
public static class ShouldExtension
{
    public static void ShouldThrowValidationErrors<T>(
        this Node node, string? customMessage)
        where T: ValidationError =>
        node.ShouldThrowValidationErrors(new Type[] { typeof(T) }, customMessage);

    public static void ShouldThrowValidationErrors<T1, T2>(
        this Node node, string? customMessage)
        where T1 : ValidationError
        where T2 : ValidationError =>
        node.ShouldThrowValidationErrors(
            new Type[] { typeof(T1), typeof(T2) },
            customMessage);

    public static void ShouldThrowValidationErrors<T1, T2, T3>(
        this Node node, string? customMessage)
        where T1 : ValidationError
        where T2 : ValidationError
        where T3 : ValidationError =>
        node.ShouldThrowValidationErrors(
            new Type[] { typeof(T1), typeof(T2), typeof(T3) },
            customMessage);

    public static void ShouldThrowValidationErrors<T1, T2, T3, T4>(
        this Node node, string? customMessage)
        where T1 : ValidationError
        where T2 : ValidationError
        where T3 : ValidationError
        where T4 : ValidationError =>
        node.ShouldThrowValidationErrors(
            new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) },
            customMessage);

    public static void ShouldThrowValidationErrors(this Node node,
        params Type[] types) =>
        node.ShouldThrowValidationErrors(types);

    private struct TypeComparer : IComparer<Type>
    {
        public readonly int Compare(Type? x, Type? y) =>
            x is null ?
                y is null ? 0 : -1 :
                y is null ? 1 : x.GetHashCode().CompareTo(y.GetHashCode());
    }

    private static readonly TypeComparer typeComparerer;

    public static void ShouldThrowValidationErrors(this Node node,
        Type[] types, string? customMessage)
    {
        if (types.Length > 0)
        {
            var fullValidationException = Should.Throw<FullValidationException>(
                node.Validate, customMessage);
            var returnedTypes = (
                from info in fullValidationException.ValidationFailureInfo
                select info.ValidationError.GetType())
                .OrderBy(type => type, typeComparerer);
            Array.Sort(types, typeComparerer);
            returnedTypes.ShouldBe(types);
        }
        else
        {
            Should.NotThrow(node.Validate, customMessage);
        }
    }
}
