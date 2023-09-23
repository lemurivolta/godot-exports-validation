namespace LemuRivolta.ExportsValidation;

using System;

public class ValidationError
{
    public string Message { get; private set; }

    public ValidationError(string message)
    {
        Message = message;
    }
}