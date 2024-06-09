using Microsoft.AspNetCore.Mvc;
using PokemonApp.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PokemonApp.Controllers
{
    public class PokemonController : Controller
    {
        private readonly ILogger<PokemonController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public PokemonController(IHttpClientFactory httpClientFactory, ILogger<PokemonController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        [Route("pokemon/{id}")]
        public async Task<IActionResult> Pokemon(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var pokemon = JsonConvert.DeserializeObject<DetailedPokemon>(json);

                return View(pokemon);
            }

            return StatusCode((int)response.StatusCode);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction("Index", "Home");
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var pokemon = JsonConvert.DeserializeObject<Pokemon>(json);

                return RedirectToAction("Pokemon", new { id = pokemon.Id }); // Use ID instead of Name
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
