namespace TelegramBotApp.Extensions;

public static class StringExtensions
{
    public static string TrimBothSides(this string str, int leftTrimScale, int rightTrimScale)
    {
        if (leftTrimScale + rightTrimScale > str.Length)
            throw new IndexOutOfRangeException();
        
        var trimString = String.Empty;
        
        if (str.Length > 3)
            trimString = str.Substring(leftTrimScale, str.Length - 1 - rightTrimScale);

        return trimString;
    }
}