using System.Collections.Generic;

namespace FilmMakarasi.Models
{
    public class Kategori
    {
        //[Key]
        public int Id { get; set; } //PK
        public string Adi { get; set; }
        public string Aciklama { get; set; }

        //-------------- İlişkileri (Gezinti Özellikleri)-------------

        public List<Film> Filmleri { get; set; } = new List<Film>();  

        public List<KategoriFilm> KategoriFilmler { get; set; }   
    }
}