using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlutterApp.Api.Models
{
    public class AuthenticateModel
    {
        [Required(ErrorMessage = "Kullanıcı adı alanı boş geçilemez!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş geçilemez!")]
        public string Password { get; set; }
    }
}
