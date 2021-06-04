using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
namespace FilmMakarasi.Models
{
    public class AppUser : IdentityUser
    {
        public string Aciklama { get; set; }
    }
}