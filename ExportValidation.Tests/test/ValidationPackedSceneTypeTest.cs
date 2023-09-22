namespace ExportValidation.Tests;

using Chickensoft.GoDotTest;

using ExportValidation.Tests.test;

using Godot;

using LemuRivolta.ExportValidation;

using Shouldly;

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
