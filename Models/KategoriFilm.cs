using System;

namespace FilmMakarasi.Models
{
    public class KategoriFilm
    {
        

        public int FilmId { get; set; }         //scaler
        public Film Film { get; set; }          //referans
        
        public int KategoriId { get; set; }     //scaler
        public Kategori Kategori { get; set; }  //referans

        public string Aciklama { get; set; } =  $"Film {DateTime.Now.ToShortDateString()} tarihinde eklendi";
        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;
    }
}