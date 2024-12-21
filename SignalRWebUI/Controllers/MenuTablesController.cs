using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.MenuTableDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class MenuTablesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration; 
        public MenuTablesController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:MenuTableApi"];
            var responseMessage = await client.GetAsync(apiAdresi);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultMenuTableDto>>(jsonData);
                return View(values);
            }
            return View();
        }
        [HttpGet]
        public IActionResult CreateMenuTable()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMenuTable(CreateMenuTableDto createMenuTableDto)
        {
            createMenuTableDto.Status = false;
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:MenuTableApi"];

            var jsonData = JsonConvert.SerializeObject(createMenuTableDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync(apiAdresi, stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> DeleteMenuTable(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:MenuTableApi"];

            var responseMessage = await client.DeleteAsync($"{apiAdresi}/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UpdateMenuTable(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:MenuTableApi"];

            var responseMessage = await client.GetAsync($"{apiAdresi}/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateMenuTableDto>(jsonData);
                return View(values);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateMenuTable(UpdateMenuTableDto updateMenuTableDto)
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:MenuTableApi"];

            var jsonData = JsonConvert.SerializeObject(updateMenuTableDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync(apiAdresi, stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TableListByStatus()
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:MenuTableApi"];
            var responseMessage = await client.GetAsync(apiAdresi);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultMenuTableDto>>(jsonData);
                return View(values);
            }
            return View();
        }
    }
}
