using System.ComponentModel;
using System.Reflection;

namespace RonWeb.Core;
public static class EnumTool
{
    public static string Description(this Enum value)
    {
        // 獲取列舉值的類型
        Type type = value.GetType();
        // 獲取列舉值的名稱
        string name = Enum.GetName(type, value)!;
        // 獲取列舉值的 FieldInfo 對象
        FieldInfo field = type.GetField(name)!;
        // 獲取列舉值的 DescriptionAttribute 特性
        DescriptionAttribute? attribute =
            Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

        // 如果有 DescriptionAttribute 特性，返回其值，否則返回列舉值的名稱
        return attribute == null ? name : attribute.Description;
    }
}