using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignalRWebUI.Dtos.MessageDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    [AllowAnonymous]
    public class DefaultController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public DefaultController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task< IActionResult> Index()
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


        [HttpGet]
        public PartialViewResult SendMessage()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateMessageDto createMessageDto)
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:MessageApi"];
            var jsonData = JsonConvert.SerializeObject(createMessageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync(apiAdresi, stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Default");  
            }

            return View();
        }
    }
}
