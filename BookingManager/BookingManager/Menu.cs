using System.ComponentModel.DataAnnotations;
using Models;
using Models.DTOs;
using Repositories;
using Services;
using Services.Apartment;
using Services.Host;
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
                break;
            case "remove host":
                _appState = MenuList.HostRemove;
                await RemoveHostOperationsAsync();
                break;
            case "add host":
                _appState = MenuList.HostAdd;
                await AddHostOperations();
                break;
            case "save changes":
                _appState = MenuList.SaveChanges;
                await SaveChangesAsync();
                break;
            case "race condition":
                _appState = MenuList.RaceCondition;
                await ShowcaseMultiThreadExample();
                break;
            default:
                switch (_appState)
                {
                    case MenuList.Default:
                        _appState =  MenuList.HostDetails;
                        await ShowHostDetailsAsync(command);
                        break;
                    default:
                        Console.WriteLine("Unknown command. Please try again.");
                        _appState = MenuList.Default;
                        await DefaultState();
                        break;
                }
                break;
        }
    }

    private async Task DefaultState()
    {
        Console.WriteLine("Here is the list of all hosts: ");
        foreach (var host in await _hostService.GetHostsListAsync())
        {
            Console.WriteLine($"Host: {host.FirstName} {host.LastName}, Id: {host.Id}, {host.Type}, phone: {host.Phone}");
        }
        Console.WriteLine();
        Console.WriteLine("Type the name and surname of the host / ID of the host to open his menu");
        Console.WriteLine("Type \"Remove Host\" to open the menu for removing the host");
        Console.WriteLine("Type \"Add Host\" to open the menu for removing the host");
        Console.WriteLine("Type \"Save Changes\" to save changes into the file");
        Console.WriteLine("Type \"Race condition\" to see and emulation of multithread race condition");
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

    public async Task ShowMenuAsync()
    {
        while (_appState != MenuList.End)
        {
            await DefaultState();
            ReadUserInput();
            await UpdateState(_command);
        }
    }
    
    private async Task RemoveHostOperationsAsync()
    {
        Console.Write("Input the id of host you want to remove: ");
        var id = Console.ReadLine();
        bool valid = Common.ChoiceNumberIsValid(id);
        
        if (!valid)
        {
            Console.WriteLine("Invalid id. Please try again.");
            return;
        }
        
        var success = int.TryParse(id, out int numId);
        if (await _hostService.RemoveHostAsync(numId))
            Console.WriteLine("Host removed");
        else
            Console.WriteLine("Host not found");
        
        _appState = MenuList.Default;
    }
    
    private async Task AddHostOperations()
    {
        var hostCreateDto = PromptToCreateHost();

        try
        {
            await _hostService.AddHostAsync(hostCreateDto);
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

    private async Task UpdateHostOperationsAsync(string command)
    {
        var hostUpdateDto = await PromptToUpdateHostAsync(command);

        if (hostUpdateDto == null)
        {
            Console.WriteLine();
            _appState = MenuList.Default;
            return;
        }
        
        try
        {
            await _hostService.UpdateHostAsync(hostUpdateDto);
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
    
    private async Task SaveChangesAsync()
    {
        await _hostService.SaveHostsAsync();
        await _apartmentService.SaveApartmentsAsync();
        Console.WriteLine("\nChanges saved\n");
        
        _appState = MenuList.Default;
    }

    private async Task ShowHostDetailsAsync(string command)
    {
        Console.WriteLine();
        
        command = command.Trim();
        command = command.ToLower();

        HostDetailsDTO hostDetails;
        if (Common.ChoiceNumberIsValid(command))
        {
            int.TryParse(command, out int hostId);
            hostDetails = await _hostService.GetHostAsync(hostId);
        }
        else
        {
            hostDetails = await _hostService.GetHostAsync(command);
        }

        if (hostDetails == null)
        {
            Console.WriteLine("Haven't found the host. Try again");
            _appState = MenuList.Default;
            return;
        }
        
        Console.WriteLine($"Name: {hostDetails.FirstName}, Surname: {hostDetails.LastName}, {hostDetails.Type},\nEmail: {hostDetails.Email}, Phone: {hostDetails.Phone}, Date of birth: {hostDetails.DateOfBirth}");
        Console.WriteLine();
        
        var apartList = await _apartmentService.GetApartmentsOfHostAsync(hostDetails.Id);
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
                await UpdateHostOperationsAsync(command);
                break;
            default:
                _appState = MenuList.Default;
                break;
        }
    }
    
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
        
        return new HostCreateDTO
        {
            FirstName = firstName,
            LastName = lastName,
            Type = hostType,
            Email = email,
            Phone = phone,
            DateOfBirth = dateOfBirth
        };
    }

    private async Task<HostDetailsDTO> PromptToUpdateHostAsync(string command)
    {
        Console.WriteLine();
        
        command = command.Trim();
        command = command.ToLower();

        HostDetailsDTO hostToUpdate;
        if (Common.ChoiceNumberIsValid(command))
        {
            int.TryParse(command, out int hostId);
            hostToUpdate = await _hostService.GetHostAsync(hostId);
        }
        else
        {
            hostToUpdate = await _hostService.GetHostAsync(command);
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
        
        return new HostDetailsDTO
        {
            Id = hostToUpdate.Id,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
            Type = type,
            DateOfBirth = dateOfBirth
        };
    }

    private async Task ShowcaseMultiThreadExample()
    {
        var startingApartmentPrice = 67;
        Console.WriteLine($"\nCreating a new apartment with a price of {startingApartmentPrice}");
        var apartment = new ApartmentListItemDTO
        {
            Id = -1,
            HostId = -1,
            Name = "New Apartment",
            Type = ApartmentType.EntireApartment,
            PricePerNight = startingApartmentPrice,
            Rating = 4.3
        };
        
        object lockObject = new object();
        int iterationCount = 10;
        
        for (int i = 0; i < iterationCount; i++)
        {
            Console.WriteLine($"Iteration {i+1}");
            Console.WriteLine($"Apartment Price at the start: {apartment.PricePerNight}");
            Task hostTask1 = Task.Run(() =>
            {
                //lock (lockObject)
                //{
                    var currentPrice = apartment.PricePerNight;
                    apartment.PricePerNight = currentPrice + 10;
                    Console.WriteLine("Host 1 added 10 to the price, now price is " + apartment.PricePerNight);
                //}
            });

            Task hostTask2 = Task.Run((() =>
            {
                //lock (lockObject)
                //{
                    var currentPrice = apartment.PricePerNight;
                    apartment.PricePerNight = currentPrice - 5;
                    Console.WriteLine("Host 2 subtracted 5 from the price, now price is " + apartment.PricePerNight);
                //}
            }));
            
            await Task.WhenAll(hostTask1, hostTask2);
            Console.WriteLine($"Price of apartments at the end - {apartment.PricePerNight}");
            apartment.PricePerNight = startingApartmentPrice;
            Console.WriteLine();
            Thread.Sleep(100);
        }

        _appState = MenuList.Default;
    }
}