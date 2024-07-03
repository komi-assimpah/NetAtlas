using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetAtlas_The_True_Project.Data;
using NetAtlas_The_True_Project.Models;
using Newtonsoft.Json;

namespace NetAtlas_The_True_Project.Controllers
{
    public class ModerateurController : Controller
    {
        private readonly NetAtlasDbContext _context;

        public ModerateurController(NetAtlasDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        // GET: Moderateur/SignIn
        //Page de connexion
        public IActionResult SignIn()
        {
            return View();
        }

        // POST: Moderateur/SignIn
        //Connexion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn([Bind("IdModerateur,UserName,Password")] Moderateur moderateur)
        {
            if (!ModelState.IsValid)
            {
                var UnModo = await _context.Moderateurs.ToListAsync();
                foreach (var i in UnModo)
                {
                    if (moderateur.UserName.Equals(i.UserName))
                    {
                        if (moderateur.Password.Equals(i.Password))
                        {
                            //recupérer la valeur de l'id du membre 
                            moderateur.IdModerateur = _context.Moderateurs.Where(i => i.UserName.Equals(moderateur.UserName)).FirstOrDefault().IdModerateur;
                            //Mettre les informations de membre dans une clé de session
                            HttpContext.Session.SetString("ModerateurSession", JsonConvert.SerializeObject(moderateur));
                            return RedirectToAction(nameof(ListeMembre));
                        }
                    }
                }
                return NotFound();
            }
            return View(moderateur);
        }
        public async Task<IActionResult> ListeMembre()
        {
            return View(await _context.Membres.ToListAsync());
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
        public async Task<ActionResult> Avertir(int? id)
        {
            var moderateur = JsonConvert.DeserializeObject<Moderateur>(HttpContext.Session.GetString("ModerateurSession"));
            if (id == null)
            {
                return NotFound();
            }
            var LaRessource = _context.Ressources.Find(id);
            LaRessource.IsThisReported = isReport.isReport;
            _context.Update(LaRessource);
            _context.SaveChanges();
            if (LaRessource == null)
            {
                return NotFound();
            }
            var Lapublication = _context.Publications.Where(p => p.IdPublication == LaRessource.IdPublication).FirstOrDefault();
            if (Lapublication == null)
            {
                return NotFound();
            }
            var membre = _context.Membres.Where(m=>m.IdMembre==Lapublication.IdMembre).FirstOrDefault();
            if(membre == null)
            {
                return NotFound();
            }
            membre.Report = membre.Report + 1;
            _context.Update(membre);
            _context.SaveChanges();
            var LeReport = new Report();
            LeReport.IdModerateur = moderateur.IdModerateur;
            LeReport.IdMembre = membre.IdMembre;
            LeReport.MessageAvertissement = "La publication du " + Lapublication.DatePublication+ "n'est pas conforme à nos conditions d'utilisation. NombreDeReport : " + membre.Report;
            _context.Add(LeReport);
            _context.SaveChanges();

            return RedirectToAction(nameof(ListeMembre));
        }
        //logout
        public async Task<ActionResult> Logout()
        {
            HttpContext.Session.Remove("ModerateurSession");
            return RedirectToAction("SignIn", "Moderateur");
        }
    }
}
