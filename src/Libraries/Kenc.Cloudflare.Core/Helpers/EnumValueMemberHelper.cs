namespace Kenc.Cloudflare.Core.Helpers
{
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;

    public static class EnumValueMemberHelper
    {
        /// <summary>
        /// Converts an enum value to it's enum member value.
        /// Modified version of: http://www.wackylabs.net/2006/06/getting-the-xmlenumattribute-value-for-an-enum-field/
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns>A string representation of the name.</returns>
        public static string ConvertToString(this Enum enumValue)
        {
            // Get the Type of the enum
            Type type = enumValue.GetType();

            // Get the FieldInfo for the member field with the enums name
            FieldInfo info = type.GetField(enumValue.ToString("G"));

            // Check to see if EnumMember is defined on this field
            if (!info.IsDefined(typeof(EnumMemberAttribute), false))
            {
                return enumValue.ToString("G");
            }

            var o = info.GetCustomAttributes(typeof(EnumMemberAttribute), false);
            var att = (EnumMemberAttribute)o[0];
            return att.Value;
        }
    }
}
