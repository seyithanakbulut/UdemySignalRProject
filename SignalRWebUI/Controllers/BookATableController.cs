using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignalRWebUI.Dtos.BookingDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class BookATableController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory; 
        private readonly IConfiguration _configuration;

        public BookATableController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public  async Task<IActionResult> Index()
        {
            HttpClient client = new HttpClient();
            var apiAdresi = _configuration["ApiAdresleri:ContactApi"];
            HttpResponseMessage response = await client.GetAsync(apiAdresi);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JArray item = JArray.Parse(responseBody);
            string value = item[0]["location"].ToString();
            ViewBag.location = value;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateBookingDto createBookingDto)
        {

            HttpClient client2 = new HttpClient();
            var apiAdresi2 = _configuration["ApiAdresleri:ContactApi"];
            HttpResponseMessage response = await client2.GetAsync(apiAdresi2);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JArray item = JArray.Parse(responseBody);
            string value = item[0]["location"].ToString();
            ViewBag.location = value;

            createBookingDto.Description = "Rezervasyon alındı";

            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:BookingDto"];
            var jsonData = JsonConvert.SerializeObject(createBookingDto);
            StringContent stringContent = new StringContent(jsonData,Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync(apiAdresi, stringContent);


            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Default");
            }
            else
            {
                var errorContent = await responseMessage.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, errorContent);
                return View();
            }
        }
    }
}
