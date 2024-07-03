using Microsoft.EntityFrameworkCore;
using NetAtlas_The_True_Project.Models;

namespace NetAtlas_The_True_Project.Data
{
    public class NetAtlasDbContext : DbContext
    {
        public NetAtlasDbContext(DbContextOptions<NetAtlasDbContext> options) : base(options)
        {
        }
        public DbSet<Membre> Membres { get; set; }
        public DbSet<Administrateur> Administrateurs { get; set; }
        public DbSet<Moderateur> Moderateurs { get; set; }
        public DbSet<Image_Video> Image_Videos { get; set; }
        public DbSet<Lien> Liens { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Ressource> Ressources { get; set; }
        public DbSet<Internaute> Internautes { get; set; }
        public DbSet<AmiMembre> Amis { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Report> Reports { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<Membre>().ToTable("Membre");
            modelBuilder.Entity<Administrateur>().ToTable("Administrateur");
            modelBuilder.Entity<Moderateur>().ToTable("Moderateur");
            modelBuilder.Entity<Publication>().ToTable("Publication");
            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<Lien>().ToTable("Lien");
            modelBuilder.Entity<Image_Video>().ToTable("Image_Video");
            modelBuilder.Entity<Ressource>().ToTable("Ressource");
            modelBuilder.Entity<Internaute>().ToTable("Internaute");
            */

            //modelBuilder.Entity<Publication>().ToTable("Publication");
            //Ami Membre

            modelBuilder.Entity<AmiMembre>()
                 .HasOne(a => a.RequestedBy)
                 .WithMany(b => b.SentFriendRequests)
                 .HasForeignKey(c => c.RequestedById)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AmiMembre>()
                .HasOne(a=>a.RequestedTo)
                .WithMany(b=>b.ReceievedFriendRequests)
                .HasForeignKey(c => c.RequestedToId);

            modelBuilder.Entity<AmiMembre>().ToTable("Ami")
                .HasKey(t => new { t.RequestedById, t.RequestedToId });

            modelBuilder.Entity<Membre>()
            .HasMany<Publication>(g => g.Publications)
            .WithOne(s => s.membre)
            .HasForeignKey(s => s.IdMembre)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Publication>()
                .HasMany<Ressource>(g=>g.Ressources)
                .WithOne(s=>s.Publication)
                .HasForeignKey(m=>m.IdPublication)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Report>()
                .HasKey(r => new { r.IdModerateur, r.IdMembre });
            modelBuilder.Entity<Report>()
                .HasOne(m => m.Moderateur)
                .WithMany(r => r.Reports)
                .HasForeignKey(k => k.IdModerateur);
            modelBuilder.Entity<Report>()
                .HasOne(m => m.Membre)
                .WithMany(r => r.Reports)
                .HasForeignKey(k => k.IdMembre);
        }
    }
}
