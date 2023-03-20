using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Farmacie.Data;
using Farmacie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Farmacie.Controllers
{
    public class MedicamentsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public MedicamentsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
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

            //adaugam si comenzile userului
            ViewBag.UserCommands = db.Commands.Where(c => c.UserId == _userManager.GetUserId(User))
                                                .ToList();

            return View(medicament);
        }

        //============== MedicamentCommand
        [HttpPost]
        public IActionResult AddCommand([FromForm] MedicamentCommand medicamentCommand)
        {
            if (ModelState.IsValid)
            {
                //verificam daca e deja in comanda
                if (db.MedicamentCommands
                    .Where(ab => ab.MedicamentId == medicamentCommand.MedicamentId)
                    .Where(ab => ab.CommandId == medicamentCommand.CommandId)
                    .Count() > 0)
                {
                    TempData["message"] = "Acest medicament este deja adaugat la comanda!";
                    TempData["messageType"] = "alert-danger";
                }
                else
                {
                    MedicamentCommand medicamentCommand1 = new MedicamentCommand();
                    medicamentCommand1.MedicamentId = medicamentCommand.MedicamentId;
                    medicamentCommand1.CommandId = medicamentCommand.CommandId;
                    medicamentCommand1.QuantityWanted = medicamentCommand.QuantityWanted;
                    Medicament med = db.Medicaments
                                        .Where(a => a.Id == medicamentCommand.MedicamentId)
                                        .First();
                    medicamentCommand1.MedicamentName = med.Name;


                    db.MedicamentCommands.Add(medicamentCommand1);
                    db.SaveChanges();
                    TempData["message"] = "Acest medicament a fost adaugat la comanda!";
                    TempData["messageType"] = "alert-success";
                }
            }
            else
            {
                TempData["message"] = "Nu s-a putut adauga medicamentul la comanda!";
                TempData["messageType"] = "alert-danger";
            }
            return Redirect("/Medicaments/Show/" + medicamentCommand.MedicamentId);
        }


        ////adaugare medicament in baza de date
        [Authorize(Roles = "Admin,Farmacist")]
        public IActionResult New()
        {
            Medicament medicament = new Medicament();
            return View(medicament);
        }

        [Authorize(Roles = "Admin,Farmacist")]
        [HttpPost]
        public IActionResult New(Medicament medicament)
        {
            if (ModelState.IsValid)
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
        [Authorize(Roles = "Admin,Farmacist")]
        public IActionResult Edit(int id)
        {
            Medicament medicament = db.Medicaments.Where(m => m.Id == id)
                                                    .First();
            return View(medicament);
        }

        [Authorize(Roles = "Admin,Farmacist")]
        [HttpPost]
        public IActionResult Edit(int id, Medicament requestmedicament)
        {
            Medicament medicament = db.Medicaments.Find(id);

            if (ModelState.IsValid)
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
        [Authorize(Roles = "Admin,Farmacist")]
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

