using HttpClient.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        public async Task<ActionResult<AlbumModel>> Get(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://jsonplaceholder.typicode.com/albums/{id}");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<AlbumModel>(responseStream);
            }
            else
            {
                return NotFound();
            }

        }

        // POST api/<BasicController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AlbumModel value)
        {
            var albumContent = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");

            using var response = await _clientFactory.CreateClient().PostAsync("https://jsonplaceholder.typicode.com/albums", albumContent);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<AlbumModel>(responseStream);
                return Ok (result);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT api/<BasicController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] AlbumModel value)
        {
            var albumContent = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");

            using var response = await _clientFactory.CreateClient().PutAsync($"https://jsonplaceholder.typicode.com/albums/{id}", albumContent);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<AlbumModel>(responseStream);
                return Ok(result);
            }
            else
            {
                return new StatusCodeResult((int)response.StatusCode);
            }
        }

        // DELETE api/<BasicController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"https://jsonplaceholder.typicode.com/albums/{id}");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return new StatusCodeResult((int)response.StatusCode);
            }
        }
    }
}
