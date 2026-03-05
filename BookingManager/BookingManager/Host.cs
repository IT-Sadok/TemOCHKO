using System.Text;
using BookingManager.Common.Enums;
using BookingManager.DBModels;
using BookingManager.Services;

namespace BookingManager;

public class Host
{
    private HostDBModel _hostDbModel;
    private List<ApartmentDBModel> _apartmentDbModels;
    private int? _id;
    private string _firstName;
    private string _lastName;
    private HostType _type;
    private string _email;
    private DateTime _dateOfBirth;
    private string _phoneNumber;

    public int Id
    {
        get => _hostDbModel.Id;
    }

    public string FirstName
    {
        get => _firstName;
        set => _firstName = value;
    }

    public string LastName
    {
        get => _lastName;
        set => _lastName = value;
    }

    public HostType Type
    {
        get => _type;
        set => _type = value;
    }

    public string Email
    {
        get => _email;
        set => _email = value;
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set => _phoneNumber = value;
    }

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set => _dateOfBirth = value;
    }

    public List<ApartmentDBModel> ApartmentDbModels
    {
        get => _apartmentDbModels;
    }

    public double AverageRating
    {
        get => CalculateAverageRating();
    }

    public Host()
    {
        _apartmentDbModels = new List<ApartmentDBModel>();
    }

    public Host(HostDBModel hostDbModel) : this()
    {
        _hostDbModel = hostDbModel;
        _id = hostDbModel.Id;
        _firstName = hostDbModel.FirstName;
        _lastName = hostDbModel.LastName;
        _type = hostDbModel.Type;
        _email = hostDbModel.Email;
        _phoneNumber = hostDbModel.Phone;
        _dateOfBirth = hostDbModel.DateOfBirth;
    }

    private double CalculateAverageRating()
    {
        if (_apartmentDbModels.Count == 0)
            return 0;
        
        double sum = 0;
        foreach (ApartmentDBModel apartment in _apartmentDbModels)
        {
            sum += apartment.Rating;
        }
        return sum / _apartmentDbModels.Count;
    }

    public void LoadApartments(StorageService storageService)
    {
        if (_id == null || ApartmentDbModels.Count > 0) return;
        foreach (ApartmentDBModel apartment in storageService.GetApartmentsOfHost(_hostDbModel.Id))
        {
            _apartmentDbModels.Add(apartment);
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Host: ").Append(_firstName).Append(" ").Append(_lastName)
            .Append(", ID: ").Append(_id).Append(", ").Append(_type)
            .Append(", Email: ").Append(_email).Append(", Phone: ").Append(_phoneNumber)
            .Append(", Birth Date: ").Append(_dateOfBirth).Append("Average Rating: ")
            .Append(AverageRating);
        return sb.ToString();
    }
}