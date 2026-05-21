namespace Tools;

public class Common
{
    public static bool ChoiceNumberIsValid(string choice)
    {
        choice = choice.Trim();
        if (string.IsNullOrEmpty(choice)) return false;
        if (choice.ToCharArray()[0] == '0') return false;
        foreach (var c in choice)
        {
            if (!char.IsDigit(c)) return false;
        }

        return true;
    }
}