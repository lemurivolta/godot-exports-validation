namespace LemuRivolta.ExportValidation;

using System;

[Serializable]
public class ValidationFailedException : Exception
{
    public ValidationFailedException() { }
    public ValidationFailedException(string message) : base(message) { }
    public ValidationFailedException(string message, Exception inner) : base(message, inner) { }
    protected ValidationFailedException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
