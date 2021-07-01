using System;
using System.Collections.Generic;

namespace Notebook.Models
{
    public class Item
    {
        public Item()
        {
            Items = new List<Item>();
        }
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();
    }
}