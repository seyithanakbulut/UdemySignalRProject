using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.ProductDtos;

namespace SignalRWebUI.ViewComponents.DefaultComponents
{
	public class _DefaultOurMenuComponentPartial:ViewComponent
	{
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration; 
        public _DefaultOurMenuComponentPartial(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:ProductApi"];
            var responseMessage = await client.GetAsync($"{apiAdresi}/GetLast9Products");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
            return View(values);
        }
    }
}
