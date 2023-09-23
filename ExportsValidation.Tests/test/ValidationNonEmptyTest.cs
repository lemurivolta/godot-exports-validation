using Chickensoft.GoDotTest;

using Godot;

using LemuRivolta.ExportsValidation;

using Shouldly;

using static LemuRivolta.ExportsValidation.ValidateNonEmptyAttribute;

namespace ExportsValidation.Tests;

partial class TestNodeNonEmpty : Node
{
    [ValidateNonEmpty]
    public string? Value = null;

    [ValidateNonEmpty(NoWhiteSpace = false)]
    public string? ValueWithWhiteSpace = null;
}

partial class TestWrongTypeNonEmpty : Node
{
    [ValidateNonEmpty]
    public int Value = 3;
}


internal class ValidationNonEmptyTest : TestClass
{
    private readonly Node testScene;

    public ValidationNonEmptyTest(Node testScene) : base(testScene)
    {
        this.testScene = testScene;
    }

    [Test]
    public void CheckWrongType()
    {
        var testNode = new TestWrongTypeNonEmpty();
        testScene.AddChild(testNode);

        testNode.ShouldThrowValidationErrors<MustBeStringValidationError>(
            "wrong type should fail not-empty test");
    }

    [Test]
    public void CheckNonEmptyNoWhitespace()
    {
        var testNode = new TestNodeNonEmpty();
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

    public void CheckNonEmptyWhitespace()
    {
        var testNode = new TestNodeNonEmpty();
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
