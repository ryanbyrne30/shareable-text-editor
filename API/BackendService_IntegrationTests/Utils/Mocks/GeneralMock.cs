namespace BackendService_IntegrationTests.Utils.Mocks;

public static class GeneralMock
{
    private static readonly Random Random = new Random();
    
    public static int GenerateInt(int min=1, int max=int.MaxValue)
    {
        return Random.Next(min, max);
    }
}