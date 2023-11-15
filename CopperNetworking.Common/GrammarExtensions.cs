namespace CopperNetworking.Common;

public static partial class GrammarExtensions
{
    public static string CorrectForm(int amount, string singular, string plural = "")
    {
        if (string.IsNullOrEmpty(plural))
            plural = $"{singular}s";

        return amount == 1 ? singular : plural;
    }
}