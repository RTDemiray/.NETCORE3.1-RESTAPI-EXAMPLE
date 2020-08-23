using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlutterApp.Api.DTOs
{
    public class DataCountDto
    {
        public int CategoriesCount { get; set; }
        public int QuestionsCount { get; set; }
        public int UsersCount { get; set; }
        public int ScoresCount { get; set; }
    }
}
