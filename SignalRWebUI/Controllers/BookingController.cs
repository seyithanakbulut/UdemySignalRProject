using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.BookingDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class BookingController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory; 
        private readonly IConfiguration _configuration;

        public BookingController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task< IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:BookingApi"];
            var responseMessage= await client.GetAsync(apiAdresi);  
            if(responseMessage.IsSuccessStatusCode)
            {
                var jsonData= await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultBookingDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateBooking()
        {
            return  View(); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingDto createBookingDto)
        {
            createBookingDto.Description = "Rezervasyon alındı";
            var client = _httpClientFactory.CreateClient();
            var apiAdres = _configuration["ApiAdresleri:BookingApi"];
            var jsonData = JsonConvert.SerializeObject(createBookingDto);
            StringContent stringContent = new StringContent(jsonData,Encoding.UTF8,"application/json");
            var responseMessage = await client.PostAsync(apiAdres,stringContent);
            if(responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");   
            }
            return View();
            
        }

        public async Task<IActionResult> DeleteBooking(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:BookingApi"];
            var responseMessage = await client.DeleteAsync($"{apiAdresi}/{id}");
            if(responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBooking(int id)
		{

			var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:BookingApi"];

            var responseMessage = await client.GetAsync($"{apiAdresi}/{id}");   
            if(responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateBookingDto>(jsonData); 
                return View(values);    
            }

            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBooking(UpdateBookingDto updateBookingDto)
		{
			updateBookingDto.Description = "Rezervasyon alındı";

			var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:BookingApi"];
            var jsonData = JsonConvert.SerializeObject(updateBookingDto);
            StringContent stringContent = new StringContent(jsonData,Encoding.UTF8, "application/json");
            var responseMessage=await client.PutAsync(apiAdresi, stringContent);    
            if(responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

		public async Task<IActionResult> BookingStatusApproved(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:BookingApi"];
			await client.GetAsync($"{apiAdresi}/BookingStatusApproved/{id}");
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> BookingStatusCancelled(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:BookingApi"];
			await client.GetAsync($"{apiAdresi}/BookingStatusCancelled/{id}");
			return RedirectToAction("Index");
		}


	}
}
