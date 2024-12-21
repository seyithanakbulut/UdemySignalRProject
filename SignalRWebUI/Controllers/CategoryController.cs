using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.CategoryDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

		public CategoryController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
		}

		public async Task< IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiAdresleri:CategoryApi"];
            var responseMessage = await client.GetAsync(apiUrl);
            // 200 ile 299 arasında başarlı kod dönerse
            if(responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values =JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
                return View(values);    
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();  
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            createCategoryDto.Status = true;
            var client = _httpClientFactory.CreateClient();
            var jsonData=JsonConvert.SerializeObject(createCategoryDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8,"application/json");

            var apiUrl = _configuration["ApiAdresleri:CategoryApi"];
            var responseMessage = await client.PostAsync(apiUrl,stringContent);
            if(responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");   
            }
            return View();  

        }


        public async Task<IActionResult> DeleteCategory(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiAdresleri:CategoryApi"];
            var responseMessage = await client.DeleteAsync($"{apiUrl}/{id}");
            if(responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiAdresleri:CategoryApi"];
            var responseMessage = await client.GetAsync($"{apiUrl}/{id}");
            if(responseMessage.IsSuccessStatusCode)
            {
                var jsonData= await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateCategoryDto>(jsonData);
                return View(values);    
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            var client= _httpClientFactory.CreateClient();
            var jsonData= JsonConvert.SerializeObject(updateCategoryDto);
            StringContent stringContent = new StringContent(jsonData,Encoding.UTF8,"application/json");
            var apiUrl = _configuration["ApiAdresleri:CategoryApi"];
            var responseMessage = await client.PutAsync(apiUrl,stringContent);
            if(responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
