using Chickensoft.GoDotTest;

using Godot;

using LemuRivolta.ExportsValidation;

using Shouldly;

using static LemuRivolta.ExportsValidation.ValidateNotEmptyAttribute;

namespace ExportsValidation.Tests;

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

        testNode.ShouldThrowValidationErrors<MustBeStringValidationError>(
            "wrong type should fail not-empty test");
    }

    [Test]
    public void CheckNotEmptyNoWhitespace()
    {
        var testNode = new TestNodeNotEmpty();
        testScene.AddChild(testNode);

        testNode.ValueWithWhiteSpace = "hello";

        testNode.Value = null;
        testNode.ShouldThrowValidationErrors<CannotBeNullValidationError>(
            "null value should fail not-empty test");

        testNode.Value = "";
        testNode.ShouldThrowValidationErrors<CannotBeEmptyValidationError>(
            "empty string value should fail not-empty test");

        testNode.Value = "  \t\n";
        testNode.ShouldThrowValidationErrors<CannotBeWhitespaceValidationError>(
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
        testNode.ShouldThrowValidationErrors<CannotBeNullValidationError>(
            "null value should fail not-empty test");

        testNode.ValueWithWhiteSpace = "";
        testNode.ShouldThrowValidationErrors<CannotBeEmptyValidationError>(
            "empty string value should fail not-empty test");

        testNode.ValueWithWhiteSpace = "  \t\n";
        Should.NotThrow(testNode.Validate,
            "only-whitespace string value should fail not-empty test");

        testNode.ValueWithWhiteSpace = "hello";
        Should.NotThrow(testNode.Validate,
            "\"hello\" value should succeed not-empty test");
    }
}
