namespace ExportValidation.Tests;

using System;

using Chickensoft.GoDotTest;

using Godot;

using LemuRivolta.ExportValidation;

using Shouldly;

partial class TestNode : Node
{
    [ValidateRange(Min = 0)]
    public int ValueRangeMinInclusive0 = 0;

    [ValidateRange(Min = 0, MinInclusive = false)]
    public int ValueRangeMinExclusive0 = 10;

    [ValidateRange(Max = 0)]
    public int ValueRangeMaxInclusive0 = 0;

    [ValidateRange(Max = 0, MaxInclusive = false)]
    public int ValueRangeMaxExclusive0 = -10;
}


public class ValidationRangeTests : TestClass
{
    private readonly Node testScene;

    public ValidationRangeTests(Node testScene) : base(testScene)
    {
        this.testScene = testScene;
    }

    private static Action<FullValidationException, string> CheckNodeFailure(string nodeName) =>
        (FullValidationException ex, string memberName) =>
            ex.ValidationFailureInfo.ShouldContain(info =>
                info.NodePath.EndsWith("/" + nodeName) &&
                info.MemberName.EndsWith(memberName));

    [Test]
    public void MinRangeInclusive()
    {
        var testNode = new TestNode
        {
            Name = "MinRangeInclusiveTestNode"
        };

        var CheckFailure = CheckNodeFailure(testNode.Name);
        testScene.AddChild(testNode);

        testNode.ValueRangeMinInclusive0 = -10;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 inclusive fails on -10"),
            nameof(TestNode.ValueRangeMinInclusive0));

        testNode.ValueRangeMinInclusive0 = 10;
        Should.NotThrow(testNode.Validate, "0 inclusive succeeds on 10");

        testNode.ValueRangeMinInclusive0 = 0;
        Should.NotThrow(testNode.Validate, "0 inclusive succeeds on 0");
    }

    [Test]
    public void MinRangeExclusive()
    {
        var testNode = new TestNode
        {
            Name = "MinRangeExclusiveTestNode"
        };

        var CheckFailure = CheckNodeFailure(testNode.Name);
        testScene.AddChild(testNode);

        testNode.ValueRangeMinExclusive0 = -10;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 exclusive fails on -10"),
            nameof(TestNode.ValueRangeMinExclusive0));

        testNode.ValueRangeMinExclusive0 = 10;
        Should.NotThrow(testNode.Validate, "0 exclusive succeeds on 10");

        testNode.ValueRangeMinExclusive0 = 0;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 exclusive fails on 0"),
            nameof(TestNode.ValueRangeMinExclusive0));
    }

    [Test]
    public void MaxRangeInclusive()
    {
        var testNode = new TestNode
        {
            Name = "MaxRangeInclusiveTestNode"
        };

        var CheckFailure = CheckNodeFailure(testNode.Name);
        testScene.AddChild(testNode);

        testNode.ValueRangeMaxInclusive0 = 10;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 inclusive fails on 10"),
            nameof(TestNode.ValueRangeMaxInclusive0));

        testNode.ValueRangeMaxInclusive0 = -10;
        Should.NotThrow(testNode.Validate, "0 inclusive succeeds on -10");

        testNode.ValueRangeMaxInclusive0 = 0;
        Should.NotThrow(testNode.Validate, "0 inclusive succeeds on 0");
    }

    [Test]
    public void MaxRangeExclusive()
    {
        var testNode = new TestNode
        {
            Name = "MaxRangeExclusiveTestNode"
        };

        var CheckFailure = CheckNodeFailure(testNode.Name);
        testScene.AddChild(testNode);

        testNode.ValueRangeMaxExclusive0 = 10;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 exclusive fails on 10"),
            nameof(TestNode.ValueRangeMaxExclusive0));

        testNode.ValueRangeMaxExclusive0 = -10;
        Should.NotThrow(testNode.Validate, "0 exclusive succeeds on -10");

        testNode.ValueRangeMaxExclusive0 = 0;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 exclusive fails on 0"),
            nameof(TestNode.ValueRangeMaxExclusive0));
    }
}
