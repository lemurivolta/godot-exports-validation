namespace ExportValidation.Tests;

using Chickensoft.GoDotTest;

using Godot;

using LemuRivolta.ExportValidation;

using Shouldly;

partial class TestNodeNonNull : Node
{
    [ValidateNonNull]
    public Node? Node = null;
}

public class ValidationNonNullTest : TestClass
{
    private readonly Node testScene;

    public ValidationNonNullTest(Node testScene) : base(testScene)
    {
        this.testScene = testScene;
    }

    [Test]
    public void CheckNonNull()
    {
        var testNode = new TestNodeNonNull();
        testScene.AddChild(testNode);

        Should.Throw<FullValidationException>(() => testNode.Validate(),
            "null value should fail non-null test");

        testNode.Node = testNode;
        Should.NotThrow(testNode.Validate,
            "non-null value should succeed non-null test");
    }
}
