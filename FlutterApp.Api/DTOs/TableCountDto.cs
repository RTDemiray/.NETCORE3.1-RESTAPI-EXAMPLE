using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlutterApp.Api.DTOs
{
    public class TableCountDto
    {
        public int ActiveCount { get; set; }
        public int PassiveCount { get; set; }
        public int TotalCount { get; set; }
    }
}
