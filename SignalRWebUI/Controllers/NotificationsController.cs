using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.NotificationDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
	public class NotificationsController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;	
		public NotificationsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
		}
		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:NotificationApi"];
			var responseMessage = await client.GetAsync(apiAdresi);
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultNotificationDto>>(jsonData);
				return View(values);
			}
			return View();
		}
		[HttpGet]
		public IActionResult CreateNotification()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateNotification(CreateNotificationDto createNotificationDto)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:NotificationApi"];
			var jsonData = JsonConvert.SerializeObject(createNotificationDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync(apiAdresi, stringContent);
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return View();
		}
		public async Task<IActionResult> DeleteNotification(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:NotificationApi"];
			var responseMessage = await client.DeleteAsync($"{apiAdresi}/{id}");
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> UpdateNotification(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:NotificationApi"];
			var responseMessage = await client.GetAsync($"{apiAdresi}/{id}");
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<UpdateNotificationDto>(jsonData);
				return View(values);
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> UpdateNotification(UpdateNotificationDto updateNotificationDto)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:NotificationApi"];
			var jsonData = JsonConvert.SerializeObject(updateNotificationDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync(apiAdresi, stringContent);
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return View();
		}


        public async Task<IActionResult> NotificationStatusChangeToTrue(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:NotificationApi"];
            await client.GetAsync($"{apiAdresi}/NotificationStatusChangeToTrue/{id}");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> NotificationStatusChangeToFalse(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:NotificationApi"];
            await client.GetAsync($"{apiAdresi}/NotificationStatusChangeToFalse/{id}");
            return RedirectToAction("Index");
        }
    }
}
