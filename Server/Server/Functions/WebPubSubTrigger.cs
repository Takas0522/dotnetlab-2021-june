using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.WebPubSub;

namespace Server.Functions
{
    public static class WebPubSubTrigger
    {
        [FunctionName("login")]
        public static WebPubSubConnection GetClientConnection(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [WebPubSubConnection(UserId = "{query.userid}")] WebPubSubConnection connection,
            ILogger log
        )
        {
            log.LogInformation("Login");
            return connection;
        }

        [FunctionName("connect")]
        public static ServiceResponse Connect(
            [WebPubSubTrigger("SampleHub", WebPubSubEventType.System, "connect")] ConnectionContext connectionContext,
            ILogger log
        )
        {
            log.LogInformation($"Received client connect with connectionId: {connectionContext.ConnectionId}");
            return new ConnectResponse
            {
                UserId = connectionContext.UserId
            };
        }

        [FunctionName("connected")]
        public static async Task Connected(
            [WebPubSubTrigger(WebPubSubEventType.System, "connected")] ConnectionContext connectionContext,
            [WebPubSub] IAsyncCollector<WebPubSubOperation> operations
        )
        {
            await operations.AddAsync(new AddUserToGroup
            {
                UserId = connectionContext.UserId,
                Group = "group1"
            });
            await operations.AddAsync(new SendToUser
            {
                UserId = connectionContext.UserId,
                Message = BinaryData.FromString(new ClientContent($"{connectionContext.UserId} joined group: group1.").ToString()),
                DataType = MessageDataType.Json
            });
        }

        [FunctionName("broadcast")]
        public static async Task<MessageResponse> Broadcast(
            [WebPubSubTrigger(WebPubSubEventType.User, "message")] BinaryData message,
            [WebPubSub(Hub = "SampleHub")] IAsyncCollector<WebPubSubOperation> operations
        )
        {
            await operations.AddAsync(new SendToAll
            {
                Message = message,
                DataType = MessageDataType.Json
            });
            return new MessageResponse
            {
                Message = BinaryData.FromString(new ClientContent("ack").ToString()),
                DataType = MessageDataType.Json
            };
        }
    }

    [JsonObject]
    public sealed class ClientContent
    {
        [JsonProperty("from")]
        public string From { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }

        public ClientContent(string message)
        {
            From = "[System]";
            Content = message;
        }

        public ClientContent(string from, string message)
        {
            From = from;
            Content = message;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
