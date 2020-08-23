using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlutterApp.Api.DTOs
{
    public class QuestionsWithCategoryDto : QuestionsDto
    {
        public CategoryDto Category { get; set; }
    }
}
