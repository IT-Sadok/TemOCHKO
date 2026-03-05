using BookingManager.Services;

namespace BookingManager;

class Program
{

    enum AppState
    {
        Default = 1, 
        HostDetails, 
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
                    break;
                case AppState.Default:
                    DefaultState();
                    break;
                default:
                    break;
            }
            
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
            Console.WriteLine("Type Back to see the list of hosts");
            _appState = AppState.Default;
        }
    }

    private static void LoadHosts()
    {
        if (_hosts != null)
        {
            return;
        }

        _hosts = new List<Host>();
        foreach (var host in _storageService.GetAllHosts())
        {
            var hostUIModel = new Host(host);
            hostUIModel.LoadApartments(_storageService);
            _hosts.Add(hostUIModel);
        }
    }
}