using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FlutterApp.Data.Entities
{
    public class Scores : BaseEntity
    {
        public int Rank { get; set; }
        public int CategoriesId { get; set; }
        public virtual Categories Categories { get; set; }
        public int UsersId { get; set; }
        public virtual Users Users { get; set; }
    }
}
