namespace ExportsValidation.Tests;

using System;

using Chickensoft.GoDotTest;

using ExportsValidation.Tests.test;

using Godot;

using LemuRivolta.ExportsValidation;

using Shouldly;

partial class TestPackedSceneTypeNonPackedScene: Node {
    [ValidatePackedSceneType(typeof(BaseScript))]
    public string Value = "";
}

partial class TestPackedSceneTypeBase : Node
{
    [ValidatePackedSceneType(typeof(BaseScript))]
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
    }

    [Test]
    public void CheckInvalid()
    {
        var testNode = new TestPackedSceneTypeNonPackedScene();
        testScene.AddChild(testNode);
        Should.Throw<FullValidationException>(testNode.Validate,
            "ValidatePackedSceneType should not accept a string value");
    }

    [Test]
    public void CheckGDScriptAndOtherProperties()
    {
        var testNode = new TestPackedSceneTypeBase();
        testScene.AddChild(testNode);
        var gdScene = GD.Load<PackedScene>("res://test/gd_node.tscn");
        testNode.Scene = gdScene;
        Should.Throw<FullValidationException>(testNode.Validate,
            "ValidatePackedSceneType should not find any GD script");
    }

    [Test]
    public void CheckEmptyScene()
    {
        var testNode = new TestPackedSceneTypeBase();
        testScene.AddChild(testNode);
        var emptyScene = GD.Load<PackedScene>("res://test/empty_scene.tscn");
        testNode.Scene = emptyScene;
        Should.Throw<FullValidationException>(testNode.Validate,
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
        Should.Throw<FullValidationException>(testNode.Validate,
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
        Should.Throw<FullValidationException>(testNode.Validate,
            "assigning a script higher in the derivation hierarchy should not work");

        testNode.Scene = derivedScene;
        Should.NotThrow(testNode.Validate,
            "assigning a script derived from the requested type should work");

        testNode.Scene = unrelatedScene;
        Should.Throw<FullValidationException>(testNode.Validate,
            "assigning a script unrelated to the requested type should throw");
    }
}
