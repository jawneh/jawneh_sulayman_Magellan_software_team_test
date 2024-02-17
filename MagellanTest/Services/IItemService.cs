using MagellanTest.Models;

namespace MagellanTest.Services
{
    public interface IItemService
    {
        Task<int> CreateItem(ItemsModel newItemDto);

        Task<ItemsModel> GetItemById(int id);

        Task<int> GetItemTotalCost(string itemName);
    }
}
