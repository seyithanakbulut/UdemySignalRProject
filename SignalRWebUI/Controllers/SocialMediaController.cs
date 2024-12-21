using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.SocialMediaDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
	public class SocialMediaController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;

		public SocialMediaController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
		}

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:SocialMediaApi"];
			var responseMessage=await client.GetAsync(apiAdresi);
			if(responseMessage.IsSuccessStatusCode) 
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values =JsonConvert.DeserializeObject<List<ResultSocialMediaDto>>(jsonData);
				return View(values);	
			}
			return View();
		}


		public IActionResult CreateSocialMedia()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateSocialMedia(CreateSocialMediaDto createSocialMediaDto)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:SocialMediaApi"];
			var jsonData = JsonConvert.SerializeObject(createSocialMediaDto);
			StringContent stringContent = new StringContent(jsonData,Encoding.UTF8,"application/json");
			var responseMessage = await client.PostAsync(apiAdresi, stringContent);
			if(responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");	
			}
			return View();
		}

		public async Task<IActionResult> DeleteSocialMedia(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:SocialMediaApi"];
			var responseMessage = await client.DeleteAsync($"{apiAdresi}/{id}");
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateSocialMedia(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:SocialMediaApi"];
			var responseMessage =await client.GetAsync($"{apiAdresi}/{id}");
			if(responseMessage.IsSuccessStatusCode)
			{
				var jsonData =await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<UpdateSocialMediaDto>(jsonData);
				return View(values);

			}
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> UpdateSocialMedia(UpdateSocialMediaDto updateSocialMediaDto)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:SocialMediaApi"];
			var jsonData =  JsonConvert.SerializeObject(updateSocialMediaDto);	
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
