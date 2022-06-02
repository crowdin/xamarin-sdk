
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestMobileApp.Models;

namespace TestMobileApp.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>()
            {
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "First item",
                    Description="This is an item description."
                },
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "Second item",
                    Description="This is an item description."
                },
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "Third item",
                    Description="This is an item description."
                },
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "Fourth item",
                    Description="This is an item description."
                },
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "Fifth item",
                    Description="This is an item description."
                },
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "Sixth item",
                    Description="This is an item description."
                }
            };
        }

        public Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);
            return Task.FromResult(true);
        }

        public Task<bool> UpdateItemAsync(Item item)
        {
            Item oldItem = items.FirstOrDefault(arg => arg.Id == item.Id);
            items.Remove(oldItem);
            items.Add(item);
            
            return Task.FromResult(true);
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            Item oldItem = items.FirstOrDefault(arg => arg.Id == id);
            items.Remove(oldItem);

            return Task.FromResult(true);
        }

        public Task<Item> GetItemAsync(string id)
        {
            return Task.FromResult(items.FirstOrDefault(item => item.Id == id));
        }

        public Task<List<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return Task.FromResult(items);
        }
    }
}