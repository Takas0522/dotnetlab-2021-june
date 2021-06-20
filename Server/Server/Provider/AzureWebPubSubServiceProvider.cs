using Azure;
using Azure.Messaging.WebPubSub;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Server.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Provider
{
    public class AzureWebPubSubServiceProvider : IAzureWebPubSubServiceProvider
    {
        private readonly WebPubSubServiceClient _client;
        private readonly string _dummyUri;

        public AzureWebPubSubServiceProvider(IOptions<AzureWebPubSubSetting> settings)
        {
            string pubsubEndpoint = settings.Value.Endpoint;
            string pubsubHub = settings.Value.Hub;
            string pubsubAccessKey = settings.Value.AccessKey;
            _dummyUri = settings.Value.DummyUri;
            _client = new WebPubSubServiceClient(new Uri(pubsubEndpoint), pubsubHub, new AzureKeyCredential(pubsubAccessKey));
        }

        public string GetWebSocketUri()
        {
            var reqRoles = new List<string> { "webpubsub.joinLeaveGroup", "webpubsub.sendToGroup" };
            Uri res = _client.GetClientAccessUri(roles: reqRoles.ToArray());
            //return res.AbsoluteUri;
            return _dummyUri;
        }

        public async Task SendToAllMessage(string message)
        {
            await _client.SendToAllAsync(message);
        }

        public async Task SendToGroupMessage(string message, string group)
        {
            await _client.SendToGroupAsync(group, message);
        }


    }
}
