using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Godot;

using LemuRivolta.ExportsValidation;

using Shouldly;

namespace ExportsValidation.Tests;

[DebuggerStepThrough]
[ShouldlyMethods]
public static class ShouldExtension
{
    public static FullValidationException ShouldThrowValidationErrors<T>(
        this Node node, string? customMessage = null)
        where T : ValidationError =>
        node.ShouldThrowValidationErrors(new Type[] { typeof(T) }, customMessage);

    public static FullValidationException ShouldThrowValidationErrors<T1, T2>(
        this Node node, string? customMessage = null)
        where T1 : ValidationError
        where T2 : ValidationError =>
        node.ShouldThrowValidationErrors(
            new Type[] { typeof(T1), typeof(T2) },
            customMessage);

    public static FullValidationException ShouldThrowValidationErrors<T1, T2, T3>(
        this Node node, string? customMessage = null)
        where T1 : ValidationError
        where T2 : ValidationError
        where T3 : ValidationError =>
        node.ShouldThrowValidationErrors(
            new Type[] { typeof(T1), typeof(T2), typeof(T3) },
            customMessage);

    public static FullValidationException ShouldThrowValidationErrors<T1, T2, T3, T4>(
        this Node node, string? customMessage = null)
        where T1 : ValidationError
        where T2 : ValidationError
        where T3 : ValidationError
        where T4 : ValidationError =>
        node.ShouldThrowValidationErrors(
            new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) },
            customMessage);

    public static FullValidationException ShouldThrowValidationErrors(this Node node,
        params Type[] types) =>
        node.ShouldThrowValidationErrors(types);

    private static readonly IComparer<Type> typeComparer = Comparer<Type>.Create(
        (Type x, Type y) =>
            x is null ?
                y is null ? 0 : -1 :
                y is null ? 1 : x.GetHashCode().CompareTo(y.GetHashCode()));

    public static FullValidationException ShouldThrowValidationErrors(this Node node,
        Type[] types, string? customMessage = null)
    {
        types.ShouldNotBeEmpty();
        var fullValidationException = Should.Throw<FullValidationException>(
            node.Validate, customMessage);
        fullValidationException.ShouldNotBeNull();
        var returnedTypes = (
            from info in fullValidationException.ValidationFailureInfo
            select info.ValidationError.GetType())
            .OrderBy(type => type, typeComparer);
        Array.Sort(types, typeComparer);
        returnedTypes.ShouldBe(types);
        return fullValidationException!;
    }
}
