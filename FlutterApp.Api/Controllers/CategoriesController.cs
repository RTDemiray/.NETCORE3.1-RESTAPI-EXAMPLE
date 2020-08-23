using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using FlutterApp.Api.DTOs;
using FlutterApp.Api.Filters;
using FlutterApp.Core.IRepositories;
using FlutterApp.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace FlutterApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Categories> _repoCategories;

        public CategoriesController(IRepository<Categories> repoCategories, IMapper mapper)
        {
            _repoCategories = repoCategories;
            _mapper = mapper;
        }

        /// <summary>
        /// Bu endpoint tüm kategorileri list olarak geri döner.
        /// </summary>
        /// <returns></returns>
        [Consumes("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _repoCategories.ListAsync(asNoTracking: true);
            return Ok(_mapper.Map<IEnumerable<CategoryDto>>(categories));
        }

        /// <summary>
        /// Bu endpoint verilen id sahip kategoriyi döner.
        /// </summary>
        /// <returns></returns>
        /// <response code="404">Verilen id sahip kategori bulunamadı!</response>
        [Consumes("application/json")]
        [HttpGet("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Categories>))]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _repoCategories.GetByIdAsync(id);
            return Ok(_mapper.Map<CategoryDto>(category));
        }

        /// <summary>
        /// Bu endpoint verilen kategori id sahip soruları döner.
        /// </summary>
        /// <returns></returns>
        /// <response code="404">Verilen kategori id sahip sorular bulunamadı!</response>
        [Consumes("application/json")]
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetWithQuestionsById(int id)
        {
            var category = await _repoCategories.ListAsync(filter: x => x.Id == id, asNoTracking: true, includeProperties: "Questions");
            return Ok(_mapper.Map<CategoryWithQuestionsDto>(category));
        }

        /// <summary>
        /// Bu endpoint kategorilerin toplam,aktif,pasif veri sayısını geri döner.
        /// </summary>
        /// <returns></returns>
        /// [Consumes("application/json")]
        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            TableCountDto tableCountDto = new TableCountDto();
            tableCountDto.TotalCount = await _repoCategories.CountAsync();
            tableCountDto.ActiveCount = await _repoCategories.CountAsync(x => x.IsActive);
            tableCountDto.PassiveCount = await _repoCategories.CountAsync(x => x.IsActive == false);

            return Ok(tableCountDto);
        }

        /// <summary>
        /// Bu endpoint kategori'nin aktiflik durumunu günceller.
        /// </summary>
        /// <returns></returns>
        /// [Consumes("application/json")]
        [HttpPut("IsActive")]
        public async Task<IActionResult> IsActive(CategoryDto categoryDto)
        {
            if (categoryDto.IsActive)
            {
                categoryDto.IsActive = false;
                await _repoCategories.UpdateAsync(_mapper.Map<Categories>(categoryDto));
            }
            else
            {
                categoryDto.IsActive = true;
                await _repoCategories.UpdateAsync(_mapper.Map<Categories>(categoryDto));
            }
            return Ok(categoryDto);
        }

        /// <summary>
        /// Bu endpoint kategori ekler.
        /// </summary>
        /// <remarks>
        /// Örnek: Categories json
        ///  
        ///     POST /Todo
        ///     {
        ///        "name": "Teknoloji",
        ///     }
        /// 
        /// </remarks>
        /// <param name="categoryDto">Categories json nesnesi</param>
        /// <returns></returns>
        [Consumes("application/json")]
        [HttpPost]
        public async Task<IActionResult> Save(CategoryDto categoryDto)
        {
            var newCategory = await _repoCategories.InsertAsync(_mapper.Map<Categories>(categoryDto));
            return Created(new Uri(Request.Path, UriKind.Relative), _mapper.Map<CategoryDto>(newCategory));
        }

        /// <summary>
        /// Bu endpoint verilen id sahip kategoriyi günceller.
        /// </summary>
        /// <remarks>
        /// Örnek: Categories json
        ///  
        ///     POST /Todo
        ///     {
        ///        "id": "2"
        ///        "name": "Teknoloji",
        ///     }
        /// 
        /// </remarks>
        /// <param name="categoryDto">Categories json nesnesi</param>
        /// <returns></returns>
        [Consumes("application/json")]
        [HttpPut]
        public async Task<IActionResult> Update(CategoryDto categoryDto)
        {
            await _repoCategories.UpdateAsync(_mapper.Map<Categories>(categoryDto));
            return NoContent();
        }

        /// <summary>
        /// Bu endpoint verilen kategori id sahip kategoriyi siler.
        /// </summary>
        /// <returns></returns>
        /// <response code="404">Verilen id sahip kategori bulunamadı!</response>
        [Consumes("application/json")]
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Categories>))]
        public async Task<IActionResult> Delete(int id)
        {
            await _repoCategories.DeleteAsync(id);
            return NoContent();
        }
    }
}