using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Godot;

namespace LemuRivolta.ExportsValidation
{
    [Serializable]
    public record ValidationFailureInfo(
            string NodePath, string MemberName, string Message);

    public class FullValidationException : Exception
    {
        private readonly IEnumerable<ValidationFailureInfo> validationFailureInfo;

        public IEnumerable<ValidationFailureInfo> ValidationFailureInfo =>
            validationFailureInfo;

        public FullValidationException(
            IEnumerable<ValidationFailureInfo> validationFailureInfo)
            : base(GetMessage(validationFailureInfo))
        {
            this.validationFailureInfo = validationFailureInfo;
        }

        protected FullValidationException(
            System.Runtime.Serialization.SerializationInfo serializationInfo,
            System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            validationFailureInfo = Array.Empty<ValidationFailureInfo>();
        }

        private static string GetMessage(IEnumerable<ValidationFailureInfo> info) =>
            "Validation failed:\n" + string.Join('\n',
                from i in info
                select $"  {i.NodePath}::{i.MemberName} - {i.Message}"
            );
    }
}
