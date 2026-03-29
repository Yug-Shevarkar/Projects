using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ItemProcessorApp.Models;
using ItemProcessorApp.Data;
using System.Collections.Generic;

namespace ItemProcessorApp.Controllers
{
    public class ItemController : Controller
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Process(double weight)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var root = new Item
            {
                Weight = weight,
                ParentId = null
            };

            ProcessItem(root);
            SaveItems(root, null);

            return View("Result", root);
        }

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