using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Godot;

namespace LemuRivolta.ExportsValidation
{
    public class PropertyValidationInfo : ValidationInfo
    {
        private readonly PropertyInfo propertyInfo;

        public PropertyValidationInfo(Node node, PropertyInfo propertyInfo)
            : base(propertyInfo, node)
        {
            this.propertyInfo = propertyInfo;
        }

        public override Type MemberType => propertyInfo.PropertyType;

        public override object Value => propertyInfo.GetValue(Node)!;

    }
}
