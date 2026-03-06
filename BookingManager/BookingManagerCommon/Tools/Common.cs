using System.Runtime.InteropServices.JavaScript;

namespace BookingManager.Common.Tools;

public static class Common
{
    private static bool PhoneValid(string phone)
    {
        phone = phone.Trim();
        if (string.IsNullOrEmpty(phone)) return false;

        foreach (char c in phone.ToCharArray())
        {
            if (!char.IsDigit(c))
            {
                if (c == '+' && phone.IndexOf(c) == 0) continue;
                return false;
            }
        }

        return true;
    }

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

    private static bool DateValid(string day, string month, string year)
    {
        if (!ChoiceNumberIsValid(day)) return false;
        if (!ChoiceNumberIsValid(month)) return false;
        if (!ChoiceNumberIsValid(year)) return false;

        var dayNum = int.Parse(day);
        var monthNum = int.Parse(month);
        var yearNum = int.Parse(year);

        if (dayNum < 1 || dayNum > 31) return false;
        if (monthNum < 1 || monthNum > 12) return false;
        if (yearNum < 1900 || yearNum > DateTime.Now.Year) return false;

        return true;
    }

    public static DateTime PromptUserForDateInConsole()
    {
        DateTime date = DateTime.Now;
        bool goodDate = false;
        do
        {
            Console.WriteLine("Select a birth date of host: ");
            Console.Write("Day: ");
            string day = Console.ReadLine();
            Console.Write("Month: ");
            string month = Console.ReadLine();
            Console.Write("Year: ");
            string year = Console.ReadLine();

            goodDate = DateValid(day, month, year);
            if (!goodDate)
            {
                Console.WriteLine("Invalid date. Please try again.");
            }
        } while (!goodDate);

        return date;
    }

    public static string PromptUserForPhoneInConsole()
    {
        string phoneNumber = "";
        do
        {
            Console.Write("Phone Number: ");
            phoneNumber = Console.ReadLine();
        } while (!PhoneValid(phoneNumber));

        return phoneNumber;
    }

    public static string PromptUserForNameInConsole(string prompt)
    {

        string name = "";
        do
        {
            Console.Write(prompt);
            name = Console.ReadLine();
            name = name.Trim();
        } while (string.IsNullOrEmpty(name));

        return name;
    }

    public static string PromptUserForEmailInConsole()
    {
        string email = "";
        do
        {
            Console.Write("Email: ");
            email = Console.ReadLine().Trim();
        } while (!email.Contains('@') && email.Length < 5);

        return email;
    }

}