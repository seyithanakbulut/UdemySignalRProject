using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.BasketDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class BasketsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private  readonly IConfiguration _configuration;

        public BasketsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task< IActionResult> Index(int id)
        {
            TempData["id"] = id;
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:BasketApi"];
            var responseMessage = await client.GetAsync($"{apiAdresi}/BasketListByMenuTableWithProductName?id="+id);
            if(responseMessage.IsSuccessStatusCode)
            {
                var jsonData=await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<List<ResultBasketDto>>(jsonData);
                return View(value);
            }
            return View();
        }

        public async Task<IActionResult> DeleteBasket(int id)
        {
            id = int.Parse(TempData["id"].ToString());
            var apiAdresi = _configuration["ApiAdresleri:BasketApi"];
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"{apiAdresi}/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            //aynı sayfa tekrar kalsın
            return NoContent();
        }



    }
}
