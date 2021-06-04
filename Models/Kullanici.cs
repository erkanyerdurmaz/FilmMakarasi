using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FilmMakarasi.Models

{
    public class Kullanici
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı adı girilmelidir")]
        public string KAdi { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre girilmelidir")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        [Display(Name = "Şifre Tekrar")]
        [DataType(DataType.Password)]
        [Compare("Sifre", ErrorMessage = "Şifreler uyuşmuyor")]
        [NotMapped]
        public string SifreTekrar { get; set; }

        public string Turu { get; set; } = "Guest";
        public bool Onay { get; set; }

    }
}