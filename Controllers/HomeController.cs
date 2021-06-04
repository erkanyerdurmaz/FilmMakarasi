using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using FilmMakarasi.Models;
using FilmMakarasi.Data;
using Microsoft.EntityFrameworkCore;

namespace FilmMakarasi.Controllers
{
    public class HomeController : Controller
    {
        private readonly FilmMakarasiContext _context;

        public HomeController(FilmMakarasiContext context)
        {
            
            _context = context;
        }


        public async Task<IActionResult> Index()
        {           
          var result = await _context.Filmler.Include(x=> x.Resimler).ToListAsync();
            return View(result);
        }    
                public async Task<IActionResult> FilmElestirisi(int id)
        {           
          var result = await _context.Filmler.Include(x=> x.Resimler).FirstOrDefaultAsync(x=> x.Id == id);
        
            return View(result);
        }         


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        

 
        
    }

}
