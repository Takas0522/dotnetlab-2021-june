using System.Threading.Tasks;

namespace Server.Provider
{
    public interface IAzureWebPubSubServiceProvider
    {
        string GetWebSocketUri();
        Task SendToAllMessage(string message);
        Task SendToGroupMessage(string message, string group);
    }
}