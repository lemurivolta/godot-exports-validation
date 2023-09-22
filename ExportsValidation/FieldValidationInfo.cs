using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Godot;

namespace LemuRivolta.ExportsValidation
{
    public class FieldValidationInfo : ValidationInfo
    {
        private readonly FieldInfo fieldInfo;

        public FieldValidationInfo(Node node, FieldInfo fieldInfo)
            : base(fieldInfo, node)
        {
            this.fieldInfo = fieldInfo;
        }

        public override Type MemberType => fieldInfo.FieldType;

        public override object Value => fieldInfo.GetValue(Node)!;
    }
}
