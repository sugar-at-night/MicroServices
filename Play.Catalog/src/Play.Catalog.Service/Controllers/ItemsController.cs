using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        // Create a new instance of the ItemsRepository class to handle database interactions
        private readonly ItemsRepository itemsRepository = new(); 
        
        // Return a list of all items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            // Retrieve all items from the database, convert each to a DTO, and return as a list
            var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto());
            return items;
        }

        // Return a single item by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByid(Guid id)
        {
            // Retrieve the item from the database with the given ID, convert to a DTO, and return
            var item = await itemsRepository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return item.AsDto();
        }

        // Create a new item in the database
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItem(CreateItemDto createItemDto)
        {
            // Create a new Item object based on the provided data, generate a new ID, set the creation date,
            // and add it to the database through the repository
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(item);

            // Return the newly created item's DTO with a "created" status code and location header
            return CreatedAtAction(nameof(GetByid), new { id = item.Id }, item.AsDto());
        }

        // Update an existing item in the database by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, UpdatedItemDto updatedItemDto)
        {
            // Retrieve the existing item from the database, update its properties with the provided data,
            // and update it in the database through the repository
            var existingItem = await itemsRepository.GetAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updatedItemDto.Name;
            existingItem.Description = updatedItemDto.Description;
            existingItem.Price = updatedItemDto.Price;

            await itemsRepository.UpdateAsync(existingItem);

            // Return a "no content" status code to indicate successful update
            return NoContent();
        }

        // Delete an existing item in the database by ID
        [HttpDelete("{id}")]
        public async Task <IActionResult> DeleteItem(Guid id)
        {
            // Retrieve the existing item from the database and remove it through the repository
            var existingItem = await itemsRepository.GetAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            await itemsRepository.RemoveAsync(id);

            // Return a "no content" status code to indicate successful deletion
            return NoContent();
        }
    }
}
