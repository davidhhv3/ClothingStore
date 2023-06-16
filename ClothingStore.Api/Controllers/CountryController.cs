using AutoMapper;
using ClothingStore.Api.Responses;
using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.DTOs;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using ClothingStore.Core.QueryFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace ClothingStore.Api.Controllers
{
    //[Authorize]
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
        /// <summary>
        /// Retrieve all posts
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetCountries))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<CountryDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetCountries([FromQuery] CountryQueryFilter filters)
        {
            var countries = _contryService.GetCountries(filters);
            var countriesDtos = _mapper.Map<IEnumerable<CountryDto>>(countries);

            var metadata = new Metadata
            {
                TotalCount = countries.TotalCount,
                PageSize = countries.PageSize,
                CurrentPage = countries.CurrentPage,
                TotalPages = countries.TotalPages,
                HasNextPage = countries.HasNextPage,
                HasPreviousPage = countries.HasPreviousPage,
            };
            var response = new ApiResponse<IEnumerable<CountryDto>>(countriesDtos)
            {
                Meta = metadata
            };
            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            Country country = await _contryService.GetCountry(id);
            CountryDto countryDto = _mapper.Map<CountryDto>(country);
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

