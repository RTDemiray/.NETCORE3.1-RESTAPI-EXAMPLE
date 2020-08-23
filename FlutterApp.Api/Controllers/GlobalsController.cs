using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlutterApp.Api.DTOs;
using FlutterApp.Core.IRepositories;
using FlutterApp.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlutterApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GlobalsController : ControllerBase
    {
        public readonly IRepository<Categories> _repoCategories;
        public readonly IRepository<Questions> _repoQuestions;
        public readonly IRepository<Scores> _repoScores;
        public readonly IRepository<Users> _repoUsers;

        public GlobalsController(IRepository<Categories> repoCategories, IRepository<Questions> repoQuestions, IRepository<Scores> repoScores, IRepository<Users> repoUsers)
        {
            _repoCategories = repoCategories;
            _repoQuestions = repoQuestions;
            _repoScores = repoScores;
            _repoUsers = repoUsers;
        }

        /// <summary>
        /// Bu endpoint categories,questions,users,scores bilgilerinin toplam veri sayısını geri döner.
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        public async Task<IActionResult> DataCount()
        {
            DataCountDto dataCountDto = new DataCountDto();

            dataCountDto.CategoriesCount = await _repoCategories.CountAsync();
            dataCountDto.QuestionsCount = await _repoQuestions.CountAsync();
            dataCountDto.UsersCount = await _repoUsers.CountAsync();
            dataCountDto.ScoresCount = await _repoScores.CountAsync();
            return Ok(dataCountDto);
        }
    }
}
