using AutoMapper;
using ClothingStore.Core.DTOs;
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
        private readonly IMapper _mapper;

        public CountryController(IContryService contryService, IMapper mapper)
        {
            _contryService = contryService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetCountries()
        {
            var countries = _contryService.GetCountries();
            var countriesDtos = _mapper.Map<IEnumerable<CountryDto>>(countries);
            return Ok(countriesDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            var country = await _contryService.GetCountry(id);
            var countryDto = _mapper.Map<CountryDto>(country);
            return Ok(countryDto);
        }
        [HttpPost]
        public async Task<IActionResult> Post(CountryDto countryDto)
        {
            var country = _mapper.Map<Country>(countryDto);
            await _contryService.InsertCountry(country);
            countryDto = _mapper.Map<CountryDto>(country);
            return Ok(countryDto);
        }
        [HttpPut]
        public async Task<IActionResult> Put(int id, CountryDto countryDto)
        {
            var country = _mapper.Map<Country>(countryDto);
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

