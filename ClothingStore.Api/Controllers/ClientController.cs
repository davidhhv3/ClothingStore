using AutoMapper;
using ClothingStore.Api.Responses;
using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.DTOs;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using ClothingStore.Core.QueryFilters;
using ClothingStore.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClothingStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieve all clients
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns></returns>
        [HttpGet("GetClients")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<ClientDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]   
        public async Task<IActionResult> GetClients([FromQuery] ClientQueryFilter filters)
        {
            PagedList<Client> clients = await _clientService.GetClients(filters);
            IEnumerable<ClientDto> clientsDtos = _mapper.Map<IEnumerable<ClientDto>>(clients);
            var metadata = new Metadata
            {
                TotalCount = clients.TotalCount,
                PageSize = clients.PageSize,
                CurrentPage = clients.CurrentPage,
                TotalPages = clients.TotalPages,
                HasNextPage = clients.HasNextPage,
                HasPreviousPage = clients.HasPreviousPage,
            };
            ApiResponse<IEnumerable<ClientDto>> response = new ApiResponse<IEnumerable<ClientDto>>(clientsDtos)
            {
                Meta = metadata
            };
            return Ok(response);
        }

        /// <summary>
        /// Retrieve client
        /// </summary>
        /// <param name="id">The ID of the client to retrieve</param>
        /// <returns></returns>
        [HttpGet("GetClient/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<ClientDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetClient(int id)
        {
            Client? client = await _clientService.GetClient(id);
            ClientDto clientDto = _mapper.Map<ClientDto>(client);
            ApiResponse<ClientDto> response = new ApiResponse<ClientDto>(clientDto);
            return Ok(response);
        }

        /// <summary>
        /// Create a new client
        /// </summary>
        /// <param name="client">Client data</param>
        /// <returns></returns>
        [HttpPost("CreateClient")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<ClientDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateClient(Client client)
        {         
            await _clientService.InsertCLient(client);
            ClientDto clientDto = _mapper.Map<ClientDto>(client);
            ApiResponse<ClientDto> response = new ApiResponse<ClientDto>(clientDto);
            return Ok(response);
        }

        /// <summary>
        /// Update a client
        /// </summary>   
        /// <param name="id">The ID of the client to update</param>
        /// <param name="clientDto">Updated client data</param>
        /// <returns></returns>
        [HttpPut("UpdateClient")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateClient(int id, ClientDto clientDto)
        {
            Client client = _mapper.Map<Client>(clientDto);
            client.Id = id;
            bool result = await _clientService.UpdateClient(client);
            ApiResponse<bool> response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Delete a client by ID
        /// </summary>    
        /// <param name="id">The ID of the client to delete</param>
        /// <returns></returns>
        [HttpDelete("DeleteClient/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteClient(int id)
        {
            bool result = await _clientService.DeleteClient(id);
            ApiResponse<bool> response = new ApiResponse<bool>(result);
            return Ok(response);
        }
    }
}
