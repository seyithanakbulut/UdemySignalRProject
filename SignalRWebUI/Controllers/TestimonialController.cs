using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.TestimonialDtos;
using SignalRWebUI.ViewComponents.LayoutComponents;
using System.Text;

namespace SignalRWebUI.Controllers
{
	public class TestimonialController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;

		public TestimonialController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
		}

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:TestimonialApi"];
			var responseMessage = await client.GetAsync(apiAdresi);	
			if(responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();	
				var values = JsonConvert.DeserializeObject<List<ResultTestimonialDto>>(jsonData);
				return View(values);
			}
			return View();

		}

		public IActionResult CreateTestimonial()
		{
			return View();	
		}

		[HttpPost]
		public async Task<IActionResult> CreateTestimonial(CreateTestimonialDto createTestimonialDto)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:TestimonialApi"];
			var jsonData = JsonConvert.SerializeObject(createTestimonialDto);
			StringContent stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8,"application/json");
			var responseMessage = await client.PostAsync(apiAdresi, stringContent);	
			if(responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return View();
		}


		public async Task<IActionResult> DeleteTestimonial(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:TestimonialApi"];
			var responseMessage= await client.DeleteAsync($"{apiAdresi}/{id}");	
			if(responseMessage.IsSuccessStatusCode) 
			{
				return RedirectToAction("Index");	
			}
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateTestimonial(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:TestimonialApi"];
			var responseMessage = await client.GetAsync($"{apiAdresi}/{id}");
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<UpdateTestimonialDto>(jsonData);
				return View(values);
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> UpdateTestimonial(UpdateTestimonialDto updateTestimonialDto)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:TestimonialApi"];
			var jsonData = JsonConvert.SerializeObject(updateTestimonialDto);
			StringContent stringContent= new StringContent(jsonData,Encoding.UTF8,"application/json");
			var responseMessage = await client.PutAsync(apiAdresi, stringContent);	
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return View();
		}
	}
}
