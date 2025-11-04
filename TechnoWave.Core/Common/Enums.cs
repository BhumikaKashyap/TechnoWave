using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TechnoWave.Core.Common
{
   public class Enums
    {
        public enum TemplateType
        {
            [EnumMember(Value = "Email")]
            Email,
            [EnumMember(Value = "PDF")]
            PDF
        }

        public enum DownloadSample
        {
            Material,
            Contact,
        }

        public static class EnumValue
        {
            public static string GetEnumValue<T>(T enumValue) where T : Enum
            {
                var type = enumValue.GetType();
                var memInfo = type.GetMember(enumValue.ToString());
                var attributes = memInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);
                return ((EnumMemberAttribute)attributes[0]).Value;
            }
        }
    }
}
