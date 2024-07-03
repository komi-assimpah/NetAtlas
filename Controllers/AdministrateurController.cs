using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetAtlas_The_True_Project.Data;
using NetAtlas_The_True_Project.Models;
using Newtonsoft.Json;

namespace NetAtlas_The_True_Project.Controllers
{
    public class AdministrateurController : Controller
    {
        private readonly NetAtlasDbContext _context;

        public AdministrateurController(NetAtlasDbContext context)
        {
            _context = context;
        }

        // GET: Internaute
        public async Task<IActionResult> ListeInternaute()
        {
            return View(await _context.Internautes.ToListAsync());
        }
        // GET: Membre
        public async Task<IActionResult> ListeMembre()
        {
            return View(await _context.Membres.ToListAsync());
        }
        // GET: Administrateur/SignIn
        //Page de connexion
        public IActionResult SignIn()
        {
            return View();
        }

        // POST: Administrateur/SignIn
        //Connexion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn([Bind("IdAdministrateur,UserName,Password")] Administrateur administrateur)
        {
            if (!ModelState.IsValid)
            {
                var UnAdmin = await _context.Administrateurs.ToListAsync();
                foreach (var i in UnAdmin)
                {
                    if (administrateur.UserName.Equals(i.UserName))
                    {
                        if (administrateur.Password.Equals(i.Password))
                        {
                            //recupérer la valeur de l'id du membre 
                            administrateur.IdAdministrateur = _context.Administrateurs.Where(i => i.UserName.Equals(administrateur.UserName)).FirstOrDefault().IdAdministrateur;
                            //Mettre les informations de membre dans une clé de session
                            HttpContext.Session.SetString("AdministrateurSession", JsonConvert.SerializeObject(administrateur));
                            return RedirectToAction(nameof(ListeInternaute));
                        }
                    }
                }
                return NotFound();
            }
            return View(administrateur);
        }
        public async Task<IActionResult> VoirPublication(int? id)
        {
            return View();
        }
        /*public async Task<ActionResult> VoirPublications(int? id)
        {
            var Administrateur = JsonConvert.DeserializeObject<Administrateur>(HttpContext.Session.GetString("AdministrateurSession"));
            var mesPublications = _context.Publications.Where(m => m.IdMembre == membre.IdMembre).ToList();
            var LesImages = new List<Image_Video>();
            foreach (var item in mesPublications)
            {
                LesImages.AddRange(_context.Image_Videos.Where(m => m.IdPublication == item.IdPublication).ToList());
            }
            return View(LesImages);
        }*/
        //Valider l'inscription
        public async Task<IActionResult> Valider(int? id)
        {
            if(id == null) { return NotFound(); }
            var internaute = await _context.Internautes
                .FirstOrDefaultAsync(m => m.IdInternaute == id);
            if (internaute == null) { return NotFound(); } else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Membre membre = new Membre();
                        membre.UserName = internaute.UserName;
                        membre.Password = internaute.Password;
                        membre.Nom=internaute.Nom;
                        membre.Prenom=internaute.Prenom;
                        membre.AdresseMail = internaute.AdresseMail;
                        _context.Membres.Add(membre);
                        _context.Internautes.Remove(internaute);
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
                    return RedirectToAction(nameof(ListeInternaute));
                }
            }
            return RedirectToAction(nameof(ListeInternaute));
        }
        //Valider l'inscription
        public async Task<IActionResult> Refuser(int? id)
        {
            if (id == null) { return NotFound(); }
            var internaute = await _context.Internautes
                .FirstOrDefaultAsync(m => m.IdInternaute == id);
            if (internaute == null) { return NotFound(); }
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Internautes.Remove(internaute);
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
                    return RedirectToAction(nameof(ListeInternaute));
                }
            }
            return RedirectToAction(nameof(ListeInternaute));
        }
        //Supprimer un membre
        //--Recupérer le membre
        public async Task<IActionResult> Supprimer(int? id)
        {
            var Administrateur = JsonConvert.DeserializeObject<Administrateur>(HttpContext.Session.GetString("AdministrateurSession"));
            if (id == null)
            {
                return NotFound();
            }

            var membre = await _context.Membres
                .FirstOrDefaultAsync(m => m.IdMembre == id);
            if (membre == null)
            {
                return NotFound();
            }

            return View(membre);
        }
        //--Valider la suppression
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Supprimer(int id)
        {
            var Administrateur = JsonConvert.DeserializeObject<Administrateur>(HttpContext.Session.GetString("AdministrateurSession"));
            var membre = await _context.Membres.FindAsync(id);
            _context.Membres.Remove(membre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListeMembre));
        }
        //Rechercher un membre
        //POST ListeMembre/SearchMembre
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListeMembre(string? SearchMembre)
        {
            var Administrateur = JsonConvert.DeserializeObject<Administrateur>(HttpContext.Session.GetString("AdministrateurSession"));
            if (SearchMembre == null) { return RedirectToAction(nameof(ListeMembre)); }
            var membre = _context.Membres.Where(m=>m.UserName.Contains(SearchMembre)).ToList();
            if(membre.Count == 0) { return NotFound(); }
            else
            {
                if (ModelState.IsValid)
                {
                    return View(membre);
                }
            }

            return RedirectToAction(nameof(ListeMembre));
        }
        private bool InternauteExists(int id)
        {
            return _context.Internautes.Any(e => e.IdInternaute == id);
        }
        private bool MembreExists(int id)
        {
            return _context.Membres.Any(e => e.IdMembre == id);
        }
        private bool IsAdmin(Administrateur admin)
        {
            return _context.Administrateurs.Any(m=>m.IdAdministrateur==admin.IdAdministrateur && m.UserName==admin.UserName);
        }
        public bool Authorize(Administrateur administrateur)
        {
            if (IsAdmin(administrateur))
            {
                return true;
            }
            return false;
        }
        public async Task<ActionResult> ListePublications(int? id)
        {
            
            if (id == null)
            {
                return View(nameof(ListeMembre));
            }
            var membre = _context.Membres.Where(m => m.IdMembre == id).FirstOrDefault();
            ViewBag.UserName = membre.UserName;
            var publications = _context.Publications.Where(m=>m.IdMembre==id).ToList();
            return View(publications);
        }
        public async Task<ActionResult> SupprimerPubli(int? id)
        {
            if(id== null)
            {
                return View(nameof(ListePublications));
            }
            var lapub = _context.Publications.Find(id);
            if(lapub == null)
            {
                return NotFound();
            }
            _context.Remove(lapub);
            await _context.SaveChangesAsync();
            return View(nameof(ListeMembre));
        }
        public async Task<ActionResult> MembreImagesVideo(int? id)
        {
            var mesPublications = _context.Publications.Where(m => m.IdMembre == id).ToList();
            var LesImages = new List<Image_Video>();
            foreach (var item in mesPublications)
            {
                LesImages.AddRange(_context.Image_Videos.Where(m => m.IdPublication == item.IdPublication).ToList());
            }
            return View(LesImages);
        }
        public async Task<ActionResult> MembreLien(int? id)
        {
            var mesPublications = _context.Publications.Where(m => m.IdMembre == id).ToList();
            var Liens = new List<Lien>();
            foreach (var item in mesPublications)
            {
                Liens.AddRange(_context.Liens.Where(m => m.IdPublication == item.IdPublication).ToList());
            }
            return View(Liens);
        }
        public async Task<ActionResult> MembreMessage(int? id)
        {
            var mesPublications = _context.Publications.Where(m => m.IdMembre == id).ToList();
            var Messages = new List<Message>();
            foreach (var item in mesPublications)
            {
                Messages.AddRange(_context.Messages.Where(m => m.IdPublication == item.IdPublication).ToList());
            }
            return View(Messages);
        }
        public async Task<ActionResult> SupprimerPublicationAdmin(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var ressource = _context.Ressources.Find(id);
            if (ressource == null)
            {
                return NotFound();
            }
            var publication = _context.Publications.Where(m => m.IdPublication == ressource.IdPublication).FirstOrDefault();
            if (publication == null)
            {
                return NotFound();
            }
            _context.Remove(publication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListeMembre));
        }
        public async Task<ActionResult> SupprimerPublication2Admin(int? id)
        {
            var publication = _context.Publications.Find(id);
            if (publication == null)
            {
                return NotFound();
            }
            _context.Remove(publication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListeMembre));
        }
        public async Task<ActionResult> Reinitialiser(int? id)
        {
            if(id== null)
            {
                return NotFound();
            }
            var membre = _context.Membres.Find(id);
            if (membre != null)
            {
                membre.Report = 0;
                var publications = _context.Publications.Where(p => p.IdMembre == id).ToList();
                if (publications.Any())
                {
                    foreach (var i in publications)
                    {
                        _context.Remove(i);
                    }
                }
                var amis = _context.Amis.Where(a => a.RequestedById == id || a.RequestedToId == id).ToList();
                if (amis.Any())
                {
                    foreach (var item in amis)
                    {
                        _context.Remove(item);
                    }
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListeMembre));
        }
        //logout
        public async Task<ActionResult> Logout()
        {
            HttpContext.Session.Remove("AdministrateurSession");
            return RedirectToAction("SignIn", "Administrateur");
        }
    }
}
