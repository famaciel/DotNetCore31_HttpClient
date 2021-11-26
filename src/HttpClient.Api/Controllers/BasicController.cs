using HttpClient.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HttpClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        public IEnumerable<AlbumModel> Albuns { get; private set; }

        public BasicController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }


        // GET: api/<BasicController>
        [HttpGet]
        public async Task<IEnumerable<AlbumModel>> Get()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://jsonplaceholder.typicode.com/albums");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                Albuns = await JsonSerializer.DeserializeAsync<IEnumerable<AlbumModel>>(responseStream);
            }
            else
            {
                Albuns = Array.Empty<AlbumModel>();
            }

            return Albuns;
        }

        // GET api/<BasicController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BasicController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BasicController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BasicController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
