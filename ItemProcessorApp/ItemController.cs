using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ItemProcessorApp.Models;
using ItemProcessorApp.Data;
using System.Linq;

namespace ItemProcessorApp.Controllers
{
    public class ItemController : Controller
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search)
        {
            if (HttpContext.Session.GetString("User") == null)
                return RedirectToAction("Login", "Account");

            var items = _context.Items.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                items = items.Where(i => i.Weight.ToString().Contains(search));

            return View(items.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(double weight)
        {
            if (weight <= 0 || weight > 100)
            {
                ViewBag.Error = "Weight must be between 0 and 100";
                return View();
            }

            var item = new Item { Weight = weight };
            _context.Items.Add(item);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var item = _context.Items.Find(id);
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(int id, double weight)
        {
            var item = _context.Items.Find(id);

            if (item != null)
            {
                item.Weight = weight;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var item = _context.Items.Find(id);

            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Process(int id)
        {
            var root = _context.Items.Find(id);

            ProcessItem(root);

            return View("Result", root);
        }

        private void ProcessItem(Item item)
        {
            if (item.Weight <= 1) return;

            var child1 = new Item { Weight = item.Weight / 2 };
            var child2 = new Item { Weight = item.Weight / 2 };

            item.Children = new System.Collections.Generic.List<Item> { child1, child2 };

            foreach (var child in item.Children)
            {
                ProcessItem(child);
            }
        }
    }
}