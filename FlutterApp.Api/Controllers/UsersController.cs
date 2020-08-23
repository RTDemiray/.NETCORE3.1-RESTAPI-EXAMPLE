using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlutterApp.Api.Models;
using FlutterApp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlutterApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        JsonResultModel jModel = new JsonResultModel();
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Bu endpoint ile login işlemi yapılır ve token üretilir.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
            {
                jModel.IsSuccess = false;
                jModel.Message = "Kullanıcı adı veya şifre hatalı!";
                return BadRequest(jModel);
            }
            jModel.IsSuccess = true;
            jModel.Message = "Yönlendiriliyorsunuz...";
            jModel.Token = user.Token;
            return Ok(jModel);
        }

        /// <summary>
        /// Bu endpoint ile api kullanıcılarının listesi döner.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}