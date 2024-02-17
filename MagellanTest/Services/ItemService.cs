using MagellanTest.Models;
using MagellanTest.Repositories;

namespace MagellanTest.Services
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message)
            : base(message) { }
    }

    public class ItemService : IItemService
    {
        private readonly ItemRepository _itemRepository;

        public ItemService(ItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<int> CreateItem(ItemsModel newItemDto)
        {
            if (newItemDto.ParentItem.HasValue)
            {
                //check if the parent item exists in database
                _ =
                    await _itemRepository.GetItemById(newItemDto.ParentItem.Value)
                    ?? throw new ItemNotFoundException(
                        $"Parent item with id {newItemDto.ParentItem.Value} does not exist."
                    );
            }

            var itemiD = await _itemRepository.CreateItemAsync(newItemDto);
            return itemiD;
        }

        public async Task<ItemsModel> GetItemById(int id)
        {
            var item =
                await _itemRepository.GetItemById(id)
                ?? throw new ItemNotFoundException($"Item with ID {id} not found.");

            return item;
        }

        public async Task<int> GetItemTotalCost(string itemName)
        {
            try
            {
                var totalCost = await _itemRepository.GetItemTotalCost(itemName);
                return totalCost;
            }
            catch (Exception ex)
            {
                throw new ItemNotFoundException($"Item with name {itemName} {ex.Message}");
            }
        }
    }
}
