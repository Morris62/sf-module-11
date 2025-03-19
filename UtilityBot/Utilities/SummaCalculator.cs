namespace UtilityBot.Utilities;

public static class SummaCalculator
{
    public static string Process(string input)
    {
        string result = string.Empty;
        try
        {
            result = $"Сумма чисел: {input.Split(' ').Sum(int.Parse)}";
        }
        catch (Exception e)
        {
            result = "Не удалось вычислить сумму чисел! Попробуйте еще раз";
        }

        return result;
    }
}