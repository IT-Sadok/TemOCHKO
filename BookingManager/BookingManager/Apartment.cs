using System.Text;
using BookingManager.Common.Enums;
using BookingManager.DBModels;

namespace BookingManager;

public class Apartment
{
    private ApartmentDBModel _apartmentDBModel;
    private int _id;
    private int _hostId;
    private string _name;
    private ApartmentType _type;
    private double _squareMeters;
    private decimal _pricePerNight;
    private int _minimumStay;
    private double _rating;

    public int Id
    {
        get { return _apartmentDBModel.Id; }
    }

    public int HostId
    {
        get { return _apartmentDBModel.HostId; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public ApartmentType Type
    {
        get { return _type; }
        set { _type = value; }
    }

    public double SquareMeters
    {
        get { return _squareMeters; }
        set { _squareMeters = value; }
    }

    public decimal PricePerNight
    {
        get { return _pricePerNight; }
        set { _pricePerNight = value; }
    }

    public int MinimumStay
    {
        get { return _minimumStay; }
        set { _minimumStay = value; }
    }

    public double Rating
    {
        get { return _rating; }
        set { _rating = value; }
    }
    
    private Apartment()
    {
        
    }

    public Apartment(ApartmentDBModel apartmentDbModel)
    {
        _apartmentDBModel = apartmentDbModel;
        _id = apartmentDbModel.Id;
        _hostId = apartmentDbModel.HostId;
        _name = apartmentDbModel.Name;
        _type = apartmentDbModel.Type;
        _squareMeters = apartmentDbModel.SquareMeters;
        _pricePerNight = apartmentDbModel.PricePerNight;
        _minimumStay =  apartmentDbModel.MinimumStay;
        _rating = apartmentDbModel.Rating;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Apartment: ").Append(_name).Append(", ID: ")
            .Append(_id).Append(", Type: ").Append(_type)
            .Append(", Square Meters: ").Append(_squareMeters)
            .Append(", Price: ").Append(_pricePerNight)
            .Append(", Min Stay: ").Append(_minimumStay)
            .Append(", Rating: ").Append(Rating);
        return sb.ToString();
    }
}