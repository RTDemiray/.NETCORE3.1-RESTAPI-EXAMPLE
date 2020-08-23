using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FlutterApp.Data.Entities
{
    public class Categories : BaseEntity
    {
        [Required(ErrorMessage = "{0} alanı boş geçilemez!"), MaxLength(50, ErrorMessage = "{0} alanı 50 Karakterden fazla olamaz!")]
        [DisplayName("Kategori Adı")]
        public string Name { get; set; }

        public ICollection<Questions> Questions { get; set; }
    }
}
