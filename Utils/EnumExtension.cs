using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Utils
{
    public static class EnumExtension
    {
        public static string GetDescription(this System.Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute), true);
            if (attribute != null && attribute is DescriptionAttribute descriptionAttribute)
            {
                return descriptionAttribute.Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static string GetDescription<T>(string enumName)
        {
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                if (e.ToString() != enumName)
                    continue;

                var objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr.Length <= 0) continue;
                if (objArr[0] is DescriptionAttribute da) return da.Description;
            }

            return "";
        }

        public static List<EnumValueObject> ToList<T>()
        {
            var list = new List<EnumValueObject>();
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                var m = new EnumValueObject();
                var objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr.Length > 0)
                {
                    if (objArr[0] is DescriptionAttribute da) m.Description = da.Description;
                }

                m.Value = Convert.ToInt32(e);
                m.Name = e.ToString();
                list.Add(m);
            }

            return list;
        }
    }

    public class EnumValueObject
    {
        public string Description { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }
    }
}