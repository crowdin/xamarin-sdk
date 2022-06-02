
using System.Collections.Generic;
using System.Threading.Tasks;
using TestMobileApp.Models;

namespace TestMobileApp.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        
        Task<bool> UpdateItemAsync(T item);
        
        Task<bool> DeleteItemAsync(string id);
        
        Task<T> GetItemAsync(string id);
        
        Task<List<Item>> GetItemsAsync(bool forceRefresh = false);
    }
}
