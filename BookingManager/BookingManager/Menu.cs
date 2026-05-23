using System.ComponentModel.DataAnnotations;
using Models;
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
    
    private void UpdateState(string command)
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
        Console.WriteLine("Type \"Exit\" to exit the program");
    }

    private void ExitProgram()
    {
        Environment.Exit(0);
    }

    private void ReadUserInput()
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

    public void ShowMenu()
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
        var hostCreateDto = PromptToCreateHost();

        try
        {
            _hostService.AddHost(hostCreateDto);
            Console.WriteLine("Host added");
        }
        catch (ValidationException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Failed to add host. Try again");
        }
        finally
        {
            Console.WriteLine();
            _appState = MenuList.Default;
        }
    }

    private void UpdateHostOperations(string command)
    {
        var hostUpdateDto = PromptToUpdateHost(command);

        if (hostUpdateDto == null)
        {
            Console.WriteLine();
            _appState = MenuList.Default;
            return;
        }
        
        try
        {
            _hostService.UpdateHost(hostUpdateDto);
            Console.WriteLine("Host updated");
        }
        catch (ValidationException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Failed to update host. Try again");
        }
        finally
        {
            Console.WriteLine();
            _appState = MenuList.Default;
        }
    }
    
    private void SaveChanges()
    {
        _hostService.SaveHosts();
        _apartmentService.SaveApartments();
        Console.WriteLine("\nChanges saved\n");
        
        _appState = MenuList.Default;
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
            _appState = MenuList.Default;
            return;
        }
        
        Console.WriteLine($"Name: {hostDetails.FirstName}, Surname: {hostDetails.LastName}, {hostDetails.Type},\nEmail: {hostDetails.Email}, Phone: {hostDetails.Phone}, Date of birth: {hostDetails.DateOfBirth}");
        Console.WriteLine();
        
        var apartList = _apartmentService.GetApartmentsOfHost(hostDetails.Id);
        if (apartList.Count <= 0)
        {
            Console.WriteLine("There are no apartments for this host. ");
        }
        else
        {
            Console.WriteLine("Here is a List of Apartments belonging to " + hostDetails.FirstName + " " + hostDetails.LastName);
            foreach (var apartment in apartList)
                Console.WriteLine($"{apartment.Name}, {apartment.Type}, Price - {apartment.PricePerNight}, Rating -  {apartment.Rating}");
            Console.WriteLine();
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
    
    // Prompt the user to create host, and returns Host Create entity
    private HostCreateDTO PromptToCreateHost()
    {
        Console.WriteLine("Menu For Creating A Host: ");
        Console.WriteLine("First Name: ");
        string firstName = Console.ReadLine();
        Console.WriteLine("Last Name: ");
        string lastName = Console.ReadLine();
        HostType hostType = Common.PromptUserForHostTypeInConsole();
        Console.WriteLine("Email: ");
        string email = Console.ReadLine();
        Console.WriteLine("Phone: ");
        string phone = Console.ReadLine();
        DateTime dateOfBirth = Common.PromptUserForDateInConsole("Enter host's date of birth: ");
        
        return new HostCreateDTO(firstName, lastName, hostType, email, phone, dateOfBirth);
    }

    private HostDetailsDTO PromptToUpdateHost(string command)
    {
        Console.WriteLine();
        
        command = command.Trim();
        command = command.ToLower();

        HostDetailsDTO hostToUpdate;
        if (Common.ChoiceNumberIsValid(command))
            hostToUpdate = _hostService.GetHost(int.Parse(command));
        else
        {
            hostToUpdate = _hostService.GetHost(command);
        }
        
        if (hostToUpdate == null)
        {
            Console.WriteLine("Host not found.");
            return hostToUpdate;
        }
        
        string firstName =  hostToUpdate.FirstName;
        string lastName =  hostToUpdate.LastName;
        string email =  hostToUpdate.Email;
        string phone = hostToUpdate.Phone;
        HostType type = hostToUpdate.Type;
        DateTime dateOfBirth = hostToUpdate.DateOfBirth;
        
        Console.WriteLine("Input the name of property of the host you want to change: ");
        Console.Write("| ");
        foreach (var prop in typeof(HostDetailsDTO).GetProperties())
        {
            if (prop.Name != "Id")
                Console.Write($"{Common.GetDisplayName(typeof(HostDetailsDTO), prop.Name)} | ");
        }
        var property = Console.ReadLine();
        property = property.ToLower();
        property = property.Trim();
        switch (property)
        {
            case "first name":
                Console.WriteLine("Enter new name: ");
                firstName = Console.ReadLine();
                break;
            case "last name":
                Console.WriteLine("Enter new surname: ");
                lastName = Console.ReadLine();
                break;
            case "email":
                Console.Write("Enter new email: ");
                email = Console.ReadLine();
                break;
            case "phone":
                Console.WriteLine("Enter new phone: ");
                phone = Console.ReadLine();
                break;
            case "date of birth":
                dateOfBirth = Common.PromptUserForDateInConsole("Enter new host's date of birth: ");
                break;
            case "type":
                type = Common.PromptUserForHostTypeInConsole();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                return null;
        }
        
        return new HostDetailsDTO(hostToUpdate.Id, firstName, lastName, type, email, phone, dateOfBirth);
    }
}