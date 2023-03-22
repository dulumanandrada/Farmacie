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
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {

        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UsersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = from user in db.Users
                        orderby user.UserName
                        select user;
            var search = "";

            // MOTOR DE CAUTARE

            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim(); // eliminam spatiile libere 

                // Cautare dupa nume
                List<string> usersIds = db.Users.Where
                                        (
                                         at => at.UserName.Contains(search)
                                        ).Select(a => a.Id).ToList();

                // Lista pacientilor
                users = (IOrderedQueryable<ApplicationUser>)db.Users.Where(user => usersIds.Contains(user.Id));

            }

            ViewBag.SearchString = search;

            ViewBag.UsersList = users;

            return View();
        }

        public async Task<ActionResult> Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = roles;
            return View(user);
        }

        public async Task<ActionResult> Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);

            user.AllRoles = GetAllRoles();

            var roleNames = await _userManager.GetRolesAsync(user); // Lista de nume de roluri

            // Cautam ID-ul rolului in baza de date
            var currentUserRole = _roleManager.Roles
                                              .Where(r => roleNames.Contains(r.Name))
                                              .Select(r => r.Id)
                                              .First(); // Selectam 1 singur rol
            ViewBag.UserRole = currentUserRole;

            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, ApplicationUser newData, [FromForm] string newRole)
        {
            ApplicationUser user = db.Users.Find(id);

            user.AllRoles = GetAllRoles();


            if (ModelState.IsValid)
            {
                user.UserName = newData.UserName;
                user.Email = newData.Email;
                user.FirstName = newData.FirstName;
                user.LastName = newData.LastName;
                user.PhoneNumber = newData.PhoneNumber;


                // Cautam toate rolurile din baza de date
                var roles = db.Roles.ToList();

                foreach (var role in roles)
                {
                    // Scoatem userul din rolurile anterioare
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                // Adaugam noul rol selectat
                var roleName = await _roleManager.FindByIdAsync(newRole);
                await _userManager.AddToRoleAsync(user, roleName.ToString());

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Delete(string id)
        {
            var user = db.Users
                         .Where(u => u.Id == id)
                         .First();

            /* Delete user commands
            if (user.Articles.Count > 0)
            {
                foreach (var article in user.Articles)
                {
                    db.Articles.Remove(article);
                }
            }
            */

            //stergem si comenzile userului odata cu acesta
            if(user.Commands.Count > 0)
            {
                foreach(var com in user.Commands)
                {
                    db.Commands.Remove(com);
                }
            }

            db.ApplicationUsers.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();

            var roles = from role in db.Roles
                        select role;

            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }
    }
}
