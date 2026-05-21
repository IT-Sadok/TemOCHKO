using Models.DTOs;
using Repositories;
using Services;
using Tools;

namespace BookingManager;

public class Menu
{
    private IHostService _hostService;
    private IApartmentService _apartmentService;

    private MenuList _appState = MenuList.Default;
    private string _command;
    
    public Menu(IHostService hostService, IApartmentService apartmentService)
    {
        _hostService = hostService;
        _apartmentService = apartmentService;
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
            Console.WriteLine($"Host: {host.FirstName} {host.LastName}, Id: {host.Id}, {host.Type}, phone: {host.Phone}");
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
    }

    public async Task ShowMenuAsync()
    {
        while (_appState != MenuList.End)
        {
            DefaultState();
            ReadUserInput();
            UpdateState(_command);
        }
    }
    
    private void RemoveHostOperations()
    {
        Console.Write("Input the id of host you want to remove: ");
        var id = Console.ReadLine();
        bool valid = Common.ChoiceNumberIsValid(id);
        
        if (!valid)
        {
            Console.WriteLine("Invalid id. Please try again.");
            return;
        }
        
        int numId = int.Parse(id);
        if (_hostService.RemoveHost(numId))
            Console.WriteLine("Host removed");
        else
            Console.WriteLine("Host not found");
        
        _appState = MenuList.Default;
    }
    
    private void AddHostOperations()
    {
        Console.WriteLine("Add the host and add the host operations");
    }

    private void UpdateHostOperations(string command)
    {
        Console.WriteLine("Update the host and update the host operations");
    }
    
    private void SaveChanges()
    {
        Console.WriteLine("Save changes");
    }

    private void ShowHostDetails(string command)
    {
        Console.WriteLine();
        
        command = command.Trim();
        command = command.ToLower();

        HostDetailsDTO hostDetails;
        if (Common.ChoiceNumberIsValid(command))
            hostDetails = _hostService.GetHost(int.Parse(command));
        else
        {
            hostDetails = _hostService.GetHost(command);
        }

        if (hostDetails == null)
        {
            Console.WriteLine("Haven't found the host. Try again");
            return;
        }
    
        var apartList = _apartmentService.GetApartmentsOfHost(hostDetails.Id);
        if (apartList.Count <= 0)
        {
            Console.WriteLine("There are no apartments for this host. ");
        }
        else
        {
            Console.WriteLine("Here is a List of Apartments belonging to " + hostDetails.FirstName + " " +
                              hostDetails.LastName);
            foreach (var apartment in apartList)
            {
                Console.WriteLine($"{apartment.Name}, {apartment.Type}, Price - {apartment.PricePerNight}, Rating -  {apartment.Rating}");
            }
        }

        Console.WriteLine("Type \"Update Host\" if you want to update the host");
        Console.WriteLine("Type Back to see the list of hosts");
        string choice = Console.ReadLine().Trim().ToLower();
        switch (choice)
        {
            case "update host":
                _appState = MenuList.HostUpdate;
                UpdateHostOperations(command);
                break;
            default:
                _appState = MenuList.Default;
                break;
        }
    }
}