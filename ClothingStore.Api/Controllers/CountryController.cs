using AutoMapper;
using ClothingStore.Api.Responses;
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
            var response = new ApiResponse<IEnumerable<CountryDto>>(countriesDtos);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            var country = await _contryService.GetCountry(id);
            var countryDto = _mapper.Map<CountryDto>(country);
            var response = new ApiResponse<CountryDto>(countryDto);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Post(CountryDto countryDto)
        {
            var country = _mapper.Map<Country>(countryDto);
            await _contryService.InsertCountry(country);
            countryDto = _mapper.Map<CountryDto>(country);
            var response = new ApiResponse<CountryDto>(countryDto);
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> Put(int id, CountryDto countryDto)
        {
            var country = _mapper.Map<Country>(countryDto);
            country.Id = id;
            var result = await _contryService.UpdateCountry(country);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _contryService.DeleteCountry(id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

    }

}

