#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetAtlas_The_True_Project.Data;
using NetAtlas_The_True_Project.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace NetAtlas_The_True_Project.Controllers
{
    public class InternauteController : Controller
    {
        private readonly NetAtlasDbContext _context;

        public InternauteController(NetAtlasDbContext context)
        {
            _context = context;
        }

        // GET: Internaute
        public async Task<IActionResult> Index()
        {
            return View(await _context.Internautes.ToListAsync());
        }

        // GET: Internaute/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internaute = await _context.Internautes
                .FirstOrDefaultAsync(m => m.IdInternaute == id);
            if (internaute == null)
            {
                return NotFound();
            }

            return View(internaute);
        }

        // GET: Internaute/SignUp
        //Page d'inscription
        public IActionResult SignUp()
        {
            return View();
        }

        // POST: Internaute/Create
        //Inscription
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([Bind("IdInternaute,Nom,Prenom,AdresseMail,UserName,Password")] Internaute internaute)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(internaute);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(SignIn));
            }
            return View(internaute);
        }
        // GET: Internaute/SignIn
        //Page de connexion
        public IActionResult SignIn()
        {
            return View();
        }

        // POST: Internaute/SignIn
        //Connexion
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn([Bind("IdMembre,Nom,Prenom,AdresseMail,UserName,Password")] Membre membre)
        {
            if (!ModelState.IsValid)
            {
                var UnMembre = await _context.Membres.ToListAsync();
                foreach(var i in UnMembre)
                {
                    if(membre.UserName.Equals(i.UserName)){
                        if (membre.Password.Equals(i.Password))
                        {
                            //recupérer la valeur de l'id du membre 
                            membre.IdMembre = _context.Membres.Where(i => i.UserName.Equals(membre.UserName)).FirstOrDefault().IdMembre;
                            //Mettre les informations de membre dans une clé de session
                            HttpContext.Session.SetString("MembreSession",JsonConvert.SerializeObject(membre));
                            return RedirectToAction(nameof(Index),"Membre");
                        }
                    }
                }
                return NotFound();
            }
            return View(membre);
        }
        // GET: Internaute/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internaute = await _context.Internautes.FindAsync(id);
            if (internaute == null)
            {
                return NotFound();
            }
            return View(internaute);
        }

        // POST: Internaute/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdInternaute,Nom,Prenom,AdresseMail,UserName,Password")] Internaute internaute)
        {
            if (id != internaute.IdInternaute)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(internaute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InternauteExists(internaute.IdInternaute))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(internaute);
        }

        // GET: Internaute/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internaute = await _context.Internautes
                .FirstOrDefaultAsync(m => m.IdInternaute == id);
            if (internaute == null)
            {
                return NotFound();
            }

            return View(internaute);
        }

        // POST: Internaute/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var internaute = await _context.Internautes.FindAsync(id);
            _context.Internautes.Remove(internaute);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InternauteExists(int id)
        {
            return _context.Internautes.Any(e => e.IdInternaute == id);
        }
    }
}
