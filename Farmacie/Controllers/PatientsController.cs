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
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext db;
        public PatientsController(ApplicationDbContext context)
        {
            db = context;
        }

        //afisam lista cu toti pacientii
        public IActionResult Index()
        {
            var patients = from patient in db.Patients
                           select patient;
            ViewBag.Patients = patients;
            return View();
        }

        //afisare pacient dupa id
        public IActionResult Show(int id)
        {
            Patient patient = db.Patients
                                .Where(patient => patient.Id == id)
                                .First();
            return View(patient);
        }

        //adaugare pacient
        //se afiseaza formularul de adaugare
        public IActionResult New()
        {
            Patient patient = new Patient();
            return View(patient);
        }

        //se adauga pacientul in baza de date
        [HttpPost]
        public IActionResult New(Patient patient)
        {
            if(ModelState.IsValid)
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
        public IActionResult Edit(int id)
        {
            Patient patient = db.Patients.Where(p => p.Id == id)
                                        .First();
            return View(patient);
        }

        [HttpPost]
        public IActionResult Edit(int id, Patient requestpatient)
        {
            Patient patient = db.Patients.Find(id);

            if(ModelState.IsValid)
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
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Patient patient = db.Patients.Where(p => p.Id == id)
                                        .First();
            db.Patients.Remove(patient);
            db.SaveChanges();
            TempData["message"] = "Pacientul a fost sters din baza de date";
            return RedirectToAction("Index");
        }
    }
}

