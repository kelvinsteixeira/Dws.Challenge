namespace Dws.Challenge.Infrastructure.Gateways.Interfaces
{
    public interface IGatewayClient
    {
        T GetAsync<T>(string resource);
    }
}