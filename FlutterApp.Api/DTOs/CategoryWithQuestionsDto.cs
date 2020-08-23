using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlutterApp.Api.DTOs
{
    public class CategoryWithQuestionsDto : CategoryDto
    {
        public ICollection<QuestionsDto> Questions { get; set; }
    }
}
