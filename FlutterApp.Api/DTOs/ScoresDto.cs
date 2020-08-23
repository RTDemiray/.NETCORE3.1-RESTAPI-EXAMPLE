using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlutterApp.Api.DTOs
{
    public class ScoresDto : BaseDto
    {
        [Required(ErrorMessage = "Puan alanı boş geçilemez!")]
        public int Rank { get; set; }
        [Required(ErrorMessage = "Kullanıcı ismi boş geçilemez!"), MaxLength(60, ErrorMessage = "Kullanıcı ismi 60 karakterden fazla olamaz!")]
        public string UserName { get; set; }
        public int CategoriesId { get; set; }
    }
}
