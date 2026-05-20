using Services;

namespace BookingManager;

public class Menu
{
    private IHostService _hostService;

    private MenuList _appState = MenuList.Default;
    private string _command;
    
    public Menu(IHostService hostService)
    {
        _hostService = hostService;
    }
    
    private async Task UpdateState(string command)
    {
        command = command.Trim();
        command = command.ToLower();
        switch (command)
        {
            case "exit":
                _appState = MenuList.Exit;
                ExitProgram();
                break;
            case "back":
                _appState = MenuList.Default;
                DefaultState();
                break;
            case "remove host":
                _appState = MenuList.HostRemove;
                RemoveHostOperations();
                break;
            case "add host":
                _appState = MenuList.HostAdd;
                AddHostOperations();
                break;
            case "save changes":
                _appState = MenuList.SaveChanges;
                SaveChanges();
                break;
            default:
                switch (_appState)
                {
                    case MenuList.Default:
                        _appState =  MenuList.HostDetails;
                        ShowHostDetails(command);
                        break;
                    case MenuList.HostDetails:
                        _appState = MenuList.Default;
                        DefaultState();
                        break;
                    default:
                        Console.WriteLine("Unknown command. Please try again.");
                        _appState = MenuList.Default;
                        DefaultState();
                        break;
                }
                break;
        }
    }

    private void DefaultState()
    {
        Console.WriteLine("Here is the list of all hosts: ");
        foreach (var host in _hostService.GetHostsList())
        {
            Console.WriteLine($"Host: {host.FirstName} {host.LastName}, {host.Type}, phone: {host.Phone}");
        }
        Console.WriteLine();
        Console.WriteLine("Type the name and surname of the host / ID of the host to open his menu");
        Console.WriteLine("Type \"Remove Host\" to open the menu for removing the host");
        Console.WriteLine("Type \"Add Host\" to open the menu for removing the host");
        Console.WriteLine("Type \"Save Changes\" to save changes into the file");
    }

    private void ExitProgram()
    {
        Environment.Exit(0);
    }

    private async Task ReadUserInput()
    {
        while (true)
        {
            Console.WriteLine("Choose an option:");
            _command = Console.ReadLine();

            if (string.IsNullOrEmpty(_command))
            {
                Console.WriteLine("Please choose a valid command.");
                continue;
            }
            else
            {
                break;
            }
        }
        
        await UpdateState(_command);
    }

    public async Task ShowMenuAsync()
    {
        Console.WriteLine();

        /*foreach (MenuList item in Enum.GetValues(typeof(MenuList)))
        {
            Console.WriteLine($"{item}");
        }*/
      
        DefaultState();
        
        await ReadUserInput();
    }
    
    private void RemoveHostOperations()
    {
        Console.WriteLine("Remove the host and remove the host operations");        
    }
    
    private void AddHostOperations()
    {
        Console.WriteLine("Add the host and add the host operations");
    }
    
    private void SaveChanges()
    {
        Console.WriteLine("Save changes");
    }

    private void ShowHostDetails(string command)
    {
        Console.WriteLine($"Command: {command}");
    }
}