using Notebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>()
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            };
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(FindItem(items, s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false, string parentId = null)
        {
            if (string.IsNullOrEmpty(parentId))
                return await Task.FromResult(items);
            else
                return await Task.FromResult(FindItem(items, parentId).Items);
        }

        public async Task<bool> AddChildAsync(Item item, string parentId)
        {
            Item parent = FindItem(items, parentId);

            if (parent.Items == null)
                parent.Items = new List<Item>();

            parent.Items.Add(item);

            return await Task.FromResult(true);
        }

        private Item FindItem(IEnumerable<Item> items, string id)
        {
            Item result = null;

            foreach (Item item in items ?? new List<Item>())
            {
                if (item.Id == id)
                    result = item;
                else
                    result = FindItem(item.Items, id);
                if (result != null)
                    break;
            }

            return result;
        }

        private Item FindItem(IEnumerable<Item> items, Func<Item, bool> function)
        {
            Item result = null;

            foreach (Item item in items ?? new List<Item>())
            {
                if (function(item))
                    result = item;
                else
                    result = FindItem(item.Items, function);
                if (result != null)
                    break;
            }

            return result;
        }

        async public Task<Item> GetParent(string childId)
        {
            return await Task.FromResult(FindItem(items, (item) => { return item.Items.Any(i => i.Id == childId); }));
        }
    }
}