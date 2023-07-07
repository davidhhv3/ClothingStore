using AutoMapper;
using ClothingStore.Api.Responses;
using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.DTOs;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using ClothingStore.Core.QueryFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClothingStore.Api.Controllers
{
    [Authorize]
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
        /// Retrieve all countries
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns></returns>
        [HttpGet("GetCountries")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<CountryDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCountries([FromQuery] CountryQueryFilter filters)
        {
            var countries = await _contryService.GetCountries(filters);
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
            ApiResponse<IEnumerable<CountryDto>> response = new ApiResponse<IEnumerable<CountryDto>>(countriesDtos)
            {
                Meta = metadata
            };           
            return Ok(response);
        }

        /// <summary>
        /// Retrieve country
        /// </summary>
        /// <param name="id">The ID of the country to retrieve</param>
        /// <returns></returns>
        [HttpGet("GetCountry/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<CountryDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCountry(int id)
        {
            Country? country = await _contryService.GetCountry(id);
            CountryDto countryDto = _mapper.Map<CountryDto>(country);
            ApiResponse<CountryDto> response = new ApiResponse<CountryDto>(countryDto);
            return Ok(response);          
        }

        /// <summary>
        /// Create a new country
        /// </summary>
        /// <param name="countryDto">Country data</param>
        /// <returns></returns>
        [HttpPost("CreateCountry")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<CountryDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateCountry(CountryDto countryDto)
        {
            Country country = _mapper.Map<Country>(countryDto);
            await _contryService.InsertCountry(country);
            countryDto = _mapper.Map<CountryDto>(country);
            ApiResponse<CountryDto> response = new ApiResponse<CountryDto>(countryDto);
            return Ok(response);
        }

        /// <summary>
        /// Update a country
        /// </summary>   
        /// <param name="id">The ID of the country to update</param>
        /// <param name="countryDto">Updated country data</param>
        /// <returns></returns>
        [HttpPut("UpdateCountry")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateCountry(int id, CountryDto countryDto)
        {
            Country country = _mapper.Map<Country>(countryDto);
            country.Id = id;
            bool result = await _contryService.UpdateCountry(country);
            ApiResponse<bool> response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Delete a country by ID
        /// </summary>    
        /// <param name="id">The ID of the country to delete</param>
        /// <returns></returns>
        [HttpDelete("DeleteCountry/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            bool result = await _contryService.DeleteCountry(id);
            ApiResponse<bool> response = new ApiResponse<bool>(result);           
            return Ok(response);            
        }
    }
}

