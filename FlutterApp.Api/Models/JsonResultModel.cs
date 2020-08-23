using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlutterApp.Api.Models
{
    public class JsonResultModel
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
    }
}
