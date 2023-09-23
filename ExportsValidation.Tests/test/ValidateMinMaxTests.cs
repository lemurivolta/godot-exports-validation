namespace ExportsValidation.Tests;

using System;

using Chickensoft.GoDotTest;

using Godot;

using LemuRivolta.ExportsValidation;

using Shouldly;

partial class TestMinMaxNode : Node
{
    [ValidateMin(0)]
    public int ValueRangeMinInclusive0 = 0;

    [ValidateMin(0, Inclusive = false)]
    public int ValueRangeMinExclusive0 { get; set; } = 10;

    [ValidateMax(0)]
    public int ValueRangeMaxInclusive0 = 0;

    [ValidateMax(0, Inclusive = false)]
    public int ValueRangeMaxExclusive0 { get; set; } = -10;
}

partial class TestOtherTypesMinMaxNode : Node
{
    [ValidateMin(0), ValidateMax(5)]
    public int ValidateIntValue = 3;

    [ValidateMin(0), ValidateMax(5)]
    public double ValidateDoubleValue = 3;

    [ValidateMin(0), ValidateMax(5)]
    public float ValidateFloatValue = 3;

    [ValidateMin(0), ValidateMax(5)]
    public decimal ValidateDecimalValue = 3;
}

partial class TestWrongTypeMinMaxNode : Node
{
    [ValidateMin(0)]
    public string ValidateStringValue = "";

    [ValidateMax(5)]
    public int[] ValidateArrayValue = Array.Empty<int>();
}

public class ValidateMinMaxTests: TestClass
{
    private readonly Node testScene;

    public ValidateMinMaxTests(Node testScene) : base(testScene)
    {
        this.testScene = testScene;
    }

    [Test]
    public void RangeOtherTypes()
    {
        var testNode = new TestOtherTypesMinMaxNode();
        testScene.AddChild(testNode);

        Should.NotThrow(testNode.Validate,
            "int, float, decimal and double types should be supported");

        var testNode2 = new TestWrongTypeMinMaxNode();
        testScene.AddChild(testNode2);

        Should.Throw<FullValidationException>(testNode2.Validate,
            "types other than int, float, decimal and double should not be supported");
    }

    private static Action<FullValidationException, string> CheckNodeFailure(string nodeName) =>
        (FullValidationException ex, string memberName) =>
            ex.ValidationFailureInfo.ShouldContain(info =>
                info.NodePath.EndsWith("/" + nodeName) &&
                info.MemberName.EndsWith(memberName));

    [Test]
    public void MinRangeInclusive()
    {
        var testNode = new TestMinMaxNode
        {
            Name = "MinRangeInclusiveTestNode"
        };

        var CheckFailure = CheckNodeFailure(testNode.Name);
        testScene.AddChild(testNode);

        testNode.ValueRangeMinInclusive0 = -10;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 inclusive fails on -10"),
            nameof(TestMinMaxNode.ValueRangeMinInclusive0));

        testNode.ValueRangeMinInclusive0 = 10;
        Should.NotThrow(testNode.Validate, "0 inclusive succeeds on 10");

        testNode.ValueRangeMinInclusive0 = 0;
        Should.NotThrow(testNode.Validate, "0 inclusive succeeds on 0");
    }

    [Test]
    public void MinRangeExclusive()
    {
        var testNode = new TestMinMaxNode
        {
            Name = "MinRangeExclusiveTestNode"
        };

        var CheckFailure = CheckNodeFailure(testNode.Name);
        testScene.AddChild(testNode);

        testNode.ValueRangeMinExclusive0 = -10;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 exclusive fails on -10"),
            nameof(TestMinMaxNode.ValueRangeMinExclusive0));

        testNode.ValueRangeMinExclusive0 = 10;
        Should.NotThrow(testNode.Validate, "0 exclusive succeeds on 10");

        testNode.ValueRangeMinExclusive0 = 0;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 exclusive fails on 0"),
            nameof(TestMinMaxNode.ValueRangeMinExclusive0));
    }

    [Test]
    public void MaxRangeInclusive()
    {
        var testNode = new TestMinMaxNode
        {
            Name = "MaxRangeInclusiveTestNode"
        };

        var CheckFailure = CheckNodeFailure(testNode.Name);
        testScene.AddChild(testNode);

        testNode.ValueRangeMaxInclusive0 = 10;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 inclusive fails on 10"),
            nameof(TestMinMaxNode.ValueRangeMaxInclusive0));

        testNode.ValueRangeMaxInclusive0 = -10;
        Should.NotThrow(testNode.Validate, "0 inclusive succeeds on -10");

        testNode.ValueRangeMaxInclusive0 = 0;
        Should.NotThrow(testNode.Validate, "0 inclusive succeeds on 0");
    }

    [Test]
    public void MaxRangeExclusive()
    {
        var testNode = new TestMinMaxNode
        {
            Name = "MaxRangeExclusiveTestNode"
        };

        var CheckFailure = CheckNodeFailure(testNode.Name);
        testScene.AddChild(testNode);

        testNode.ValueRangeMaxExclusive0 = 10;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 exclusive fails on 10"),
            nameof(TestMinMaxNode.ValueRangeMaxExclusive0));

        testNode.ValueRangeMaxExclusive0 = -10;
        Should.NotThrow(testNode.Validate, "0 exclusive succeeds on -10");

        testNode.ValueRangeMaxExclusive0 = 0;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 exclusive fails on 0"),
            nameof(TestMinMaxNode.ValueRangeMaxExclusive0));
    }
}
