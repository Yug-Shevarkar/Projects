using Microsoft.AspNetCore.Mvc;
using ItemProcessorApp.Models;
using ItemProcessorApp.Data;

namespace ItemProcessorApp.Controllers
{
    public class ItemController : Controller
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        // Show input page
        public IActionResult Index()
        {
            return View();
        }

        // Process item
        [HttpPost]
        public IActionResult Process(double weight)
        {
            var root = new Item
            {
                Weight = weight,
                ParentId = null
            };

            ProcessItem(root);

            SaveItems(root, null);

            return View("Result", root);
        }

        // 🔁 Recursion logic
        private void ProcessItem(Item item)
        {
            if (item.Weight <= 1) return;

            var child1 = new Item { Weight = item.Weight / 2 };
            var child2 = new Item { Weight = item.Weight / 2 };

            item.Children = new List<Item> { child1, child2 };

            foreach (var child in item.Children)
            {
                ProcessItem(child);
            }
        }

        // 💾 Save to DB
       private void SaveItems(Item item, int? parentId)
{
    var newItem = new Item
    {
        Weight = item.Weight,
        ParentId = parentId
    };

    _context.Items.Add(newItem);
    _context.SaveChanges();

    if (item.Children != null)
    {
        foreach (var child in item.Children)
        {
            SaveItems(child, newItem.Id);
        }
    }
}
    }
}