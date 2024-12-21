using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using SignalRWebUI.Dtos.FeatureDtos;
using SignalRWebUI.Dtos.SliderDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
	public class SliderController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;

		public SliderController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
		}

		public async Task< IActionResult >Index()
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:SliderApi"];
			var responseMessage = await client.GetAsync(apiAdresi);
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();	
				var values = JsonConvert.DeserializeObject<List<ResultSliderDto>>(jsonData);	
				return View(values);
			}
			return View();
		}

		[HttpGet]
		public IActionResult CreateSlider()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateSlider(CreateSliderDto createSliderDto)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:SliderApi"];
			var jsonData = JsonConvert.SerializeObject(createSliderDto);
			StringContent stringContent = new StringContent(jsonData,Encoding.UTF8,"application/json");
			var responseMessage =await client.PostAsync(apiAdresi, stringContent);
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");	
			}
			return View();
		}

		public async Task<IActionResult> DeleteSlider(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:SliderApi"];
			var responseMessage= await client.DeleteAsync($"{apiAdresi}/{id}");	
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");	
			}
			return View();

		}
		[HttpGet]
		public async Task<IActionResult> UpdateSlider(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:SliderApi"];
			var responseMessage = await client.GetAsync($"{apiAdresi}/{id}");
            if(responseMessage.IsSuccessStatusCode) 
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();	
				var values = JsonConvert.DeserializeObject<UpdateSliderDto>(jsonData);
				return View(values);	
			}
			return View();
        }

		[HttpPost]
		public async Task<IActionResult> UpdateSlider(UpdateSliderDto updateSliderDto)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:SliderApi"];
			var jsonData = JsonConvert.SerializeObject(updateSliderDto);	
		
			StringContent stringContent = new StringContent(jsonData,Encoding.UTF8,"application/json");
			var responseMessage = await client.PutAsync(apiAdresi, stringContent);	
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");	
			}
			return View();
		}

	}
}
