using Microsoft.AspNetCore.Identity;
namespace FilmMakarasi.Models
{
    public class AppRole : IdentityRole
    {
        public string Aciklama { get; set; }
    }
}