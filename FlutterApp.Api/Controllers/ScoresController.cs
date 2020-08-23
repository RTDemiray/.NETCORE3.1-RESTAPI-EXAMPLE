using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlutterApp.Api.DTOs;
using FlutterApp.Api.Filters;
using FlutterApp.Core.IRepositories;
using FlutterApp.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlutterApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoresController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Scores> _repoScores;

        public ScoresController(IMapper mapper, IRepository<Scores> repoScores)
        {
            _mapper = mapper;
            _repoScores = repoScores;
        }

        /// <summary>
        /// Bu endpoint tüm skorları list olarak geri döner.
        /// </summary>
        /// <returns></returns>
        [Consumes("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Scores = await _repoScores.ListAsync(asNoTracking: true, includeProperties: "Categories,Users");
            return Ok(_mapper.Map<IEnumerable<ScoresDto>>(Scores));
        }

        /// <summary>
        /// Bu endpoint verilen id sahip skoru döner.
        /// </summary>
        /// <returns></returns>
        /// /// <response code="404">Verilen id sahip skor bulunamadı!</response>
        [Consumes("application/json")]
        [HttpGet("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Scores>))]
        public async Task<IActionResult> GetById(int id)
        {
            var question = await _repoScores.GetByIdAsync(id);
            return Ok(_mapper.Map<ScoresDto>(question));
        }

        /// <summary>
        /// Bu endpoint verilen skor id sahip kategorisini döner.
        /// </summary>
        /// <returns></returns>
        /// <response code="404">Verilen skor id sahip kategori bulunamadı!</response>
        [Consumes("application/json")]
        [HttpGet("{id}/category")]
        public async Task<IActionResult> ScoresWithCategory(int id)
        {
            var score = await _repoScores.GetByIdAsync(id);
            return Ok(_mapper.Map<IEnumerable<ScoresWithCategoryDto>>(score));
        }

        /// <summary>
        /// Bu endpoint skorların toplam,aktif,pasif veri sayısını geri döner.
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            TableCountDto tableCountDto = new TableCountDto();
            tableCountDto.TotalCount = await _repoScores.CountAsync();
            tableCountDto.ActiveCount = await _repoScores.CountAsync(x => x.IsActive);
            tableCountDto.PassiveCount = await _repoScores.CountAsync(x => x.IsActive == false);

            return Ok(tableCountDto);
        }

        /// <summary>
        /// Bu endpoint skor ekler.
        /// </summary>
        /// <remarks>
        /// Örnek: Scores json
        ///  
        ///     POST /Todo
        ///     {
        ///        "rank": 50,
        ///        "userName": "Recep",
        ///        "categoriesId": 1
        ///     }
        /// 
        /// </remarks>
        /// <param name="ScoresDto">Scores json nesnesi</param>
        /// <returns></returns>
        [Consumes("application/json")]
        [HttpPost]
        public async Task<IActionResult> Save(ScoresDto ScoresDto)
        {
            var newQuestion = await _repoScores.InsertAsync(_mapper.Map<Scores>(ScoresDto));
            return Created(new Uri(Request.Path, UriKind.Relative), newQuestion);
        }


        /// <summary>
        /// Bu endpoint verilen id sahip skoru günceller.
        /// </summary>
        /// <remarks>
        /// Örnek: Scores json
        ///  
        ///     POST /Todo
        ///     {
        ///        "id": "2"
        ///        "rank": 50,
        ///        "userName": "Recep",
        ///        "categoriesId": 1
        ///     }
        /// 
        /// </remarks>
        /// <param name="ScoresDto">Scores json nesnesi</param>
        /// <returns></returns>
        [Consumes("application/json")]
        [HttpPut]
        public async Task<IActionResult> Update(ScoresDto ScoresDto)
        {
            await _repoScores.UpdateAsync(_mapper.Map<Scores>(ScoresDto));
            return NoContent();
        }

        /// <summary>
        /// Bu endpoint verilen skor id sahip skoru siler.
        /// </summary>
        /// <returns></returns>
        /// <response code="404">Verilen id sahip skor bulunamadı!</response>
        [Consumes("application/json")]
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Scores>))]
        public async Task<IActionResult> Delete(int id)
        {
            await _repoScores.DeleteAsync(id);
            return NoContent();
        }
    }
}