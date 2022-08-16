using HRSystem.Constants;
using HRSystem.Data;
using HRSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.Controllers
{
    public class HolidaysController : Controller
    {
        private readonly ApplicationDbContext context;

        public HolidaysController(ApplicationDbContext context)
        {
            this.context = context;
        }


        // Index | Holidays Page
        [Authorize(Permissions.Holidays.View)]
        public IActionResult Index()
        {
            var result = context.Holidays.ToList();
            return View(result);
        }



        // New | Add New Holiday
        [Authorize(Permissions.Holidays.Create)]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(Holidays newHoliday)
        {
            if (ModelState.IsValid)
            {
                context.Holidays.Add(newHoliday);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newHoliday);

        }



        // Edit | Update existed Holiday
        [Authorize(Permissions.Holidays.Edit)]
        public IActionResult Edit(int? id)
        {
            var result = context.Holidays.Find(id);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Holidays editModel)
        {
            if (ModelState.IsValid)
            {
                context.Holidays.Update(editModel);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(editModel);

        }



        // Delete | Remove existed Holiday
        public IActionResult Delete(int? id)
        {
            var result = context.Holidays.Find(id);
            if (result != null)
            {
                context.Holidays.Remove(result);
                context.SaveChanges();
            }

            return RedirectToAction("Index");

        }

    }
}
