namespace LemuRivolta.ExportValidation;

using System;

using Godot;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class ValidatePackedSceneTypeAttribute : NodeValidationBaseAttribute
{
    private readonly Type type;

    public ValidatePackedSceneTypeAttribute(Type type)
    {
        this.type = type;
    }

    public override void Validate(ValidationInfo validationInfo)
    {
        if (validationInfo.Value is not PackedScene packedScene)
        {
            throw new ValidationFailedException(
                $"can apply ValidatePackedSceneType attribute only to members of type PackedScene, not {validationInfo.MemberType.Name}");
        }

        var packedSceneType = CheckPackedScene(packedScene) ??
            throw new ValidationFailedException(
                "no script attached to the packed scene");
        if (!type.IsAssignableFrom(packedSceneType))
        {
            throw new ValidationFailedException(
                $"should be a {type.FullName}, instead is a {packedSceneType.FullName}");
        }
    }

    private static Type? CheckPackedScene(PackedScene packedScene)
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
                        return go.GetType();
                    }
                }
                break;
            }
        }
        return null;
    }
}
