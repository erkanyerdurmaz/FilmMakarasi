using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmMakarasi.Data;
using FilmMakarasi.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FilmMakarasi.Controllers
{
    public class FilmIslemleri : Controller
    {
        private readonly FilmMakarasiContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        private string _dosyaYolu;  //       .../wwwroot/resimler

        public FilmMakarasiContext Context => _context;


        //Yapıcı method, Dependency Injection (Bağımlı enjeksiyon)
        public FilmIslemleri(FilmMakarasiContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;

            _dosyaYolu = Path.Combine(_hostEnvironment.WebRootPath, "resimler");

            if (!Directory.Exists(_dosyaYolu))
            {
                Directory.CreateDirectory(_dosyaYolu);  //ilgili klasör yoksa oluştur
            }
        }

        
        public async Task<IActionResult> Index()
        {
            // ViewBag.KategoriAdi = "Tüm Kategoriler";
            var result = await _context.Filmler.Include(x=>x.Resimler).ToListAsync();
            return View(result);  //index.cshtml
        }

        public async Task<IActionResult> KategorininFilmleri(int id)
        {
            var kategori = await Context.Kategoriler
                                            .Include(x=>x.Filmleri)
                                                .ThenInclude(x=>x.Resimler)
                                            .SingleOrDefaultAsync(x=>x.Id==id);

            // ViewBag.KategoriAdi = kategori.Adi;
            // ViewBag.KategoriId = kategori.Id;
            ViewBag.Kategori = kategori;

            return View("Index", kategori.Filmleri);  
        }

       
        public async Task<IActionResult> Details(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await Context.Filmler.Include(x=>x.Resimler)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

         public async Task<IActionResult> KategorileriniAyarla(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await Context.Filmler.Include(x=>x.Kategorileri)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }
        [HttpGet]
        
        public IActionResult Create(int? id)  //id:null olabilir
        {
            return View(); //form ---model binding(asp-for)
        }

       
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [Bind("Ad,Aciklama,Dosyalar")] Film film) 
        {
            if (ModelState.IsValid)
            {
                foreach (var item in film.Dosyalar)
                {
                    var tamDosyaAdi = Path.Combine(_dosyaYolu, item.FileName);
                
                    using (var dosyaAkisi = new FileStream(tamDosyaAdi, FileMode.Create))
                    {
                        await item.CopyToAsync(dosyaAkisi); //upload
                    }

                    film.Resimler.Add( new Resim{DosyaAdi=item.FileName} ); 
                }

                if (id !=null ) film.Kategorileri.Add( await  Context.Kategoriler.FindAsync(id)    );

                Context.Add(film);
                await Context.SaveChangesAsync();

                TempData["mesaj"] = $"{film.Ad} film başarılı bir şekilde eklendi";

                if (id !=null )  return RedirectToAction(nameof(KategorininFilmleri), new {id = id});
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await Context.Filmler.Include(x=>x.Resimler).SingleOrDefaultAsync(x=>x.Id==id); 

            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

       
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Aciklama")] Film film)
        {
            if (id != film.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Context.Update(film);
                    await Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.Id))
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
            return View(film);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await Context.Filmler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await Context.Filmler.Include(x=>x.Resimler).SingleOrDefaultAsync(x=>x.Id==id);
            Context.Filmler.Remove(film);
            try
            {
                await Context.SaveChangesAsync();         
                
                foreach (var item in film.Resimler)
                {
                    System.IO.File.Delete(    Path.Combine(_dosyaYolu,item.DosyaAdi)       );
                }


                TempData["mesaj"] = $"{film.Ad} filmi başarılı bir şekilde silindi";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["mesaj"] = $"{film.Ad} filmi silinemedi. Hata Kodu:1584";
                return RedirectToAction(nameof(Index));
            }
            catch(IOException)
            {
                TempData["mesaj"] = $"{film.Ad} filminin resimleri silinirken dosya okuma-yazma hatası oluştu. Hata Kodu:1585";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> ResimSil(int id)
        {
            var resim =await Context.Resimler.FindAsync(id); //id'si verilen resim bulundu
            Context.Resimler.Remove(resim); //bulduğumuz resim varlığını Resimler listesinden kaldırdık
            await Context.SaveChangesAsync(); //Bu değişiklikleri veritabanına yansıttık

            System.IO.File.Delete( Path.Combine(_dosyaYolu, resim.DosyaAdi));

            return RedirectToAction(nameof(Edit),new {id=resim.FilmiId});
        }

        [HttpPost]
        public async Task<IActionResult> ResimEkle(Film film)
        {
            var resimEklenecekFilm = await Context.Filmler.FindAsync(film.Id);
            try
            {
                foreach (var item in film.Dosyalar)
                {
                    var tamDosyaAdi = Path.Combine(_dosyaYolu, item.FileName);
                
                    using (var dosyaAkisi = new FileStream(tamDosyaAdi, FileMode.Create))
                    {
                        await item.CopyToAsync(dosyaAkisi); //server'a upload
                    }

                    resimEklenecekFilm.Resimler.Add( new Resim{DosyaAdi=item.FileName} ); 
                }
                await Context.SaveChangesAsync();

                return RedirectToAction(nameof(Edit),new {id=film.Id});
            }
            catch (System.Exception)
            {
                TempData["mesaj"]="Lütfen önce yüklenecek dosyaları seçiniz";
                return RedirectToAction(nameof(Edit),new {id=film.Id});
            }

            
        }

        private bool FilmExists(int id)
        {
            return Context.Filmler.Any(e => e.Id == id);
        }
    }
}
