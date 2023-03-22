using System;
using System.Collections.Generic;
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
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public PatientsController(ApplicationDbContext context,
                                    UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //afisam lista cu toti pacientii
        [Authorize(Roles = "Admin,Farmacist")]
        public IActionResult Index()
        {
            var patients = from patient in db.Patients
                           select patient;
            var search = "";

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Msg = TempData["message"];
            }

            

            if (User.IsInRole("Admin") || User.IsInRole("Farmacist"))
            {
                // MOTOR DE CAUTARE

                if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
                {
                    search = Convert.ToString(HttpContext.Request.Query["search"]).Trim(); // eliminam spatiile libere 

                    // Cautare dupa nume
                    List<string> patientsIds = db.Patients.Where
                                            (
                                             at => at.LastName.Contains(search)
                                             || at.FirstName.Contains(search)
                                             || at.CNP.Contains(search)
                                            ).Select(a => a.Id).ToList();

                    // Lista pacientilor
                    patients = db.Patients.Where(patient => patientsIds.Contains(patient.Id));

                }

                ViewBag.SearchString = search;

                ViewBag.Patients = patients;

                SetAccessRights();
                return View();
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa vedeti pacientii farmaciei!";
                return Redirect("Home");
            }
        }

        //afisare pacient dupa id
        [Authorize(Roles = "Admin,Farmacist,Customer")]
        public IActionResult Show(string id)
        {
            Patient patient = db.Patients
                                .Where(patient => patient.Id == id)
                                .First();

            if (User.IsInRole("Admin") || User.IsInRole("Farmacist") || _userManager.GetUserId(User) == patient.Id)
            {
                SetAccessRights();
                return View(patient);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa vedeti acest pacient al farmaciei!";
                return RedirectToPage("/Home/Index");

            }
        }

        //adaugare pacient
        //se afiseaza formularul de adaugare
        [Authorize(Roles = "Admin,Farmacist")]
        public IActionResult New()
        {
            Patient patient = new Patient();
            return View(patient);
        }

        //se adauga pacientul in baza de date
        [Authorize(Roles = "Admin,Farmacist")]
        [HttpPost]
        public IActionResult New(Patient patient)
        {
            patient.Id = patient.CNP;

            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                TempData["message"] = "Pacientul a fost adaugat in baza de date!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(patient);
            }
        }

        //se editeaza datele unui pacient
        [Authorize(Roles = "Admin,Farmacist,Customer")]
        public IActionResult Edit(string id)
        {
            
            Patient patient = db.Patients.Where(p => p.Id == id)
                                        .First();
           

            if(User.IsInRole("Admin") || User.IsInRole("Farmacist") || _userManager.GetUserId(User) == id)
            {
                return View(patient);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa editati acest pacient al farmaciei!";
                return RedirectToPage("/Home/Index");
            }
     
        }

        [Authorize(Roles = "Admin,Farmacist,Customer")]
        [HttpPost]
        public IActionResult Edit(string id, Patient requestpatient)
        {
            Patient patient = db.Patients.Find(id);

            if (ModelState.IsValid)
            {
                patient.FirstName = requestpatient.FirstName;
                patient.LastName = requestpatient.LastName;
                patient.CNP = requestpatient.CNP;
                patient.Phone = requestpatient.Phone;
                patient.Address = requestpatient.Address;

                TempData["message"] = "Pacientul a fost editat!";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(requestpatient);
            }
        }

        //se sterge un pacient din baza de date
        [Authorize(Roles = "Admin,Farmacist")]
        [HttpPost]
        public IActionResult Delete(string id)
        {
            Patient patient = db.Patients.Where(p => p.Id == id)
                                        .First();
            db.Patients.Remove(patient);
            db.SaveChanges();
            TempData["message"] = "Pacientul a fost sters din baza de date";
            return RedirectToAction("Index");
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

