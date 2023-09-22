namespace ExportsValidation.Tests;

using System;

using Chickensoft.GoDotTest;

using Godot;

using LemuRivolta.ExportsValidation;

using Shouldly;

partial class TestRangeNode : Node
{
    [ValidateRange(min: 0)]
    public int ValueRangeMinInclusive0 = 0;

    [ValidateRange(min: 0, minInclusive: false)]
    public int ValueRangeMinExclusive0 { get; set; } = 10;

    [ValidateRange(max: 0)]
    public int ValueRangeMaxInclusive0 = 0;

    [ValidateRange(max: 0, maxInclusive: false)]
    public int ValueRangeMaxExclusive0 { get; set; } = -10;
}

partial class TestOtherTypesRangeNode : Node
{
    [ValidateRange(min: 0, max: 5)]
    public int ValidateIntValue = 3;

    [ValidateRange(min: 0, max: 5)]
    public double ValidateDoubleValue = 3;

    [ValidateRange(min: 0, max: 5)]
    public float ValidateFloatValue = 3;

    [ValidateRange(min: 0, max: 5)]
    public decimal ValidateDecimalValue = 3;
}

partial class TestWrongTypeRangeNode : Node
{
    [ValidateRange(min: 0, max: 5)]
    public string ValidateStringValue = "";
}


public class ValidationRangeTests : TestClass
{
    private readonly Node testScene;

    public ValidationRangeTests(Node testScene) : base(testScene)
    {
        this.testScene = testScene;
    }

    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public void RangeValidation()
    {
        Should.Throw<ArgumentException>(() => new ValidateRangeAttribute(
            min: 5,
            max: 0), "should not allow a min greater than max");
        Should.Throw<ArgumentException>(() => new ValidateRangeAttribute(
            min: 0,
            max: 0), "should not allow a min equal max");
    }

    [Test]
    public void RangeOtherTypes()
    {
        var testNode = new TestOtherTypesRangeNode();
        testScene.AddChild(testNode);

        Should.NotThrow(testNode.Validate,
            "int, float, decimal and double types should be supported");

        var testNode2 = new TestWrongTypeRangeNode();
        testScene.AddChild(testNode2);

        Should.Throw<FullValidationException>(testNode2.Validate,
            "int, float, decimal and double types should be supported");
    }

    private static Action<FullValidationException, string> CheckNodeFailure(string nodeName) =>
        (FullValidationException ex, string memberName) =>
            ex.ValidationFailureInfo.ShouldContain(info =>
                info.NodePath.EndsWith("/" + nodeName) &&
                info.MemberName.EndsWith(memberName));

    [Test]
    public void MinRangeInclusive()
    {
        var testNode = new TestRangeNode
        {
            Name = "MinRangeInclusiveTestNode"
        };

        var CheckFailure = CheckNodeFailure(testNode.Name);
        testScene.AddChild(testNode);

        testNode.ValueRangeMinInclusive0 = -10;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 inclusive fails on -10"),
            nameof(TestRangeNode.ValueRangeMinInclusive0));

        testNode.ValueRangeMinInclusive0 = 10;
        Should.NotThrow(testNode.Validate, "0 inclusive succeeds on 10");

        testNode.ValueRangeMinInclusive0 = 0;
        Should.NotThrow(testNode.Validate, "0 inclusive succeeds on 0");
    }

    [Test]
    public void MinRangeExclusive()
    {
        var testNode = new TestRangeNode
        {
            Name = "MinRangeExclusiveTestNode"
        };

        var CheckFailure = CheckNodeFailure(testNode.Name);
        testScene.AddChild(testNode);

        testNode.ValueRangeMinExclusive0 = -10;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 exclusive fails on -10"),
            nameof(TestRangeNode.ValueRangeMinExclusive0));

        testNode.ValueRangeMinExclusive0 = 10;
        Should.NotThrow(testNode.Validate, "0 exclusive succeeds on 10");

        testNode.ValueRangeMinExclusive0 = 0;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 exclusive fails on 0"),
            nameof(TestRangeNode.ValueRangeMinExclusive0));
    }

    [Test]
    public void MaxRangeInclusive()
    {
        var testNode = new TestRangeNode
        {
            Name = "MaxRangeInclusiveTestNode"
        };

        var CheckFailure = CheckNodeFailure(testNode.Name);
        testScene.AddChild(testNode);

        testNode.ValueRangeMaxInclusive0 = 10;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 inclusive fails on 10"),
            nameof(TestRangeNode.ValueRangeMaxInclusive0));

        testNode.ValueRangeMaxInclusive0 = -10;
        Should.NotThrow(testNode.Validate, "0 inclusive succeeds on -10");

        testNode.ValueRangeMaxInclusive0 = 0;
        Should.NotThrow(testNode.Validate, "0 inclusive succeeds on 0");
    }

    [Test]
    public void MaxRangeExclusive()
    {
        var testNode = new TestRangeNode
        {
            Name = "MaxRangeExclusiveTestNode"
        };

        var CheckFailure = CheckNodeFailure(testNode.Name);
        testScene.AddChild(testNode);

        testNode.ValueRangeMaxExclusive0 = 10;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 exclusive fails on 10"),
            nameof(TestRangeNode.ValueRangeMaxExclusive0));

        testNode.ValueRangeMaxExclusive0 = -10;
        Should.NotThrow(testNode.Validate, "0 exclusive succeeds on -10");

        testNode.ValueRangeMaxExclusive0 = 0;
        CheckFailure(Should.Throw<FullValidationException>(testNode.Validate,
            "0 exclusive fails on 0"),
            nameof(TestRangeNode.ValueRangeMaxExclusive0));
    }
}
