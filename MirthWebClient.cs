using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BABsConsoleTemplate
{
    public class MirthWebClient
    {
        public HttpClient Client;
        public MirthWebClient(HttpClient httpClient)
        {
            Client = httpClient;
        }

        public  async Task<string> ChannelGroups()
        {
            var results = await Client.GetStringAsync(new Uri(Client.BaseAddress, @"api/channelgroups"));

            return results;
        }

        public async Task<string> ChannelGroup(string channelGroupId)
        {
            var results = await Client.GetStringAsync(new Uri(Client.BaseAddress, $@"api/channelgroups?channelGroupId={channelGroupId}"));

            return results;
        }

        public async Task<string> Channels()
        {
            var results = await Client.GetStringAsync(new Uri(Client.BaseAddress, @"/api/channels"));

            return results;
        }
    }
}
