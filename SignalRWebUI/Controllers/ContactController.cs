using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.ContactDtos;
using System.Text;
using System.Text.Json.Serialization;

namespace SignalRWebUI.Controllers
{
	public class ContactController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;

		public ContactController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
		}

		public async  Task<IActionResult> Index()
		{

			var client =  _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:ContactApi"];
			var responseMessage= await client.GetAsync(apiAdresi);
			if(responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultContactDto>>(jsonData);
				return View(values);
			}
			return View();
		}

		public IActionResult CreateContact()
		{
			return View();	
		}

		[HttpPost]
		public async Task<IActionResult> CreateContact(CreateContactDto createContactDto)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:ContactApi"];
			var jsonData = JsonConvert.SerializeObject(createContactDto);
			StringContent stringContent = new StringContent(jsonData,Encoding.UTF8,"application/json");
			var responseMessage= await client.PostAsync(apiAdresi, stringContent);	
			if(responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return View();
		}

		public async Task<IActionResult> DeleteContact(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:ContactApi"];
			var responseMessage = await client.DeleteAsync($"{apiAdresi}/{id}");
			if(responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateContact(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:ContactApi"];
			var responseMessage = await client.GetAsync($"{apiAdresi}/{id}");
			if(responseMessage.IsSuccessStatusCode) 
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<UpdateContactDto>(jsonData);
				return View(values);

			}
			return View();

		}

		[HttpPost]
		public async Task<IActionResult> UpdateContact(UpdateContactDto updateContactDto)
		{
			var client = _httpClientFactory.CreateClient();
			var apiAdresi = _configuration["ApiAdresleri:ContactApi"];
			var jsonData = JsonConvert.SerializeObject(updateContactDto);	
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8,"application/json");
			var responseMessage = await client.PutAsync(apiAdresi, stringContent);
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");	
			}
			return View();
		}


	}
}
