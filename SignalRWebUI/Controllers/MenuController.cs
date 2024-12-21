using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.BasketDtos;
using SignalRWebUI.Dtos.ProductDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class MenuController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public MenuController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task< IActionResult> Index(int id)
        {
            ViewBag.v = id;
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:ProductApi"];
            var responseMessage = await client.GetAsync($"{apiAdresi}/ProductListWithCategory");
            if(responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
                return View(value); 
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBasket(int id, int menuTableId)
        {
            if (menuTableId == 0)
            {
                return BadRequest("MenuTableId 0 geliyor.");
            }

            CreateBasketDto createBasketDto = new CreateBasketDto
            {
                ProductID = id,
                MenuTableID = menuTableId // Gelen MenuTableID burada kullanılıyor
            };



            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:BasketApi"];
            var jsonData = JsonConvert.SerializeObject(createBasketDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync(apiAdresi, stringContent);

            var client2 = _httpClientFactory.CreateClient();
            var apiAdresi2 = _configuration["ApiAdresleri:MenuTableApi"];
            await client2.GetAsync($"{apiAdresi2}/ChangeMenuTableStatusToTrue?id=" + menuTableId);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return Json(createBasketDto);
        }

    }
}
