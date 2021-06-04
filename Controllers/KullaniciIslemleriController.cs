using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FilmMakarasi.Data;
using FilmMakarasi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmMakarasi.Controllers
{

    public class KullaniciIslemleriController : Controller

    {
        private readonly FilmMakarasiContext _context;
        public KullaniciIslemleriController(FilmMakarasiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Giris(Kullanici kullanici)
        {
            SHA256 sha = new SHA256CryptoServiceProvider();
            var passHash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(kullanici.Sifre)));
            var girisYapanKullanici = await _context.Kullanicilar.Where(x => x.KAdi == kullanici.KAdi && x.Sifre == passHash).SingleOrDefaultAsync();

            if (girisYapanKullanici != null)
            {
                //bilgilerini session'lara yaz
                HttpContext.Session.SetString("KAdi", girisYapanKullanici.KAdi);
                HttpContext.Session.SetString("Turu", girisYapanKullanici.Turu);
                TempData["Mesaj"] = $"\"{girisYapanKullanici.KAdi}\" kullanıcısı başarıyla giriş yaptı. İşlemlerinize \"{girisYapanKullanici.Turu}\" yetkisi ile devam edebilirsiniz.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Mesaj"] = "Geçersiz kullanıcı adı veya şifre!";
                return View();
            }
            return View();
        }
        [HttpGet]
        public IActionResult Cikis()
        {
            HttpContext.Session.Clear();
            TempData["Mesaj"] = "Geçerli oturum başarıyla sonlandırıldı.";
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Detay()
        {
            TempData["Mesaj"] = "Yapım aşamasında...";
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Kayit()
        {

            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Kayit([Bind("KAdi,Sifre")] Kullanici kullanici)
        {
            SHA256 sha = new SHA256CryptoServiceProvider();

            kullanici.Sifre = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(kullanici.Sifre)));
            await _context.Kullanicilar.AddAsync(kullanici);        //overposting attack ???

            // var passHash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(kullanici.Sifre)));
            //await _context.Kullanicilar.AddAsync(new Kullanici { KAdi = kullanici.KAdi, Sifre = passHash });

            await _context.SaveChangesAsync();
            TempData["Mesaj"] = "Kullanıcı başarıyla kaydedildi";
            return RedirectToAction("Index", "Home");
        }


    }
}