using FlutterApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlutterApp.Api.DTOs
{
    public class ScoresWithCategoryDto
    {
        public Categories Categories { get; set; }
        public Users Users { get; set; }
    }
}
