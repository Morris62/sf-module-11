namespace VoiceTextBot.Extentions;

public class StringExtention
{
    public static string UppercaseFirst(string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        return char.ToUpper(str[0]) + str.Substring(1);
    }
}