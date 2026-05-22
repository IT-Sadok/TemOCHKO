using Models;

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
    
    public static DateTime PromptUserForDateInConsole(string prompt)
    {
        DateTime date = DateTime.Now;
        bool goodDate = false;
        do
        {
            Console.WriteLine(prompt);
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
            else
            {
                date = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day)); 
            }
        } while (!goodDate);

        return date;
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
    
    public static HostType PromptUserForHostTypeInConsole()
    {
        int counter = 0;
        foreach (var type in Enum.GetNames(typeof(HostType)))
        {
            counter++;
            Console.WriteLine($"{counter}. {type}");
        }

        var hostTypeLength = HostType.GetValuesAsUnderlyingType<HostType>().Length;
        int choice = -1;
        var userInput = "";
        do
        {
            Console.WriteLine("Choose a type of host (input a number): ");
            userInput = Console.ReadLine();
            if (ChoiceNumberIsValid(userInput)) 
            {
                choice  = int.Parse(userInput);
            }
        } while (choice < 1 || choice > hostTypeLength);
        
        Console.WriteLine("Successfully validated host position.");
        return (HostType)(choice - 1);
    }
}