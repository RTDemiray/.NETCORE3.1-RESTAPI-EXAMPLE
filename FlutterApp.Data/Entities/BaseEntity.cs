using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FlutterApp.Data.Entities
{
    public class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid RowGuid { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }

        public BaseEntity()
        {
            IsActive = false;
            CreatedOn = DateTime.Now;
            RowGuid = Guid.NewGuid();
        }
    }
}
