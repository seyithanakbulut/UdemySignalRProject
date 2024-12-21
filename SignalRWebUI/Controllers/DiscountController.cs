using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.DiscountDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
	public class DiscountController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;	
		private readonly IConfiguration _configuration;

		public DiscountController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
		}

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:DiscountApi"];
			var responseMessage = await client.GetAsync(apiAdresi);
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultDiscountDto>>(jsonData);
				return View(values);	

			}
			return View();
		}

		public IActionResult CreateDiscount()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateDiscount(CreateDiscountDto createDiscountDto)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:DiscountApi"];
			var jsonData = JsonConvert.SerializeObject(createDiscountDto);

			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync(apiAdresi, stringContent);	
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}

			return View();	

		}

		public async Task<IActionResult> DeleteDiscount(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:DiscountApi"];
			var responseMessage = await client.DeleteAsync($"{apiAdresi}/{id}");
			if (responseMessage.IsSuccessStatusCode) 
			{ 
				return RedirectToAction("Index");
			}
			return View();

		}


		[HttpGet]
		public async Task<IActionResult> UpdateDiscount(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:DiscountApi"];
			var responseMessage = await client.GetAsync($"{apiAdresi}/{id}");
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<UpdateDiscountDto>(jsonData);
				return View(values);
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateDiscount(UpdateDiscountDto updateDiscountDto)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:DiscountApi"];
			var jsonData = JsonConvert.SerializeObject(updateDiscountDto);
			StringContent stringContent = new StringContent(jsonData,Encoding.UTF8,"application/json");	
			var responseMessage = await client.PutAsync(jsonData, stringContent);
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return View();

		}

		public async Task<IActionResult> ChangeStatusToTrue(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:DiscountApi"];
			await client.GetAsync($"{apiAdresi}/ChangeStatusToTrue/{id}");
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> ChangeStatusToFalse(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:DiscountApi"];
			await client.GetAsync($"{apiAdresi}/ChangeStatusToFalse/{id}");
			return RedirectToAction("Index");
		}
	}
}
