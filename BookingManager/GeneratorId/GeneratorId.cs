namespace IdGenerator;

public class GeneratorId : IGeneratorId
{
    private static int _hostInstanceCounter = 0;
    private static int _apartmentInstanceCounter = -1000;
    
    public int GenerateHostId()
    {
        return ++_hostInstanceCounter;
    }

    public int GenerateApartmentId()
    {
        return  ++_apartmentInstanceCounter;
    }
}