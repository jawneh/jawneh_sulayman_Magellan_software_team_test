using MagellanTest.Models;
using MagellanTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace MagellanTest.Controllers
{
    [ApiController]
    [Route("api/items")]
    // [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        /** <summary>
        * Gets an item by its item name.
        * </summary>
        * <param name="itemName">The ID of the item to retrieve.</param>
        * <returns>The item with the specified ID, or null if no item was found.</returns>
        */
        [HttpGet("totalCost/{itemName}")]
        public async Task<IActionResult> GetItemTotalCost(string itemName)
        {
            try
            {
                var item = await _itemService.GetItemTotalCost(itemName);

                return Ok(item);
            }
            catch (ItemNotFoundException ex)
            {
                // Log the exception or handle it in a way that makes sense for your application
                // Console.WriteLine($"Error: {ex.Message}");
                return NotFound($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other exceptions if needed...
                // return StatusCode(500, "Internal server error");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /** <summary>
        * Gets an item by its ID.
        * </summary>
        * <param name="id">The ID of the item to retrieve.</param>
        * <returns>The item with the specified ID, or null if no item was found.</returns>
        */
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            try
            {
                var item = await _itemService.GetItemById(id);

                if (item == null)
                {
                    return NotFound();
                }

                return Ok(item);
            }
            catch (ItemNotFoundException ex)
            {
                // Log the exception or handle it in a way that makes sense for your application
                // Console.WriteLine($"Error: {ex.Message}");
                return NotFound($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other exceptions if needed...
                // return StatusCode(500, "Internal server error");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("newItem")]
        public async Task<IActionResult> CreateItem([FromBody] ItemsModel newItemModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var createdItem = await _itemService.CreateItem(newItemModel);
                    return Ok(createdItem);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
    }
}
