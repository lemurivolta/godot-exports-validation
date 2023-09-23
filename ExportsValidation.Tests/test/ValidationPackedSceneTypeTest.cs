namespace ExportsValidation.Tests;

using System;

using Chickensoft.GoDotTest;

using ExportsValidation.Tests.test;

using Godot;

using LemuRivolta.ExportsValidation;

using Shouldly;

using static LemuRivolta.ExportsValidation.ValidatePackedSceneTypeAttribute;

partial class TestPackedSceneTypeNonPackedScene: Node {
    [ValidatePackedSceneType(typeof(BaseScript))]
    public string Value = "";
}

partial class TestPackedSceneTypeBase : Node
{
    [ValidatePackedSceneType(typeof(BaseScript))]
    public PackedScene? Scene = null;
}

partial class TestPackedSceneTypeBaseNullable : Node
{
    [ValidatePackedSceneType(typeof(BaseScript), AllowNullValues = true)]
    public PackedScene? Scene = null;
}

partial class TestPackedSceneTypeDerived : Node
{
    [ValidatePackedSceneType(typeof(DerivedScript))]
    public PackedScene? Scene = null;
}

public class ValidationPackedSceneTypeTest : TestClass
{
    private readonly Node testScene;

    public ValidationPackedSceneTypeTest(Node testScene) : base(testScene)
    {
        this.testScene = testScene;
    }

    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public void CheckNoParameter()
    {
        Should.Throw<ArgumentNullException>(
            () => new ValidatePackedSceneTypeAttribute(null!),
            "should not allow a null parameter");
        Should.Throw<ArgumentException>(
            () => new ValidatePackedSceneTypeAttribute(typeof(int)),
            "should not allow a type that's not a node");
    }

    [Test]
    public void CheckInvalid()
    {
        var testNode = new TestPackedSceneTypeNonPackedScene();
        testScene.AddChild(testNode);
        testNode.ShouldThrowValidationErrors
            <CanOnlyValidatePackedScenesValidationError>(
            "ValidatePackedSceneType should not accept a string value");
    }

    [Test]
    public void CheckNull()
    {
        var testNode = new TestPackedSceneTypeBase();
        testScene.AddChild(testNode);
        testNode.ShouldThrowValidationErrors
            <CannotBeNullValidationError>(
            "by default packed scenes cannot be null");

        var testNode2 = new TestPackedSceneTypeBaseNullable();
        testScene.AddChild(testNode2);
        Should.NotThrow(testNode2.Validate, "can be allowed null packed scenes");
    }

    [Test]
    public void CheckGDScriptAndOtherProperties()
    {
        var testNode = new TestPackedSceneTypeBase();
        testScene.AddChild(testNode);
        var gdScene = GD.Load<PackedScene>("res://test/gd_node.tscn");
        testNode.Scene = gdScene;
        testNode.ShouldThrowValidationErrors
            <NoScriptAttachedValidationError>(
            "ValidatePackedSceneType should not find any GD script");
    }

    [Test]
    public void CheckEmptyScene()
    {
        var testNode = new TestPackedSceneTypeBase();
        testScene.AddChild(testNode);
        var emptyScene = GD.Load<PackedScene>("res://test/empty_scene.tscn");
        testNode.Scene = emptyScene;
        testNode.ShouldThrowValidationErrors
            <NoScriptAttachedValidationError>(
            "ValidatePackedSceneType should not accept a scene without type");
    }

    [Test]
    public void CheckBaseScript()
    {
        var testNode = new TestPackedSceneTypeBase();
        testScene.AddChild(testNode);

        var baseScene = GD.Load<PackedScene>("res://test/base_node.tscn");
        var derivedScene = GD.Load<PackedScene>("res://test/derived_node.tscn");
        var unrelatedScene = GD.Load<PackedScene>("res://test/unrelated_node.tscn");

        testNode.Scene = baseScene;
        Should.NotThrow(testNode.Validate,
            "assigning a script of the requested type should work");

        testNode.Scene = derivedScene;
        Should.NotThrow(testNode.Validate,
            "assigning a script derived from the requested type should work");

        testNode.Scene = unrelatedScene;
        testNode.ShouldThrowValidationErrors
            <NotAssignableValidationError>(
            "assigning a script unrelated to the requested type should throw");
    }

    [Test]
    public void CheckDerivedScript()
    {
        var testNode = new TestPackedSceneTypeDerived();
        testScene.AddChild(testNode);

        var baseScene = GD.Load<PackedScene>("res://test/base_node.tscn");
        var derivedScene = GD.Load<PackedScene>("res://test/derived_node.tscn");
        var unrelatedScene = GD.Load<PackedScene>("res://test/unrelated_node.tscn");

        testNode.Scene = baseScene;
        testNode.ShouldThrowValidationErrors
            <NotAssignableValidationError>(
            "assigning a script higher in the derivation hierarchy should not work");

        testNode.Scene = derivedScene;
        Should.NotThrow(testNode.Validate,
            "assigning a script derived from the requested type should work");

        testNode.Scene = unrelatedScene;
        testNode.ShouldThrowValidationErrors
            <NotAssignableValidationError>(
            "assigning a script unrelated to the requested type should throw");
    }
}
