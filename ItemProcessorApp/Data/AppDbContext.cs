using System.Collections.Generic;

namespace ItemProcessorApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        public double Weight { get; set; }

        public int? ParentId { get; set; }
        public Item Parent { get; set; }

        public List<Item> Children { get; set; }
    }
}