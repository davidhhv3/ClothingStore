using ClothingStore.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
      
        public CountryController()
        {
          
        }
        [HttpGet]
        public IActionResult GetCountry()
        {
            return Ok();
        }

    }
}
