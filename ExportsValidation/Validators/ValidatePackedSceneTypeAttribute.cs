namespace LemuRivolta.ExportsValidation;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Godot;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class ValidatePackedSceneTypeAttribute : NodeValidationBaseAttribute
{
    private readonly Type type;

    public bool AllowNullValues { get; set; }

    public ValidatePackedSceneTypeAttribute(Type type)
    {
        this.type = type ??
            throw new ArgumentNullException(nameof(type), "cannot be null");
        if (!typeof(Node).IsAssignableFrom(type))
        {
            throw new ArgumentException("packed scene type must derive from Node", nameof(type));
        }
    }

    public override ValidationError? Validate(ValidationInfo validationInfo) =>
        validationInfo.Value is null && AllowNullValues ?
            null :
        validationInfo.Value is null && !AllowNullValues ?
            new CannotBeNullValidationError() :
        validationInfo.Value is not PackedScene packedScene ?
            new CanOnlyValidatePackedScenesValidationError(validationInfo) :
        !TryGetPackedSceneType(packedScene, out var packedSceneType) ?
            new NoScriptAttachedValidationError() :
        !type.IsAssignableFrom(packedSceneType) ?
            new NotAssignableValidationError(packedSceneType, type) :
            null;

    private static bool TryGetPackedSceneType(
        PackedScene packedScene,
        [NotNullWhen(true)] out Type? type)
    {
        var sceneState = packedScene.GetState();
        var nodePath = sceneState.GetNodePath(0);
        // consider only root node - should always be the first
        if (nodePath != ".")
        {
            throw new InvalidOperationException("root node was not the first");
        }
        for (var i = 0; i < sceneState.GetNodePropertyCount(0); i++)
        {
            var propertyName = sceneState.GetNodePropertyName(0, i);
            // consider only the "script" property
            if (propertyName != "script")
            {
                continue;
            }
            // extract the script and compare the types
            Variant propertyValue = sceneState.GetNodePropertyValue(0, i);
            var godotObject = propertyValue.AsGodotObject();
            if (godotObject is CSharpScript cSharpScript)
            {
                var instance = cSharpScript.New();
                var go = instance.AsGodotObject();
                type = go.GetType();
                return true;
            }
        }
        type = null;
        return false;
    }

    internal class CannotBeNullValidationError : ValidationError
    {
        public CannotBeNullValidationError()
            : base("no packed scene has been assigned")
        {
        }
    }

    internal class CanOnlyValidatePackedScenesValidationError : ValidationError
    {
        public CanOnlyValidatePackedScenesValidationError(ValidationInfo validationInfo) :
            base($"can only validate PackedScenes, not {validationInfo.MemberType.Name}")
        {
        }
    }

    internal class NoScriptAttachedValidationError : ValidationError
    {
        public NoScriptAttachedValidationError()
            : base("no script attached to the packed scene") { }
    }

    internal class NotAssignableValidationError : ValidationError
    {
        public NotAssignableValidationError(Type packedSceneType, Type requiredType) :
            base($"Cannot assign a '{packedSceneType.FullName}' to a '{requiredType.FullName}'")
        {
        }
    }
}