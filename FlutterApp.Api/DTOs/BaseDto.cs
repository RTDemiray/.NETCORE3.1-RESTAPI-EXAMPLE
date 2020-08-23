using FlutterApp.Api.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlutterApp.Api.DTOs
{
    public class BaseDto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid RowGuid { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }

        public BaseDto()
        {
            IsActive = false;
            CreatedOn = DateTime.Now;
            RowGuid = Guid.NewGuid();
        }
    }
}
