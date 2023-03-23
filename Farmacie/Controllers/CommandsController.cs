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
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Msg = TempData["message"];
            }

            if (User.IsInRole("Admin") || User.IsInRole("Farmacist"))
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
            SetAccessRights();

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
            command.AllPatients = GetAllPatients();

            return View(command);
        }

        [HttpPost]
        public IActionResult New(Command command)
        {
            command.AllPatients = GetAllPatients();
            command.UserId = _userManager.GetUserId(User);
            command.Status = "WAITING";

            if (ModelState.IsValid)
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

        //=====================================================================
        [Authorize(Roles = "Admin,Farmacist")]
        //se accepta o comanda
        [HttpPost]
        public IActionResult ChangeStatus(int id)
        {
            Command command = db.Commands.Include("User")
                                    .Where(c => c.Id == id)
                                    .First();
            bool ok = true;

            if (User.IsInRole("Admin") || User.IsInRole("Farmacist"))
            {
                ViewBag.MedicamentsCommand = db.MedicamentCommands.Where(c => c.CommandId == id)
                                                                    .ToList();
                //var medcom = from med in db.MedicamentCommands
                //             select med;
                var medcom = db.MedicamentCommands.Where(c => c.CommandId == id)
                                                                    .ToList();

                foreach (var mc in medcom)
                {
                    var medicament = db.Medicaments.Where(a => a.Id == mc.MedicamentId).First();
                    if (mc.QuantityWanted > medicament.Quantity)
                        ok = false;
                }

                if(ok == true)
                {
                    foreach (var mc in medcom)
                    {
                        Medicament medicament = db.Medicaments.Find(mc.MedicamentId);
                        int a = medicament.Quantity;
                        int b;
                        if (mc.QuantityWanted == null)
                        {
                            b = 0;
                        }
                        else
                        {
                            b = (int)mc.QuantityWanted;
                        }
                        medicament.Quantity = a - b;

                    }
                    TempData["message"] = "Comanda a fost acceptata!";
                    command.Status = "ACCEPTED";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Comanda nu poate fi acceptata! Stoc insuficient!";
                    return RedirectToAction("Index");
                }  
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra statusului!";
                return RedirectToAction("Index");
            }
        }




        //[Authorize(Roles = "Admin,Farmacist")]
        ////se editeaza statusul
        //public IActionResult ChangeStatus(int id)
        //{
        //    Command command = db.Commands.Include("User")
        //                            .Where(c => c.Id == id)
        //                            .First();
        //    command.AllStatus = GetAllStatus();

        //    if (User.IsInRole("Admin") || User.IsInRole("Farmacist"))
        //    {
        //        ViewBag.MedicamentsCommand = db.MedicamentCommands.Where(c => c.CommandId == id)
        //                                                            .ToList();
        //        return View(command);
        //    }

        //    else
        //    {
        //        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra statusului!";
        //        return RedirectToAction("Index");
        //    }
        //}

        //[Authorize(Roles = "Admin,Farmacist")]
        //[HttpPost]
        //public IActionResult ChangeStatus(int id, [FromForm] string newstatus)
        //{
        //    Command command = db.Commands.Find(id);

        //    command.AllStatus = GetAllStatus();

        //    if (ModelState.IsValid)
        //    {
        //        if (User.IsInRole("Admin") || User.IsInRole("Farmacist"))
        //        {
        //            command.Status = newstatus;

        //            TempData["message"] = "Comanda a fost modificata!";
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra statusului!";
        //            return RedirectToAction("Index");
        //        }

        //    }
        //    else
        //    {
        //        return View(command);
        //    }
        //}

        //se editeaza datele unei comenzi
        public IActionResult Edit(int id)
        {
            Command command = db.Commands.Where(c => c.Id == id)
                                                    .First();
            command.AllPatients = GetAllPatients();

            if (command.UserId == _userManager.GetUserId(User) && command.Status != "ACCEPTED")
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
            command.AllPatients = GetAllPatients();

            if (ModelState.IsValid)
            {
                if (command.UserId == _userManager.GetUserId(User))
                {
                    command.Name = requestcommand.Name;
                    command.Diagnostic = requestcommand.Diagnostic;
                    command.PatientId = requestcommand.PatientId;
                    command.Patient = requestcommand.Patient;
                    //command.CommandMedicaments = requestcommand.CommandMedicaments;

                    TempData["message"] = "Comanda a fost modificata!";
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

            if (command.UserId == _userManager.GetUserId(User))
            {
                db.Commands.Remove(command);
                db.SaveChanges();
                TempData["message"] = "Comanda a fost stearsa!";
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

        [NonAction]
        public IEnumerable<SelectListItem> GetAllPatients()
        {
            var selectList = new List<SelectListItem>();

            if (User.IsInRole("Admin") || User.IsInRole("Farmacist"))
            {
                //extragem toate categoriile din baza de date
                var patients = from pat in db.Patients
                               select pat;

                //iteram prin categorii
                foreach (var patient in patients)
                {
                    //adaugam in lista categoriile
                    selectList.Add(new SelectListItem
                    {
                        Value = patient.Id.ToString(),
                        Text = patient.FirstName.ToString() + " " + patient.LastName.ToString()
                    });
                }
            }
            else
            {
                //var patient = db.Patients.Where(a => a.Id == _userManager.GetUserId(User))
                //                            .First();
                //selectList.Add(new SelectListItem
                //{
                //    Value = patient.Id.ToString(),
                //    Text = patient.FirstName.ToString() + " " + patient.LastName.ToString()
                //});
                selectList.Add(new SelectListItem
                {
                    Value = _userManager.GetUserId(User),
                    Text = _userManager.GetUserName(User)
                });
            }
            

            return selectList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllStatus()
        {
            var selectList = new List<SelectListItem>();

            selectList.Add(new SelectListItem
            {
                Value = "WAITING",
                Text = "WAITING"
            });

            selectList.Add(new SelectListItem
            {
                Value = "ACCEPTED",
                Text = "ACCEPTED"
            });

            selectList.Add(new SelectListItem
            {
                Value = "DECLINED",
                Text = "DECLINED"
            });

            return selectList;
        }



        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if (User.IsInRole("Admin") || User.IsInRole("Farmacist"))
            {
                ViewBag.AfisareButoane = true;
            }

            ViewBag.UserCurent = _userManager.GetUserId(User);
            ViewBag.EsteAdmin = User.IsInRole("Admin");
            ViewBag.EsteFarmacist = User.IsInRole("Farmacist");
        }
    }
}

