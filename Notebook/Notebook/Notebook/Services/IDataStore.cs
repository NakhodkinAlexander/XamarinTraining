using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notebook.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> AddChildAsync(T item, string parentId);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false, string parentId = null);
        Task<T> GetParent(string childId = null);
    }
}
