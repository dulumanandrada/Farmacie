using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Farmacie.Data;
using Farmacie.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Farmacie.Controllers
{
    public class MedicamentsController : Controller
    {
        private readonly ApplicationDbContext db;
        public MedicamentsController(ApplicationDbContext context)
        {
            db = context;
        }

        //afisa lista cu toate medicamentele
        public IActionResult Index()
        {
            var medicaments = from medicament in db.Medicaments
                              select medicament;
            ViewBag.Medicaments = medicaments;
            return View();
        }

        //afisare medicament dupa id
        public IActionResult Show(int id)
        {
            Medicament medicament = db.Medicaments
                                    .Where(m => m.Id == id)
                                    .First();
            return View(medicament);
        }

        //adaugare medicament in baza de date
        public IActionResult New()
        {
            Medicament medicament = new Medicament();
            return View(medicament);
        }

        [HttpPost]
        public IActionResult New(Medicament medicament)
        {
            if(ModelState.IsValid)
            {
                db.Medicaments.Add(medicament);
                db.SaveChanges();
                TempData["message"] = "Medicamentul a fost adaugat!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(medicament);
            }
        }

        //se editeaza datele unui medicament
        public IActionResult Edit(int id)
        {
            Medicament medicament = db.Medicaments.Where(m => m.Id == id)
                                                    .First();
            return View(medicament);
        }

        [HttpPost]
        public IActionResult Edit(int id, Medicament requestmedicament)
        {
            Medicament medicament = db.Medicaments.Find(id);

            if(ModelState.IsValid)
            {
                medicament.Name = requestmedicament.Name;
                medicament.Weight = requestmedicament.Weight;
                medicament.Form = requestmedicament.Form;
                medicament.Lot = requestmedicament.Lot;
                medicament.ExpDate = requestmedicament.ExpDate;
                medicament.Quantity = requestmedicament.Quantity;

                TempData["message"] = "Medicamentul a fost modificat!";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(requestmedicament);
            }
        }

        //stergerea unui medicament din baza de date
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Medicament medicament = db.Medicaments.Where(m => m.Id == id)
                                                    .First();
            db.Medicaments.Remove(medicament);
            db.SaveChanges();
            TempData["message"] = "Medicamentul a fost sters!";
            return RedirectToAction("Index");
        }
    }
}

