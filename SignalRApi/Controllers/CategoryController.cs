using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.CategoryDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult CategoryList()
        {
            var value = _mapper.Map<List<ResultCategoryDto>>(_categoryService.TGetListAll());
            return Ok(value);  
        }

        [HttpGet("CategoryCount")]
        public IActionResult CategoryCount()
        {
            return Ok(_categoryService.TCategoryCount());
        }


        [HttpGet("ActiveCategoryCount")]
        public IActionResult ActiveCategoryCount()
        {
            return Ok(_categoryService.TActiveCategoryCount());
        }


        [HttpGet("PassiveCategoryCount")]
        public IActionResult PassiveCategoryCount()
        {
            return Ok(_categoryService.TPassiveCategoryCount());
        }



        [HttpPost]
        public IActionResult CreateCategory(CreateCategoryDto createcategoryDto)
        {
            createcategoryDto.Status = true;
            var value = _mapper.Map<Category>(createcategoryDto);
            _categoryService.TAdd(value);
            return Ok("Kategori başarılı bir şekilde eklenmiştir");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id) { 
        
            var values = _categoryService.TGetByID(id); 
            _categoryService.TDelete(values);
            return Ok("Başarılı bir şekilde silme işlemi gerçekleştirilmiştir");
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var values = _categoryService.TGetByID(id);
            return Ok(values);
        }

        [HttpPut]
        public IActionResult UpdateCategory(UpdateCategoryDto updateCategoryDto) 
        {
            var value = _mapper.Map<Category>(updateCategoryDto);
            _categoryService.TUpdate(value);    
          
            return Ok("Güncelleme işlemi başarıyla gerçekleştirilmiştir.");
        }
    }
}
