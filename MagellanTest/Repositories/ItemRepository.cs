namespace MagellanTest.Repositories
{
    // Repositories/ItemRepository.cs
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using MagellanTest.Models;
    using Npgsql;

    public class ItemRepository
    {
        private readonly NpgsqlConnection _dbConn;

        public ItemRepository(NpgsqlConnection dbConnection)
        {
            _dbConn = dbConnection;
        }

        public async Task<int> CreateItemAsync(ItemsModel newItemDto)
        {
            try
            {
                int newItemId = 0;

                var sql =
                    "INSERT INTO item (item_name, parent_item, cost, req_date) "
                    + "VALUES (@ItemName, @ParentItem, @Cost, @ReqDate) RETURNING id";

                await using (var cmd = new NpgsqlCommand(sql, _dbConn))
                {
                    // if (newItemDto.ParentItem.HasValue)
                    // {
                    //     cmd.Parameters.AddWithValue("ParentItem", newItemDto.ParentItem); //possible null value
                    // }
                    // else
                    // {
                    //     cmd.Parameters.AddWithValue("ParentItem", DBNull.Value); //possible null value
                    // }

                    cmd.Parameters.AddWithValue("ItemName", newItemDto.ItemName);
                    cmd.Parameters.AddWithValue(
                        "ParentItem",
                        newItemDto.ParentItem.HasValue ? newItemDto.ParentItem : DBNull.Value
                    ); //possible null value
                    cmd.Parameters.AddWithValue("Cost", newItemDto.Cost);
                    cmd.Parameters.AddWithValue("ReqDate", newItemDto.ReqDate);
                    var insertId = await cmd.ExecuteNonQueryAsync();

                    newItemId = (int)insertId;
                }
                return newItemId;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in Create Item: {e.Message}");

                throw;
            }
        }

        public async Task<ItemsModel> GetItemById(int id)
        {
            try
            {
                var sql =
                    "SELECT id, item_name, parent_item, cost, req_date FROM item WHERE id = @Id";

                using var cmd = new NpgsqlCommand(sql, _dbConn);
                cmd.Parameters.AddWithValue("Id", id);

                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    // Map data from the reader to an ItemDto object
                    var item = new ItemsModel
                    {
                        Id = reader.GetInt32(0),
                        ItemName = reader.GetString(1),
                        ParentItem = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2),
                        Cost = reader.GetInt32(3),
                        ReqDate = reader.GetDateTime(4)
                    };

                    return item;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in GetItemById: {e.Message}");
                // throw;
                // Return null if no item is found with the given id
                return null;
            }
        }

        public async Task<int> GetItemTotalCost(string itemName)
        {
            try
            {
                var sql = "SELECT get_total_cost(@ItemName)";

                await using var cmd = new NpgsqlCommand(sql, _dbConn);
                cmd.Parameters.AddWithValue("ItemName", itemName);

                await using var reader = await cmd.ExecuteReaderAsync();
                // Execute the function and retrieve the result
                // var result = await command.ExecuteScalarAsync();

                // Handle the result based on your function's return type
                // return result as int?;
                if (await reader.ReadAsync())
                {
                    return reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                Console.WriteLine($"Error in GetItemCostAsync: {ex.Message}");
                // return 0;
                if (ex.Message == "Column 'get_total_cost' is null")
                {
                    return 0;
                }
                throw;
            }

            return 0;
        }
    }
}
