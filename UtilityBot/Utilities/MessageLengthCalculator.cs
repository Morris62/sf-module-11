namespace UtilityBot.Utilities;

public static class  MessageLengthCalculator
{
    public static string Process(string input)
    {
        return $"В вашем сообщении {input?.Length} символов";
    }
}