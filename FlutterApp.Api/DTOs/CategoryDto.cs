using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlutterApp.Api.DTOs
{
    public class CategoryDto : BaseDto
    {
        [Required(ErrorMessage = "{0} alanı boş geçilemez!"), MaxLength(50, ErrorMessage = "{0} alanı 50 Karakterden fazla olamaz!")]
        [DisplayName("Kategori Adı")]
        public string Name { get; set; }
    }
}
