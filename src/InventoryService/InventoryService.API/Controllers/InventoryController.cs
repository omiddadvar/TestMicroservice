using InventoryService.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryAppService _service;
        public InventoryController(InventoryAppService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
                Ok(await _service.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Add(string name, int quantity)
        {
            await _service.AddItemAsync(name, quantity);
            return Ok();
        }

        //[HttpPut("{id}/decrease")]
        //public async Task<IActionResult> Decrease(IList<>)
        //{
        //    var success = await _service.DecreaseInventoryAsync(id, amount);
        //    return success ? Ok() : NotFound();
        //}
    }
}
