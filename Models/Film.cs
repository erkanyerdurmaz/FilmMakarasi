using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace FilmMakarasi.Models
{
    public class Film
    {
        public Film()
        {
            Resimler = new List<Resim>(); //null referance hatası almamak için
        }

        //[Key]
        public int Id { get; set; }  //Varsayılan olarak PK

        [StringLength(60, ErrorMessage="Film adları en fazla 60 karakter olabilir")]
        [Required (ErrorMessage="{0} alanı boş bırakılamaz")]
        [Display(Name="İsim")]
        public string Ad { get; set; }


        [Display(Name="Açıklama")]
        public string Aciklama { get; set; }

        [Required]
        [NotMapped]  //veritabanına yansıMAMAsını sağlar
        public IFormFile[] Dosyalar { get; set; }  //Resmin asıl yer kaplayan dosyalarını tutacağız



        //-------------------İlişkileri (Gezinti Özellikleri)-------------------------


        public List<Resim> Resimler { get; set; }  //birden fazla resim bağlayabileceğimiz anlamına gelir
        public List<Kategori> Kategorileri { get; set; } = new List<Kategori>(); //birden fazla kategori bağlayabileceğimiz anlamına gelir

        public List<KategoriFilm> KategoriFilmler { get; set; }   //  -1------------N-> 
    }
}