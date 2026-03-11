using System.Text;
using BookingManager.Common.Enums;
using BookingManager.Services;
using BookingManager.Common.Tools;
using BookingManager.DBModels;

namespace BookingManager.MainApp;

class Program
{
    enum AppState
    {
        Default = 1, 
        HostDetails, 
        HostRemove, 
        HostAdd, 
        HostUpdate,
        SaveChanges,
        End, 
        Exit = 100
    }
    private static AppState _appState = AppState.Default;
    private static StorageService _storageService;
    private static List<Host> _hosts;
    
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome To Booking Manager");
        
        _storageService = new StorageService();
        _storageService.LoadDataFromFile();
        string? command = null;
        
        // Main App Loop
        while (_appState != AppState.Exit)
        {
            switch (_appState)
            {
                case AppState.HostDetails:
                    HostDetailsState(command);
                    if (_appState  == AppState.HostUpdate) 
                        UpdateHostInLists(command);
                    break;
                case AppState.Default:
                    DefaultState();
                    break;
                case AppState.HostRemove:
                    RemoveHostResult(RemoveHost());
                    break;
                case AppState.HostAdd:
                    HostDBModel newHost = CreateHostDb();
                    Console.WriteLine("New Host successfully created! ");
                    AddHostUi(newHost);
                    _storageService.AddHost(newHost);
                    break;
                case AppState.SaveChanges:
                    _storageService.SaveDataToFile();
                    Console.WriteLine("Changes saved");
                    break;
                default:
                    break;
            }
            
            Console.WriteLine("Type Exit to exit the application");
            command = Console.ReadLine();
            UpdateState(command);
        }
        
        Console.WriteLine("Thanks for using the booking manager. Bye!");
        //_storageService.SaveDataToFile();
    }

    // Method to change apptate property by the input (command) user gives.
    // Defines App Behaviour based on user input
    private static void UpdateState(string command)
    {
        command = command.Trim();
        command = command.ToLower();
        switch (command)
        {
            case "exit":
                _appState = AppState.Exit;
                break;
            case "back":
                _appState = AppState.Default;
                break;
            case "remove host":
                _appState = AppState.HostRemove;
                break;
            case "add host":
                _appState =  AppState.HostAdd;
                break;
            case "save changes":
                _appState = AppState.SaveChanges;
                break;
            default:
                switch (_appState)
                {
                    case AppState.Default:
                        _appState =  AppState.HostDetails;
                        break;
                    case AppState.HostDetails:
                        _appState = AppState.Default;
                        break;
                    default:
                        Console.WriteLine("Unknown command. Please try again.");
                        _appState = AppState.Default;
                        break;
                }
                break;
        }
    }

    // Method used to show default state of application, which shows a list of
    // all hosts and then prompts user for next command
    private static void DefaultState()
    {
        Console.WriteLine("Here is a list of all Hosts");
        LoadHosts();
        foreach (var host in _hosts)
        {
            host.LoadApartments(_storageService);
            Console.WriteLine(host);
        }
        Console.WriteLine("Type the name and surname of the host / ID of the host to open his menu");
        Console.WriteLine("Type \"Remove Host\" to open the menu for removing the host");
        Console.WriteLine("Type \"Add Host\" to open the menu for removing the host");
        Console.WriteLine("Type \"Save Changes\" to save changes into the file");
    }

    // Method, which shows info about particular host, which name or id user gives 
    // as input (command argument)
    private static void HostDetailsState(string command)
    {
        command = command.Trim();
        command = command.ToLower();
        bool hostExists = false;
        
        // Find host by id or name
        Host host = null;
        if (Common.Tools.Common.ChoiceNumberIsValid(command))
        {
            host = FindHostById(int.Parse(command));
        }
        else
        {
            host = FindHostByName(command);
        }
        
        if (host != null)
        {
            hostExists = true;
            
            // if no apartments
            if (host.Apartments.Count <= 0)
            {
                Console.WriteLine("No apartments found for this host.");
            }
            else
            {
                Console.WriteLine("Here is a List of Apartments belonging to " + host.FirstName + " " +
                                  host.LastName);
                foreach (var apartment in host.Apartments)
                {
                    Console.WriteLine(apartment);
                }
            }

        }

        if (!hostExists)
        {
            Console.WriteLine("Haven't found the host. Try again");
        }
        
        // Prompts whether user wants to update host entity
        else
        {
            Console.WriteLine("Type \"Update Host\" if you want to update the host");
            string choice = Console.ReadLine().Trim().ToLower();
            switch (choice)
            {
                case "update host":
                    _appState = AppState.HostUpdate;
                    break;
                default:
                    Console.WriteLine("Type Back to see the list of hosts");
                    _appState = AppState.Default;
                    break;
            }
        }
    }

    // Method loads all host entities into the list using 
    // StorageService field
    private static void LoadHosts()
    {
        _hosts = new List<Host>();
        foreach (var host in _storageService.GetAllHosts())
        {
            var hostUIModel = new Host(host);
            hostUIModel.LoadApartments(_storageService);
            _hosts.Add(hostUIModel);
        }
    }

    // Prompt the user to create host, and returns Host DB entity
    private static HostDBModel CreateHostDb()
    {
        Console.WriteLine("Menu For Creating A Host: ");

        // Checking if fullname is already occupied of reserved
        string name = "";
        string surname = "";
        string fullName = "";
        while (true)
        {
            name = Common.Tools.Common.PromptUserForNameInConsole("Name: ");
            surname = Common.Tools.Common.PromptUserForNameInConsole("Surname: ");
            fullName =  name + " " + surname;
            if (!IsNameDuplicate(fullName) && !IsNameReserved(fullName)) 
                break;
            Console.WriteLine("Fullname already occupied or reserved name. Please try again.");
        }
        
        // Phone, email, date of birth
        string phone = Common.Tools.Common.PromptUserForPhoneInConsole();
        string email = Common.Tools.Common.PromptUserForEmailInConsole();
        DateTime dateOfBirth = Common.Tools.Common.PromptUserForDateInConsole();
        
        // Choosing a host position (from enum HostType)
        HostType hostType = Common.Tools.Common.PromptUserForHostTypeInConsole();
        
        return new HostDBModel(name, surname, hostType, email, phone, dateOfBirth);
    }

    // Adds host to UI Host list 
    private static void AddHostUi(HostDBModel host)
    {
        _hosts.Add(new Host(host));
    }
    
    // Prompts the user needed info to remove the host and
    // removes it from UI list and from storage service
    private static bool RemoveHost()
    {
        Console.Write("Input the id of host you want to remove: ");
        var id = Console.ReadLine();
        bool valid = Common.Tools.Common.ChoiceNumberIsValid(id);
        
        if (!valid)
        {
            Console.WriteLine("Invalid id. Please try again.");
            return false;
        }
        
        int numId = int.Parse(id);
        Host host = FindHostById(numId);
        _hosts.Remove(host);
        return _storageService.RemoveHost(numId); 
    }
    
    private static void RemoveHostResult(bool removed)
    {
        string result = (removed) ? "Successfully Removed Host" : "Failed to Remove";
        Console.WriteLine(result);
    }

    // Updates the host on UI and DB part (storage service)
    private static void UpdateHostInLists(string command)
    {
        // Ui part
        Host updatedHost = UpdateHostUi(command);
        int index = FindHostIndexByName(updatedHost.FirstName + " " +  updatedHost.LastName);
        _hosts.RemoveAt(index);
        _hosts.Insert(index, updatedHost);
                  
        // DB part
        HostDBModel hostDbModel = new HostDBModel(updatedHost.FirstName, updatedHost.LastName, 
            updatedHost.Type, updatedHost.Email, updatedHost.PhoneNumber, updatedHost.DateOfBirth);
        hostDbModel.ChangeId(updatedHost.Id);;
        _storageService.UpdateHost(hostDbModel);
    }

    // Prompts the user what property they want to update
    private static Host UpdateHostUi(string command)
    {
        
        // Find host by id or fullname
        Host hostToUpdate = null;
        if (Common.Tools.Common.ChoiceNumberIsValid(command))
        {
            hostToUpdate = FindHostById(int.Parse(command));
        }
        else
        {
            hostToUpdate = FindHostByName(command);
        }

        if (hostToUpdate == null)
        {
            Console.WriteLine("Host not found.");
            return hostToUpdate;
        }
        
        Console.Write("Input the name of property of the host you want to change: ");
        var property = Console.ReadLine();
        switch (property)
        {
            case "Name":
                hostToUpdate.FirstName = PromptHostNameForNoDuplicate("Enter new Name: ", false, hostToUpdate.LastName);
                Console.WriteLine("Name Successully Changed");
                break;
            case "Surname":
                hostToUpdate.LastName = PromptHostNameForNoDuplicate("Enter new Surname: ", true, hostToUpdate.FirstName);
                Console.WriteLine("Surname Successully Changed");
                break;
            case "Email":
                hostToUpdate.Email = Common.Tools.Common.PromptUserForEmailInConsole();
                break;
            case "Phone":
                hostToUpdate.PhoneNumber = Common.Tools.Common.PromptUserForPhoneInConsole();
                break;
            case "Birth Date":
                hostToUpdate.DateOfBirth = Common.Tools.Common.PromptUserForDateInConsole();
                break;
            case "Position":
                hostToUpdate.Type = Common.Tools.Common.PromptUserForHostTypeInConsole();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }

        _appState = AppState.HostDetails;
        return hostToUpdate;
    }

    private static int FindHostIndexByName(string hostName)
    {
        hostName = hostName.ToLower();
        hostName = hostName.Trim();
        foreach (var host in _hosts)
        {
            if ((host.FirstName.ToLower() + " " + host.LastName.ToLower()) == hostName)
            {
                return _hosts.IndexOf(host);
            }
        }
        return -1;
    }

    private static Host FindHostByName(string hostName)
    {
        hostName = hostName.ToLower();
        hostName = hostName.Trim();
        foreach (var host in _hosts)
        {
            if ((host.FirstName.ToLower() + " " + host.LastName.ToLower()) == hostName)
            {
                return host;
            }
        }
        return null;
    }

    private static Host FindHostById(int hostId)
    {
        foreach (var host in _hosts)
        {
            if (host.Id == hostId)
                return  host;
        }

        return null;
    }

    private static bool IsNameDuplicate(string fullName)
    {
        fullName = fullName.ToLower();
        fullName = fullName.Trim();
        foreach (var host in _hosts)
        {
            if ((host.FirstName.ToLower() + " " + host.LastName.ToLower()) == fullName)
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsNameReserved(string fullName)
    {
        fullName = fullName.ToLower();
        fullName =  fullName.Trim();
        switch (fullName)
        {
            case "remove host":
                return true;
            case "add host":
                return true;
            case "save changes":
                return true;
            default:
                return false;
        }
    }

    // Prompts the user for name and surname while fullname already exists or is 
    // one of coordination commands
    private static string PromptHostNameForNoDuplicate(string prompt, bool isSurname, string otherInitials)
    {
        string newName = "";
        string wholeName = "";
        while (true)
        {
            newName = Common.Tools.Common.PromptUserForNameInConsole(prompt);
            if (isSurname)
                wholeName = otherInitials + " " + newName;
            else 
                wholeName = newName + " " + otherInitials;
            if (IsNameDuplicate(wholeName) || IsNameReserved(wholeName))
                Console.WriteLine("Name already exists. Try again.");
            else 
                break;
        }
        return newName;
    }
}