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
using Newtonsoft.Json;

namespace FlutterApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Questions> _repoQuestions;

        public QuestionsController(IMapper mapper, IRepository<Questions> repoQuestions)
        {
            _mapper = mapper;
            _repoQuestions = repoQuestions;
        }

        /// <summary>
        /// Bu endpoint tüm soruları list olarak geri döner.
        /// </summary>
        /// <returns></returns>
        [Consumes("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var questions = await _repoQuestions.ListAsync(asNoTracking: true, includeProperties: "Categories");
            return Ok(_mapper.Map<IEnumerable<QuestionsDto>>(questions));
        }

        /// <summary>
        /// Bu endpoint verilen id sahip soruyu döner.
        /// </summary>
        /// <returns></returns>
        /// /// <response code="404">Verilen id sahip soru bulunamadı!</response>
        [HttpGet("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Questions>))]
        public async Task<IActionResult> GetById(int id)
        {
            var question = await _repoQuestions.GetByIdAsync(id);
            return Ok(_mapper.Map<QuestionsDto>(question));
        }

        /// <summary>
        /// Bu endpoint verilen soru id sahip kategorisini döner.
        /// </summary>
        /// <returns></returns>
        /// <response code="404">Verilen soru id sahip kategori bulunamadı!</response>
        [HttpGet("{id}/category")]
        public async Task<IActionResult> GetWithCategoryByIdAsync(int id)
        {
            var question = await _repoQuestions.GetByIdAsync(id);
            return Ok(_mapper.Map<QuestionsWithCategoryDto>(question));
        }

        /// <summary>
        /// Bu endpoint soruların toplam,aktif,pasif veri sayısını geri döner.
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            TableCountDto tableCountDto = new TableCountDto();
            tableCountDto.TotalCount = await _repoQuestions.CountAsync();
            tableCountDto.ActiveCount = await _repoQuestions.CountAsync(x => x.IsActive);
            tableCountDto.PassiveCount = await _repoQuestions.CountAsync(x => x.IsActive == false);

            return Ok(tableCountDto);
        }

        /// <summary>
        /// Bu endpoint kategori'nin aktiflik durumunu günceller.
        /// </summary>
        /// <returns></returns>
        /// [Consumes("application/json")]
        [HttpPut("IsActive")]
        public async Task<IActionResult> IsActive(QuestionsDto questionsDto)
        {
            if (questionsDto.IsActive)
            {
                questionsDto.IsActive = false;
                await _repoQuestions.UpdateAsync(_mapper.Map<Questions>(questionsDto));
            }
            else
            {
                questionsDto.IsActive = true;
                await _repoQuestions.UpdateAsync(_mapper.Map<Questions>(questionsDto));
            }
            return Ok(questionsDto);
        }

        /// <summary>
        /// Bu endpoint soru ekler.
        /// </summary>
        /// <remarks>
        /// Örnek: Questions json
        ///  
        ///     POST /Todo
        ///     {
        ///        "question": "soru",
        ///        "OptionA": "cevap A",
        ///        "OptionB": "cevap B",
        ///        "OptionC": "cevap C",
        ///        "OptionD": "cevap D",
        ///        "OptionE": "cevap E",
        ///        "TrueOption": "A",
        ///        "categoriesId": 1
        ///     }
        /// 
        /// </remarks>
        /// <param name="questionsDto">Questions json nesnesi</param>
        /// <returns></returns>
        [Consumes("application/json")]
        [HttpPost]
        public async Task<IActionResult> Save(QuestionsDto questionsDto)
        {
            var newQuestion = await _repoQuestions.InsertAsync(_mapper.Map<Questions>(questionsDto));
            return Created(new Uri(Request.Path, UriKind.Relative), newQuestion);
        }

        /// <summary>
        /// Bu endpoint soru günceller.
        /// </summary>
        /// <remarks>
        /// Örnek: Questions json
        ///  
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "question": "soru",
        ///        "OptionA": "cevap A",
        ///        "OptionB": "cevap B",
        ///        "OptionC": "cevap C",
        ///        "OptionD": "cevap D",
        ///        "OptionE": "cevap E",
        ///        "TrueOption": "A",
        ///        "categoriesId": 1
        ///     }
        /// 
        /// </remarks>
        /// <param name="questionsDto">Questions json nesnesi</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(QuestionsDto questionsDto)
        {
            await _repoQuestions.UpdateAsync(_mapper.Map<Questions>(questionsDto));
            return NoContent();
        }

        /// <summary>
        /// Bu endpoint verilen soru id sahip soruyu siler.
        /// </summary>
        /// <returns></returns>
        /// <response code="404">Verilen id sahip soru bulunamadı!</response>
        [Consumes("application/json")]
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Questions>))]
        public async Task<IActionResult> Delete(int id)
        {
            await _repoQuestions.DeleteAsync(id);
            return NoContent();
        }
    }
}