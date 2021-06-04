using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmMakarasi.Models
{
    public class Resim
    {
        public int Id { get; set; } //PK
        public string DosyaAdi { get; set; }

        //-------------------ilişkileri (Gezinti Özellikleri)--------------
    
        public int FilmiId { get; set; } //scaler
        [Required]
        public Film Filmi { get; set; } //referans  //bir tane bağlayabileceğimiz anlamına gelir
    }
}