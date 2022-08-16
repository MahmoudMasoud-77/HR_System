using HRSystem.Models;
using Microsoft.AspNetCore.Mvc;
using HRSystem.Data;
using Microsoft.AspNetCore.Authorization;
using HRSystem.Constants;

namespace HRSystem.Controllers
{
    public class GeneralSettingsController : Controller
    {
        private readonly ApplicationDbContext context;

        public GeneralSettingsController(ApplicationDbContext context)
        {
            this.context = context;
        }


        // Index | GeneralSettings Page
        [Authorize(Permissions.GeneralSettings.View)]
        public IActionResult Index()
        {
            var result = context.GeneralSettings.ToList();
            return View(result);
        }



        // New | Add New GeneralSetting
        [Authorize(Permissions.GeneralSettings.Create)]
        public IActionResult New()
        {
            var weekEndList = new List<string>()
                    { "Friday", "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday"};
            ViewBag.WeekEndList = weekEndList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(GeneralSettings newGeneralSetting)
        {
            if (ModelState.IsValid)
            {
                context.GeneralSettings.Add(newGeneralSetting);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newGeneralSetting);

        }


        // Edit | Update existed GeneralSetting
        [Authorize(Permissions.GeneralSettings.Edit)]
        public IActionResult Edit(int? id)
        {
            var weekEndList = new List<string>()
                    { "Friday", "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday"};
            ViewBag.WeekEndList = weekEndList;
            var result = context.GeneralSettings.Find(id);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(GeneralSettings editModel)
        {
            if (ModelState.IsValid)
            {
                context.GeneralSettings.Update(editModel);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(editModel);

        }



        // Delete | Remove existed GeneralSetting
        public IActionResult Delete(int? id)
        {
            var result = context.GeneralSettings.Find(id);
            if (result != null)
            {
                context.GeneralSettings.Remove(result);
                context.SaveChanges();
            }

            return RedirectToAction("Index");

        }
    }
}
