using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FlutterApp.Data.Entities
{
    public class Questions : BaseEntity
    {
        [Required(ErrorMessage = "Soru alanı boş geçilemez!"), MaxLength(200, ErrorMessage = "200 Karakterden fazla olamaz!")]
        public string Question { get; set; }
        [Required(ErrorMessage = "A seçeneği boş geçilemez!")]
        public string OptionA { get; set; }
        [Required(ErrorMessage = "B seçeneği boş geçilemez!")]
        public string OptionB { get; set; }
        [Required(ErrorMessage = "C seçeneği boş geçilemez!")]
        public string OptionC { get; set; }
        [Required(ErrorMessage = "D seçeneği boş geçilemez!")]
        public string OptionD { get; set; }
        [Required(ErrorMessage = "E seçeneği boş geçilemez!")]
        public string OptionE { get; set; }
        [Required(ErrorMessage = "Doğru seçenek boş geçilemez!"), MaxLength(1,ErrorMessage = "1 Karakterden fazla olamaz!")]
        public string TrueOption { get; set; }
        [Required(ErrorMessage = "Kategori alanı boş geçilemez!")]
        public virtual Categories Categories { get; set; }
        public int CategoriesId { get; set; }
    }
}
