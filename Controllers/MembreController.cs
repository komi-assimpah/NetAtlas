using Microsoft.AspNetCore.Mvc;
using NetAtlas_The_True_Project.Data;
using NetAtlas_The_True_Project.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;

namespace NetAtlas_The_True_Project.Controllers
{
    public class MembreController : Controller
    {
        private readonly NetAtlasDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MembreController(NetAtlasDbContext context,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }
        //page principale
        public IActionResult Index()
        {
            //recupérer les données de session
            var membre = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if(membre == null)
            {
                return Unauthorized();
            }
            ViewBag.UserNameRecup=membre.UserName;
            return View();
        }
        //page de publication vidéo/image
        public IActionResult PublicationImVid()
        {
            //recupérer les données de session
            var membre = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            ViewBag.UserNameRecup = membre.UserName;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublicationImVid(string Description, [Bind("IdRessource,Description,Type,Image_VideoFile")] Image_Video imagevideo)
        {
            var membres = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if(membres == null || Description.Equals(null))
            {
                return NotFound();
            }
            ViewBag.UserNameRecup = membres.UserName;
            var publication = new Publication();
            publication.DatePublication=DateTime.Now;
            publication.IdMembre = membres.IdMembre;
            /*if(publication.membre.IdMembre == null)
            {
                return NotFound();
            }*/
            _context.Publications.Update(publication);
            await _context.SaveChangesAsync();

            if (!ModelState.IsValid)
            {
                if (publication.IdPublication.Equals(null))
                {
                    return NotFound();
                }
                //enregistrer l'image dans le wwwRoot
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imagevideo.Image_VideoFile.FileName);
                string extension = Path.GetExtension(imagevideo.Image_VideoFile.FileName);
                imagevideo.ImageName=fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                imagevideo.Description = Description;
                imagevideo.Taille = imagevideo.Image_VideoFile.Length;
                imagevideo.IdPublication = publication.IdPublication;
                if (extension.Equals(null) || extension.Equals(""))
                {
                    return NotFound();
                }
                else if (extension.Equals(".mp4"))
                {
                    imagevideo.Type = "Video";
                }
                else
                {
                    imagevideo.Type = "Image";
                }
                string path = Path.Combine(wwwRootPath+"/Image", fileName);
                using(var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imagevideo.Image_VideoFile.CopyToAsync(fileStream);
                }
                //inserer l'image dans la base de données
                _context.Add(imagevideo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imagevideo);
        }
        //page de publication vidéo/image par post
        //publication Lien
        //get
        public async Task<IActionResult> PublicationLien()
        {
            var membres = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            ViewBag.UserNameRecup = membres.UserName;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublicationLien([Bind("IdRessource,AdresseUrl")] Lien unLien)
        {
            var membres = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if (membres == null || unLien.AdresseUrl==null)
            {
                return NotFound();
            }
            ViewBag.UserNameRecup = membres.UserName;
            var publication = new Publication();
            publication.DatePublication = DateTime.Now;
            publication.IdMembre = membres.IdMembre;
            _context.Publications.Update(publication);
            await _context.SaveChangesAsync();

            if (!ModelState.IsValid)
            {
                if (publication.IdPublication.Equals(null))
                {
                    return NotFound();
                }
                //affecter le idPublication
                unLien.IdPublication = publication.IdPublication;
                //inserer l'image dans la base de données
                _context.Add(unLien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(unLien);
        }
        //publication Message
        //get
        public async Task<IActionResult> PublicationMessage()
        {
            var membres = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            ViewBag.UserNameRecup = membres.UserName;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublicationMessage([Bind("IdRessource,MessageSend")] Message unMessage)
        {
            var membres = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if (membres == null || unMessage.MessageSend == null)
            {
                return NotFound();
            }
            ViewBag.UserNameRecup = membres.UserName;
            var publication = new Publication();
            publication.DatePublication = DateTime.Now;
            publication.IdMembre = membres.IdMembre;
            _context.Publications.Update(publication);
            await _context.SaveChangesAsync();

            if (!ModelState.IsValid)
            {
                if (publication.IdPublication.Equals(null))
                {
                    return NotFound();
                }
                //affecter le idPublication
                unMessage.IdPublication = publication.IdPublication;
                //inserer l'image dans la base de données
                _context.Add(unMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(unMessage);
        }
        //Index Ami
        public async Task<IActionResult> IndexAmi()
        {
            return View();
        }
        //Tous les membres qu'il peut ajouter
        public async Task<ActionResult> LesMembresAJ()
        {
            //recupérer les données de session
            var membre = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if (membre == null)
            {
                return Unauthorized();
            }
            ViewBag.UserNameRecup = membre.UserName;
            ViewBag.amis = new List<Membre>();
                /*if (TempData["MembreEnv"] == null)
                {*/
                var liste = _context.Amis.Where(a => a.RequestedById == membre.IdMembre).ToList();
                var liste2 = new List<Membre>();
                foreach (var i in liste)
                {
                    liste2.AddRange(_context.Membres.Where(m => m.IdMembre == i.RequestedToId).ToList());
                }
                ViewBag.amis = liste2;
            /*}*/
            //else
            //{
                //ViewBag.amis = JsonConvert.DeserializeObject<List<Membre>>(TempData["MembreEnv"].ToString());
            //}
            List <Membre> listAmi = new List<Membre>();
            List<Membre> nonAmi = new List<Membre>();
            var tousLesmembresSaufMoi = _context.Membres.Where(m => m.IdMembre != membre.IdMembre).ToList();
            var ami = _context.Amis.Where(m=>m.RequestedById==membre.IdMembre && m.FriendRequestFlag==FriendRequestFlag.Approved).ToList();
            var amiViewBag = _context.Amis.Where(a => a.RequestedById == membre.IdMembre && a.FriendRequestFlag == FriendRequestFlag.None).ToList();
            List<Membre> AmiForViewBag = new List<Membre>();
            foreach (var amis in amiViewBag)
            {
                AmiForViewBag.AddRange(_context.Membres.Where(m => m.IdMembre == amis.RequestedById).ToList());
            }
            //TempData["Membre_amis"] = AmiForViewBag;
            ViewBag.MesAmi = AmiForViewBag;
            if (!ami.Any())
            {
                return View(tousLesmembresSaufMoi);
            }
            else
            {
                foreach(var item1 in ami)
                {
                    listAmi.Add(_context.Membres.Where(m => m.IdMembre == item1.RequestedToId).FirstOrDefault());
                }
                if (listAmi.Count() == 0)
                {
                    return NotFound();
                }
                foreach(var item in tousLesmembresSaufMoi)
                {
                    if (!(listAmi.Contains(item)))
                    {
                        nonAmi.Add(item);
                    }
                }
            }
            return View(nonAmi);
            
        }
        //Inviter ami
        //GET/id
        public async Task<ActionResult> Inviter(int id)
        {
            //recupérer les données de session
            var membre = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if(membre == null)
            {
                return Unauthorized();
            }
            ViewBag.UserNameRecup = membre.UserName;

            var membreRecup = _context.Membres.Where(m=>m.IdMembre==membre.IdMembre).FirstOrDefault();
            var ami = _context.Membres.Where(m=>m.IdMembre==id).FirstOrDefault();
            if (ami == null)
            {
                return NotFound();
            }
            var amiMembre = new AmiMembre();
            amiMembre.RequestedById = membre.IdMembre;
            amiMembre.RequestedToId= id;
            _context.Add(amiMembre);
            await _context.SaveChangesAsync();
            /*var liste=_context.Amis.Where(a=>a.RequestedById==membre.IdMembre).ToList();
            var liste2 = new List<Membre>();
            foreach (var i in liste) {
                liste2.AddRange(_context.Membres.Where(m => m.IdMembre == i.RequestedToId).ToList());
            }
            TempData["MembreEnv"] = JsonConvert.SerializeObject(liste2, Formatting.Indented,
    new JsonSerializerSettings()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    });*/
            return RedirectToAction(nameof(LesMembresAJ));
        }

        public async Task<ActionResult> RequeteAmi()
        {
            var membre = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if(membre == null)
            {
                return Unauthorized();
            }
            ViewBag.UserNameRecup = membre.UserName;
            List<Membre>Requete = new List<Membre>();
            var requete = _context.Amis.Where(m => m.RequestedToId==membre.IdMembre).ToList();
            foreach(var item in requete)
            {
                Requete.AddRange(_context.Membres.Where(m => m.IdMembre == item.RequestedById && item.FriendRequestFlag.Equals(FriendRequestFlag.None)).ToList());
            }
            return View(Requete);

        }
        public async Task<ActionResult> Annuler(int id)
        {
            var membre = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if (membre == null)
            {
                return Unauthorized();
            }
            ViewBag.UserNameRecup = membre.UserName;
            _context.Remove(_context.Amis.Where(m => m.RequestedById == membre.IdMembre && m.RequestedToId == id).FirstOrDefault());
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(LesMembresAJ));

        }
        public async Task<ActionResult> Accepter(int id)
        {
            var membre = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if (membre == null)
            {
                return Unauthorized();
            }
            ViewBag.UserNameRecup = membre.UserName;
            var Ami = _context.Amis.Where(m=>m.RequestedById==id && m.RequestedToId==membre.IdMembre).FirstOrDefault();
            Ami.FriendRequestFlag = FriendRequestFlag.Approved;
            _context.Update(Ami);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(RequeteAmi));

        }
        public async Task<ActionResult> Refuser(int id)
        {
            var membre = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if (membre == null)
            {
                return Unauthorized();
            }
            ViewBag.UserNameRecup = membre.UserName;
            _context.Remove(_context.Amis.Where(m => m.RequestedById == id && m.RequestedToId == membre.IdMembre).FirstOrDefault());
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(RequeteAmi));

        }
        public async Task<ActionResult> ListeAmi()
        {
            //recupérer les données de session
            var membre = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if (membre == null)
            {
                return Unauthorized();
            }
            ViewBag.UserNameRecup = membre.UserName;
            var ami = _context.Amis.Where(m => m.RequestedById == membre.IdMembre && m.FriendRequestFlag == FriendRequestFlag.Approved).ToList();
            List <Membre> MesAmi = new List<Membre>();
            foreach(var item in ami)
            {
                MesAmi.AddRange(_context.Membres.Where(m=>m.IdMembre==item.RequestedToId).ToList());
            }
            return View(MesAmi);
        }
        public async Task<ActionResult> SupprimerAmi(int id)
        {
            var membre = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if (membre == null)
            {
                return Unauthorized();
            }
            ViewBag.UserNameRecup = membre.UserName;
            var thisAmi = _context.Amis.Where(m=>m.RequestedById==membre.IdMembre && m.RequestedToId==id).FirstOrDefault();
            _context.Amis.Remove(thisAmi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListeAmi));
        }
        public async Task<ActionResult> ListePublicationImgVid()
        {
            var membre = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if (membre == null)
            {
                return Unauthorized();
            }
            ViewBag.UserNameRecup = membre.UserName;
            var mesPublications = _context.Publications.Where(m => m.IdMembre == membre.IdMembre).ToList();
            var LesImages = new List<Image_Video>();
            foreach(var item in mesPublications)
            {
                LesImages.AddRange(_context.Image_Videos.Where(m=>m.IdPublication==item.IdPublication).ToList());
            }
            return View(LesImages);
        }
        public async Task<ActionResult> ListePublicationMessage()
        {
            var membre = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if (membre == null)
            {
                return Unauthorized();
            }
            ViewBag.UserNameRecup = membre.UserName;
            var mesPublications = _context.Publications.Where(m => m.IdMembre == membre.IdMembre).ToList();
            var LesMessages = new List<Message>();
            foreach (var item in mesPublications)
            {
                LesMessages.AddRange(_context.Messages.Where(m => m.IdPublication == item.IdPublication).ToList());
            }
            return View(LesMessages);
        }
        public async Task<ActionResult> ListePublicationLien()
        {
            var membre = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if (membre == null)
            {
                return Unauthorized();
            }
            ViewBag.UserNameRecup = membre.UserName;
            var mesPublications = _context.Publications.Where(m => m.IdMembre == membre.IdMembre).ToList();
            var Liens = new List<Lien>();
            foreach (var item in mesPublications)
            {
                Liens.AddRange(_context.Liens.Where(m => m.IdPublication == item.IdPublication).ToList());
            }
            return View(Liens);
        }
        //supprimer une publication
        public async Task<ActionResult> SupprimerPublication(int? id)
        {
            if(id== null)
            {
                return RedirectToAction(nameof(Index));
            }
            var membre = JsonConvert.DeserializeObject<Membre>(HttpContext.Session.GetString("MembreSession"));
            if (membre == null)
            {
                return Unauthorized();
            }
            ViewBag.UserNameRecup = membre.UserName;
            var ressource = _context.Ressources.Find(id);
            if(ressource == null)
            {
                return NotFound();
            }
            var publication = _context.Publications.Where(m => m.IdPublication == ressource.IdPublication && m.IdMembre == membre.IdMembre).FirstOrDefault();
            if (publication == null)
            {
                return NotFound();
            }
            _context.Remove(publication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        //logout
        public async Task<ActionResult> Logout()
        {
            HttpContext.Session.Remove("MembreSession");
            return RedirectToAction("SignIn","Internaute");
        }
        
    }
}
