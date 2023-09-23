# Exports Validation

![line coverage][line-coverage] ![branch coverage][branch-coverage]

A .NET package to easily **validate exports field in your Godot C# Scripts**.

Validation allows you to immediately see if there's something wrong with the setup of your nodes, before getting cryptic messages five minutes into a debugging session.

---

<p align="center">
<img alt="ExportsValidation" src="ExportsValidation/icon.svg" width="200">
</p>

## Quick Start

Want to try it out? Quickly get your project to use Exports Validation!

1. *Install the NuGet package*. You can search for **LemuRivolta.ExportsValidation** using [Visual Studio](https://learn.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio#nuget-package-manager), [VSCode](https://code.visualstudio.com/docs/csharp/package-management) or do it on the [command line](https://learn.microsoft.com/en-us/nuget/reference/cli-reference/cli-ref-install).
2. *Add some validation attributes* to your fields or properties, like this:
  ```csharp
[Export, ValidateNonNull]
private Path2D path;

[Export, ValidateNonEmpty]
private string name;

[Export, ValidateMin(0, Inclusive = false)]
private float speed;

[Export]
[ValidatePackedSceneType(typeof(Bullet))]
private PackedScene bulletPackedScene;
  ```
3. Remember to *call Validate*! Typically, you want to do this on _Ready:
  ```csharp
public override void _Ready()
{
    // extension method, it requires "this."
    this.Validate();
}
  ```
4. You *may* need to rebuild the project from Visual Studio / using dotnet build. This is only needed the first time.
4. **You're done!**,

Validation errors will be displayed when nodes are instantiated, and you can see them under Errors => Debugger:

![A screenshot of Godot, showing that one must click under "Debugger" and then "Errors" to see various validation errors](images/errors.png)

# Supported validators

All validators are expressed as attributes, and work on both properties and fields.

Here is the list of the currently available validators.

## `ValidateNonNull`

This validator checks that the field (or property) is **not null**.

## `ValidateNonEmpty`

This validator checks that a string member **is not empty**.

By default, strings only made of white spaces are considered empty too. You can pass `NoWhiteSpace = false` as a property to the attribute to disable this behavior.

E.g.:

```csharp
// valid
[ValidateNonEmpty]
public string Value = "hello";

// fails validation
[ValidateNonEmpty]
public string ValueOnlyWhiteSpaces = "   ";

// valid
[ValidateNonEmpty(NoWhiteSpace = false)]
public string? ValueThatAcceptsWhiteSpaces = "   ";

// fails validation
[ValidateNonEmpty]
public string ValueEmpty = "";

// fails validation
[ValidateNonEmpty]
public string ValueNull = null;
```

## `ValidateMin` and `ValidateMax`

These validators checks that a **numeric field (or property) is greater or lesser than a value**. The value to check against (minimum or maximum) is passed as a parameter to the attribute.

`[ValidateMin(min)]` can be used to check that a value is _at least_ a certain amount, and `[ValidateMax(max)]` to check that is _at most_ it. Using them together allows to check for a value to be in a certain range.

By default, both attributes interpret the range as inclusive (the parameter is a valid value for the member). The parameter `Inclusive = false` can be passed to make that part of the range exclusive.

Numeric types that are supported for the check are `int`, `float`, `double` and `decimal`.

E.g.:

```csharp
[ValidateMin(0)]
public int ValueRangeMinInclusive0 = 0;

[ValidateMin(0, Inclusive = false)]
public int ValueRangeMinExclusive0 { get; set; } = 10;

[ValidateMax(0)]
public int ValueRangeMaxInclusive0 = 0;

[ValidateMax(0, Inclusive = false)]
public int ValueRangeMaxExclusive0 { get; set; } = -10;

// valid values are between 0 and 10; 0 is valid and 10 is not.
[ValidateMin(0), ValidateMax(10, Inclusive = false)]
public int ValueRangeMinInclusive0MaxExclusive0 = 5;
```

## `ValidatePackedSceneType`

This validator checks that a **PackedScene field (or property) points to a scene that is of the given type**. The type is its parameter.

E.g.:

```csharp
[ValidatePackedSceneType(typeof(PuzzlePiece))]
public PackedScene PuzzlePieceScene = null;
```

The check `[ValidatePackedSceneType(typeof(MyScript))]` is satisfied if the root node of the PackedScene has a C# script attached to it of type `MyScript`, or of a type derived from `MyScript`.

In other words, instances of the packed scene must be assignable to variables ot type `MyScript`.

By default, the `null` value is not valid. If the `null` value is valid, add `AllowNullValues = true` as a parameter to the attribute. E.g.:

```csharp
[ValidatePackedSceneType(
    typeof(PuzzlePiece),
    AllowNullValues = true)]
public PackedScene PuzzlePieceScene = null;
```

---
---
---

🐣 Package generated from a 🐤 Chickensoft Template — <https://chickensoft.games>

[line-coverage]: ExportsValidation.Tests/badges/line_coverage.svg
[branch-coverage]: ExportsValidation.Tests/badges/branch_coverage.svg