using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DotNet.UI.Util
{
    public static class EnumUtil
    {
        public static String GetDescription<T>(T en)
        {
            FieldInfo fi = en.GetType().GetField(en.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? en.ToString() : attribute.Description;
        }
    }
}
