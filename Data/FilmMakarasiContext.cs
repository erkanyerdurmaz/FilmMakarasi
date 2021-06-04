using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FilmMakarasi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FilmMakarasi.Data
{
    public class FilmMakarasiContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public FilmMakarasiContext(DbContextOptions<FilmMakarasiContext> options)
            : base(options)
        {
        }

        //ORM  ---> Entity Framework (ef core 5.0)



        public DbSet<Film> Filmler { get; set; }

        //Resim+Resim...= Resimler(liste) C#  <----------------DbContext---------->Resimler(Tablo) SQLite
        public DbSet<Resim> Resimler { get; set; }

        //Kategori+Kategori...= Kategoriler(liste) C# (Add,remove,where) <----------------DbContext---------->Kategoriler(Tablo) SQLite
        public DbSet<Kategori> Kategoriler { get; set; }

        public DbSet<Kullanici> Kullanicilar { get; set; }




        public DbSet<KategoriFilm> KategorilerVeFilmleri { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //FluentAPI

            modelBuilder.Entity<Film>()
                //       <---N------------------------N---> Kategori
                .HasMany(x => x.Kategorileri)
                .WithMany(x => x.Filmleri)


                .UsingEntity<KategoriFilm>(
                    j => j.HasOne(x => x.Kategori).WithMany(x => x.KategoriFilmler).HasForeignKey(x => x.KategoriId),   //  1----N
                    j => j.HasOne(x => x.Film).WithMany(x => x.KategoriFilmler).HasForeignKey(x => x.FilmId),           //  1----N
                    j =>
                    {
                        j.HasKey(x => new { x.FilmId, x.KategoriId });  //Birleştirilmiş Id : Composite Key

                    }
                );



        }
    }
}
