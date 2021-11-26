using HttpClient.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Album : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public Album(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlbumModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"albums/{id}");

            var client = _clientFactory.CreateClient("jsonplaceholder");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var album = await JsonSerializer.DeserializeAsync<AlbumModel>(responseStream);

                return Ok(album);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AlbumModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] AlbumModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var albumContent = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");

            using var response = await _clientFactory.CreateClient("jsonplaceholder").PostAsync("albums", albumContent);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<AlbumModel>(responseStream);

                return CreatedAtAction(nameof(Get), new { id = result.id }, result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
