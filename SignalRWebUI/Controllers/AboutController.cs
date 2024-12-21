using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.AboutDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class AboutController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AboutController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:AboutApi"];

            var responseMessage= await client.GetAsync(apiAdresi);  
            if(responseMessage.IsSuccessStatusCode)
            {
                var jsonData =await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultAboutDto>>(jsonData);
                return View(values);
            }

            return View();
        }

        [HttpGet]
        public IActionResult CreateAbout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAbout(CreateAboutDto createAboutDto)
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:AboutApi"];
            var jsonData = JsonConvert.SerializeObject(createAboutDto); 
            StringContent stringContent=new StringContent(jsonData,Encoding.UTF8,"application/json");
            var responseMessage = await client.PostAsync(apiAdresi, stringContent); 
            if(responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");   

            }
            return View();  
        }

       
        public async Task<IActionResult> DeleteAbout(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:AboutApi"];
            var responseMessage = await client.DeleteAsync($"{apiAdresi}/{id}");
            if(responseMessage.IsSuccessStatusCode) 
            {
                return RedirectToAction("Index");
               
                
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAbout(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:AboutApi"];
            var responseMessage = await client.GetAsync($"{apiAdresi}/{id}");
            if(responseMessage.IsSuccessStatusCode )
            {
                var jsonData= await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateAboutDto>(jsonData);   
                return View(values);
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:AboutApi"];
            var jsonData = JsonConvert.SerializeObject(updateAboutDto);
            StringContent stringContent=new StringContent(jsonData,Encoding.UTF8,"application/json");
            var responseMessage= await client.PutAsync(apiAdresi,stringContent);
            if(responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();  
        }
    }
}
