namespace ExportsValidation.Tests;

using System.Linq;

using Chickensoft.GoDotTest;

using Godot;

using LemuRivolta.ExportsValidation;

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

        var exc = Should.Throw<FullValidationException>(testNode.Validate,
            "null value should fail non-null test");
        exc!.ValidationFailureInfo.Count().ShouldBe(1);
        exc!.Message.Split('\n').Length.ShouldBe(2);

        testNode.Node = testNode;
        Should.NotThrow(testNode.Validate,
            "non-null value should succeed non-null test");
    }
}
