using HttpClient.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NamedClient : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public NamedClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<IEnumerable<BranchModel>> Get()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "repos/famaciel/LogSample/branches");

            var client = _clientFactory.CreateClient("github");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<BranchModel>>(responseStream);
            }
            else
            {
                return Array.Empty<BranchModel>();
            }
        }
    }
}
