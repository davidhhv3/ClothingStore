using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IContryService _contryService;
        public CountryController(IContryService contryService)
        {
            _contryService = contryService;
        }

        [HttpGet]
        public IActionResult GetCountries()
        {
            var countries = _contryService.GetCountries();
            return Ok(countries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            var country = await _contryService.GetCountry(id);
            return Ok(country);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Country country)
        {

            await _contryService.InsertCountry(country);
            return Ok(country);
        }
        [HttpPut]
        public async Task<IActionResult> Put(int id, Country country)
        {
            country.Id = id;
            var result = await _contryService.UpdateCountry(country);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _contryService.DeleteCountry(id);
            return Ok(result);
        }
    }
}

