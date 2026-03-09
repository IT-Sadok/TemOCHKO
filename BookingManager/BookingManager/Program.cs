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
        _storageService.LoadData();
        string? command = null;
        
        while (_appState != AppState.Exit)
        {
            switch (_appState)
            {
                case AppState.HostDetails:
                    HostDetailsState(command);
                    if (_appState  == AppState.HostUpdate) {
                        Host updatedHost = UpdateHostUi(command);
                        int index = FindHostIndexByName(updatedHost.FirstName);
                        _hosts.RemoveAt(index);
                        _hosts.Insert(index, updatedHost);
                        
                        HostDBModel hostDbModel = new HostDBModel(updatedHost.FirstName, updatedHost.LastName, 
                            updatedHost.Type, updatedHost.Email, updatedHost.PhoneNumber, updatedHost.DateOfBirth);
                        hostDbModel.Id = updatedHost.Id;
                        _storageService.UpdateHost(hostDbModel);
                    }
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
                default:
                    break;
            }
            
            Console.WriteLine("Type back to see the menu");
            Console.WriteLine("Type Exit to exit the application");
            command = Console.ReadLine();
            UpdateState(command);
        }
        
        Console.WriteLine("Thank s for using the booking manager. Bye!");
    }

    private static void UpdateState(string command)
    {
        command = command.Trim();
        command = command.ToLower();
        switch (command)
        {
            case  "exit":
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

    private static void DefaultState()
    {
        Console.WriteLine("Here is a list of all Hosts");
        LoadHosts();
        foreach (var host in _hosts)
        {
            host.LoadApartments(_storageService);
            Console.WriteLine(host);
        }
        Console.WriteLine("Type the name of the host to see its apartments");
        Console.WriteLine("Type \"Remove Host\" to open the menu for removing the host");
        Console.WriteLine("Type \"Add Host\" to open the menu for removing the host");
    }

    private static void HostDetailsState(string command)
    {
        command = command.Trim();
        command = command.ToLower();
        bool cinemaHallExists = false;
        foreach (var host in _hosts)
        {
            if (host.FirstName.ToLower() == command || host.LastName.ToLower() == command ||
                (host.FirstName.ToLower() + " " + host.LastName.ToLower()) == command)
            {
                cinemaHallExists = true;
                if (host.Apartments.Count <= 0)
                {
                    Console.WriteLine("No apartments found for this host.");
                }
                else {
                    Console.WriteLine("Here is a List of Apartments belonging to " + host.FirstName + " " +
                                      host.LastName);
                    foreach (var apartment in host.Apartments)
                    {
                        Console.WriteLine(apartment);
                    }
                }
            }
        }

        if (!cinemaHallExists)
        {
            Console.WriteLine("Haven't found the host");
        }
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

    private static HostDBModel CreateHostDb()
    {
        Console.WriteLine("Menu For Creating A Host: ");
        string name = Common.Tools.Common.PromptUserForNameInConsole("Name: ");
        string surname = Common.Tools.Common.PromptUserForNameInConsole("Surname: ");
        string phone = Common.Tools.Common.PromptUserForPhoneInConsole();
        string email = Common.Tools.Common.PromptUserForEmailInConsole();
        DateTime dateOfBirth = Common.Tools.Common.PromptUserForDateInConsole();
        
        Console.WriteLine("Choose a type of host (input a number): ");
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
            userInput = Console.ReadLine();
            if (!Common.Tools.Common.ChoiceNumberIsValid(userInput)) 
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
            else
            {
                choice  = int.Parse(userInput);
            }
        } while (choice < 1 || choice > hostTypeLength);
        HostType hostType = (HostType)(choice - 1);
        
        return new HostDBModel(name, surname, hostType, email, phone, dateOfBirth);
    }

    private static void AddHostUi(HostDBModel host)
    {
        _hosts.Add(new Host(host));
    }
    
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
        return _storageService.RemoveHost(numId); 
    }

    private static void RemoveHostResult(bool removed)
    {
        string result = (removed) ? "Successfully Removed Host" : "Failed to Remove";
        Console.WriteLine(result);
    }

    private static Host UpdateHostUi(string command)
    {
        Host hostToUpdate = null;
        foreach (var host in _hosts)
        {
            if (host.FirstName.ToLower() == command || host.LastName.ToLower() == command ||
                (host.FirstName.ToLower() + " " + host.LastName.ToLower()) == command)
            {
                hostToUpdate = host;
                break;
            }
        }

        Console.Write("Input the name of property of the host you want to change: ");
        var property = Console.ReadLine();
        switch (property)
        {
            case "Name":
                hostToUpdate.FirstName = Common.Tools.Common.PromptUserForNameInConsole("Enter new name: ");
                break;
            case "Surname":
                hostToUpdate.LastName = Common.Tools.Common.PromptUserForNameInConsole("Enter new surname: ");
                break;
            case "Email":
                hostToUpdate.Email = Common.Tools.Common.PromptUserForEmailInConsole();
                break;
            case "Phone":
                hostToUpdate.PhoneNumber = Common.Tools.Common.PromptUserForPhoneInConsole();
                break;
            case "Date Of Birth":
                hostToUpdate.DateOfBirth = Common.Tools.Common.PromptUserForDateInConsole();
                break;
            case "Position":
                hostToUpdate.Type = Common.Tools.Common.PromptUserForHostTypeInConsole();
                break;
            default:
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
            if (host.FirstName.ToLower() == hostName || host.LastName.ToLower() == hostName ||
                (host.FirstName.ToLower() + " " + host.LastName.ToLower()) == hostName)
            {
                return _hosts.IndexOf(host);
            }
        }

        return -1;
    }
}