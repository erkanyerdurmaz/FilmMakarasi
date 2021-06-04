using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using FilmMakarasi.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace FilmMakarasi.Data
{
    public static class SeedData
    {
        public async static void Initialize(IServiceProvider serviceProvider)
        {
            // using (resource)
            // {

            // } //resource gb(çöp toplayıcı) beklenmeden yok edeilir
            using (var context = new FilmMakarasiContext(serviceProvider.GetRequiredService<DbContextOptions<FilmMakarasiContext>>()))
            {
                context.Database.Migrate(); //veritabanını oluştur
                // Filmler tablosunda herhangi bir kayıt varsa...
                if (context.Filmler.Any())
                {
                    return;   // hiç bir şey yapmadan methoddan çık
                }

                var a1 = new Random();
                a1.Next(200);
                a1.Next(100);



                var r = new Random();

                var rDizisi = new Random[]
                {
                    new Random(),  //rDizisi[0]
                    new Random()  //rDizisi[1]
                };

                var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
                roleManager.CreateAsync(new AppRole { Name = "Admin" });
                roleManager.CreateAsync(new AppRole { Name = "Manager" });
                roleManager.CreateAsync(new AppRole { Name = "User" });

                var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
                var admin = new AppUser { UserName = "admin@gmail.com", Email = "admin@gmail.com", EmailConfirmed = true };
                var manager = new AppUser { UserName = "manager@gmail.com", Email = "manager@gmail.com", EmailConfirmed = true };
                var user = new AppUser { UserName = "user@gmail.com", Email = "user@gmail.com", EmailConfirmed = true };

                userManager.CreateAsync(admin, "Erkan.123");
                userManager.CreateAsync(manager, "Erkan.123");
                userManager.CreateAsync(user, "Erkan.123");


                // userManager.AddToRoleAsync(admin,"Admin");
                // userManager.AddToRoleAsync(admin,"Manager");
                userManager.AddToRolesAsync(admin, new List<string> { "Admin", "Manager" });
                userManager.AddToRoleAsync(manager, "Manager");
                userManager.AddToRoleAsync(manager, "User");

                //-------------------Filmler------------------
                var filmler = new Film[]
                {
                    new Film
                    {
                        Ad = "Zindan Adası", Aciklama = "...",
                        Resimler = new List<Resim>()
                        {
                            new Resim { DosyaAdi = "shutter.island.01.jpg" }

                        }
                    },
                    new Film
                    {
                        Ad = "Yüzüklerin Efendisi", Aciklama = "...",
                        Resimler = new List<Resim>
                        {
                            new Resim { DosyaAdi = "27070.jpg" },
                        }
                    },
                    new Film
                    {
                        Ad = "Ölü Gelin", Aciklama = "...",
                        Resimler = new List<Resim>()
                        {
                            new Resim { DosyaAdi = "event-poster-8603781-800x1200.jpg" }

                        },
                    },
                    new Film
                    {
                        Ad = "Recep İvedik 7", Aciklama = "...",
                        Resimler = new List<Resim>()
                        {
                            new Resim { DosyaAdi = "recep_ivedik_6_full_izle_h26871.jpg" }

                        },
                    },

                };

                context.Filmler.AddRange(filmler);

                var kategoriler = new Kategori[]
                {

                    new Kategori {Adi = "Eski Filmler" },                                  //kategoriler[0]
                    new Kategori {Adi = "Bugünkü Filmler "},                              //kategoriler[1]
                    new Kategori {Adi = "Gelecek Filmler "},                             //kategoriler[2]
                  
                };

                context.Kategoriler.AddRange(kategoriler);

                context.KategorilerVeFilmleri.AddRange(
                    new KategoriFilm { Film = filmler[0], Kategori = kategoriler[0] },
                    new KategoriFilm { Film = filmler[1], Kategori = kategoriler[0] },
                    new KategoriFilm { Film = filmler[2], Kategori = kategoriler[0] },
                    new KategoriFilm { Film = filmler[2], Kategori = kategoriler[2] }

                );

                SHA256 sha = new SHA256CryptoServiceProvider();


                context.Kullanicilar.AddRange(
                    new Kullanici { KAdi = "ahmet@gmail.com", Sifre = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes("Aa123.."))), Turu = "Admin", Onay = true },
                    new Kullanici { KAdi = "Mehmet", Sifre = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes("321"))), Turu = "User", Onay = true }
                );
                context.SaveChanges();

                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Çekirdek veriler başarıyla yazıldı");
            }
        }
    }
}