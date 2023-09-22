using Chickensoft.GoDotTest;

using Godot;

using LemuRivolta.ExportValidation;

using Shouldly;

namespace ExportValidation.Tests;

partial class TestNodeNotEmpty : Node
{
    [ValidateNotEmpty]
    public string? Value = null;

    [ValidateNotEmpty(NoWhiteSpace = false)]
    public string? ValueWithWhiteSpace = null;
}

partial class TestWrongTypeNotEmpty : Node
{
    [ValidateNotEmpty]
    public int Value = 3;
}


internal class ValidationNotEmptyTest : TestClass
{
    private readonly Node testScene;

    public ValidationNotEmptyTest(Node testScene) : base(testScene)
    {
        this.testScene = testScene;
    }

    [Test]
    public void CheckWrongType()
    {
        var testNode = new TestWrongTypeNotEmpty();
        testScene.AddChild(testNode);

        Should.Throw<FullValidationException>(testNode.Validate,
            "wrong type should fail not-empty test");
    }

    [Test]
    public void CheckNotEmptyNoWhitespace()
    {
        var testNode = new TestNodeNotEmpty();
        testScene.AddChild(testNode);

        testNode.ValueWithWhiteSpace = "hello";

        testNode.Value = null;
        Should.Throw<FullValidationException>(testNode.Validate,
            "null value should fail not-empty test");

        testNode.Value = "";
        Should.Throw<FullValidationException>(testNode.Validate,
            "empty string value should fail not-empty test");

        testNode.Value = "  \t\n";
        Should.Throw<FullValidationException>(testNode.Validate,
            "only-whitespace string value should fail not-empty test");

        testNode.Value = "hello";
        Should.NotThrow(testNode.Validate,
            "\"hello\" value should succeed not-empty test");
    }

    public void CheckNotEmptyWhitespace()
    {
        var testNode = new TestNodeNotEmpty();
        testScene.AddChild(testNode);

        testNode.Value = "hello";
        testNode.ValueWithWhiteSpace = null;
        Should.Throw<FullValidationException>(testNode.Validate,
            "null value should fail not-empty test");

        testNode.ValueWithWhiteSpace = "";
        Should.Throw<FullValidationException>(testNode.Validate,
            "empty string value should fail not-empty test");

        testNode.ValueWithWhiteSpace = "  \t\n";
        Should.NotThrow(testNode.Validate,
            "only-whitespace string value should fail not-empty test");

        testNode.ValueWithWhiteSpace = "hello";
        Should.NotThrow(testNode.Validate,
            "\"hello\" value should succeed not-empty test");
    }
}
