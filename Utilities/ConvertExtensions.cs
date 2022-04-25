namespace Plus.Utilities;

public static partial class ConvertExtensions
{
    public static string ToStringEnumValue(bool value)
    {
        return value ? "1" : "0";
    }
    
    public static bool EnumToBool(string value)
    {
        return value == "1";
    }
    
    public static int ToInt32(this bool value)
    {
        return value ? 1 : 0;
    }
}