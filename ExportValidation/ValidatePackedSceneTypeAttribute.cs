namespace LemuRivolta.ExportValidation;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Godot;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class ValidatePackedSceneTypeAttribute : NodeValidationBaseAttribute
{
    private readonly Type type;

    public ValidatePackedSceneTypeAttribute(Type type)
    {
        this.type = type ??
            throw new ArgumentNullException(nameof(type), "cannot be null");
    }

    public override ValidationError? Validate(ValidationInfo validationInfo) =>
        validationInfo.Value is not PackedScene packedScene ?
            new($"can only validate PackedScenes, not {validationInfo.MemberType.Name}") :
        !TryGetPackedSceneType(packedScene, out var packedSceneType) ?
            new("no script attached to the packed scene") :
        !type.IsAssignableFrom(packedSceneType) ?
            new($"Cannot assign a '{packedSceneType.FullName}' to a '{type.FullName}'") :
            null;

    private static bool TryGetPackedSceneType(
        PackedScene packedScene,
        [NotNullWhen(true)] out Type? type)
    {
        var sceneState = packedScene.GetState();
        for (var i = 0; i < sceneState.GetNodeCount(); i++)
        {
            var nodePath = sceneState.GetNodePath(i);
            // consider only root node
            if (nodePath == ".")
            {
                for (var j = 0; j < sceneState.GetNodePropertyCount(i); j++)
                {
                    var propertyName = sceneState.GetNodePropertyName(i, j);
                    // consider only the "script" property
                    if (propertyName != "script")
                    {
                        continue;
                    }
                    // extract the script and compare the types
                    Variant propertyValue = sceneState.GetNodePropertyValue(i, j);
                    var godotObject = propertyValue.AsGodotObject();
                    if (godotObject is CSharpScript cSharpScript)
                    {
                        var instance = cSharpScript.New();
                        var go = instance.AsGodotObject();
                        type = go.GetType();
                        return true;
                    }
                }
                break;
            }
        }
        type = null;
        return false;
    }
}
