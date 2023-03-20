using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Farmacie.Data;
using Farmacie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Farmacie.Controllers
{
    public class CommandsController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public CommandsController(ApplicationDbContext context,
                                    UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //afisare lista cu toate comenzile
        public IActionResult Index()
        {
            if(User.IsInRole("Admin") || User.IsInRole("Farmacist"))
            {
                var commands = db.Commands.Include("User")
                                            .OrderBy(a => a.Id);
                ViewBag.Commands = commands;
                return View();
            }
            else
            {
                var commands = db.Commands.Include("User")
                                            .Where(a => a.UserId == _userManager.GetUserId(User))
                                            .OrderBy(a => a.Id);
                ViewBag.Commands = commands;
                return View();
            }

        }

        //afisare comanda dupa id comanda

        public IActionResult Show(int id)
        {
            Command command = db.Commands
                                    .Include("User")
                                    .Where(c => c.Id == id)
                                    .First();

            if (command.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin") || User.IsInRole("Farmacist"))
            {
                ViewBag.MedicamentsCommand = db.MedicamentCommands.Where(c => c.CommandId == id)
                                                                    .ToList();

                return View(command);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa vedeti o comanda ce nu va apartine";
                return RedirectToAction("Index");
            }
        }

        //adaugare comanda in baza de date
        public IActionResult New()
        {
            Command command = new Command();
            //command.Med = GetAllMedicaments();

            return View(command);
        }

        [HttpPost]
        public IActionResult New(Command command)
        {
            command.UserId = _userManager.GetUserId(User);
            if(ModelState.IsValid)
            {
                db.Commands.Add(command);
                db.SaveChanges();
                TempData["message"] = "Comanda a fost realizata!";
                return RedirectToAction("Index");
            }
            else
            {
                //command.Med = GetAllMedicaments();
                return View(command);
            }
        }

        //se editeaza datele unei comenzi
        public IActionResult Edit(int id)
        {
            Command command = db.Commands.Where(c => c.Id == id)
                                                    .First();

            if (command.UserId == _userManager.GetUserId(User))
            {
                ViewBag.MedicamentsCommand = db.MedicamentCommands.Where(c => c.CommandId == id)
                                                                    .ToList();
                return View(command);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei comenzi ce nu va apartine";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, Command requestcommand)
        {
            Command command = db.Commands.Find(id);

            if (ModelState.IsValid)
            {
                if(command.UserId == _userManager.GetUserId(User))
                {
                    command.PatientName = requestcommand.PatientName;
                    command.Diagnostic = requestcommand.Diagnostic;
                    //command.CommandMedicaments = requestcommand.CommandMedicaments;

                    TempData["message"] = "Comanda a fost modificat!";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei comenzi ce nu va apartine";
                    return RedirectToAction("Index");
                }
                
            }
            else
            {
                return View(requestcommand);
            }
        }

        //stergerea unei comenzi din baza de date
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Command command = db.Commands.Where(c => c.Id == id)
                                                    .First();

            if(command.UserId == _userManager.GetUserId(User))
            {
                db.Commands.Remove(command);
                db.SaveChanges();
                TempData["message"] = "Comanda a fost sters!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti o comanda ce nu va apartine";
                return RedirectToAction("Index");
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllMedicaments()
        {
            var selectList = new List<SelectListItem>();

            //extragem toate categoriile din baza de date
            var medicaments = from med in db.Medicaments
                             select med;

            //iteram prin categorii
            foreach (var medicament in medicaments)
            {
                //adaugam in lista categoriile
                selectList.Add(new SelectListItem
                {
                    Value = medicament.Id.ToString(),
                    Text = medicament.Name.ToString()
                });
            }

            return selectList;
        }
    }
}

