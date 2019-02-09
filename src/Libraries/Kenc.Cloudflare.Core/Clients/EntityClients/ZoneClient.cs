namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;

    public class ZoneClient : IZoneClient
    {
        private readonly IRestClient restClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneClient"/>
        /// </summary>
        /// <param name="restClient">Client to use to send requests.</param>
        public ZoneClient(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public Task<Zone> CreateZoneAsync(string name, Account account)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteZoneAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<Zone> GetZoneAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Zone>> GetZonesAsync(string name = null, string status = null, int? page = null, int? perPage = null, string order = null, string direction = null, string match = null)
        {
            throw new NotImplementedException();
        }

        public Task<string> InitiateZoneActivationCheckAsync(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<Zone> PatchZoneAsync(string identifier, bool? paused = null, IList<string> vanityNameServers = null, string planId = null)
        {
            throw new NotImplementedException();
        }
    }
}
