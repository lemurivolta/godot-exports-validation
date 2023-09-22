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
  private Path path;

  [Export, ValidateNotEmpty]
  private string name;

  [Export]
  [ValidatePackedSceneType(typeof(Bullet))]
  private PackedScene bulletPackedScene;

  [Export]
  [ValidateRange(Min = 0, MinInclusive = false)]
  private float speed;
  ```
3. Remember to *call Validate*! Typically, you want to do this on _Ready:
  ```csharp
  public override void _Ready() {
    // extension method, it required "this."
    this.Validate();
  }
  ```
4. **You're done!**

# Supported validators

All validators are expressed as attributes, and work on both properties and on fields.

Here is the list of the currently available validators.

## `ValidateNonNull`

This validator checks that the field (or property) is **not null**. It takes no arguments and has no properties.

## `ValidateNotEmpty`

This validator checks that a **`string` is not empty**.

By default, strings only made of white spaces are considered empty too. You can pass `NoWhiteSpace = false` as a property to the attribute to disable this behavior.

E.g.:

```csharp
[ValidateNotEmpty]
public string? Value = "hello";

[ValidateNotEmpty(NoWhiteSpace = false)]
public string? ValueThatAcceptsWhiteSpaces =
  "   ";
```

## `ValidateRange`

This validator checks that a **numeric field (or property) is within a given range**.

Ranges are expressed using arguments `min` and `max`. You can pass just min or just max, if the other end of the range must not be checked.

By default, all ranges are inclusive. The `minInclusive` and `maxInclusive` arguments are flags that can be set to `false` so that the respective extreme is not considered valid for the range.

Supported numeric types are `int`, `float`, `double` and `decimal`.

E.g.:

```csharp
[ValidateRange(min: 0)]
public int ValueRangeMinInclusive0 = 0;

[ValidateRange(min: 0, minInclusive: false)]
public float ValueRangeMinExclusive0 = 1;

[ValidateRange(max: 10)]
public double ValueRangeMaxInclusive0 = 10;

[ValidateRange(max: 10, maxInclusive: false)]
public decimal ValueRangeMaxExclusive0 = 9;

[ValidateRange(min: 0, max: 10, maxInclusive: false)]
public int ValueRangeMinInclusive0MaxExclusive0 = 5;
```

## `ValidatePackedSceneType`

This validator checks that a **PackedScene field (or property) points to a scene that is of the given type**. The type is passed as its only argument.

E.g.:

```csharp
[ValidatePackedSceneType(typeof(PuzzlePiece))]
public PackedScene? PuzzlePieceScene = null;
```

The check `[ValidatePackedSceneType(typeof(MyScript))]` is satisfied if the root node of the PackedScene has a C# script attached to it of type `MyScript`, or of a type derived from `MyScript`.

In other words, instances of the packed scene must be assignable to variables ot type `MyScript`.

---
---
---

🐣 Package generated from a 🐤 Chickensoft Template — <https://chickensoft.games>

[line-coverage]: ExportsValidation.Tests/badges/line_coverage.svg
[branch-coverage]: ExportsValidation.Tests/badges/branch_coverage.svg