using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.SliderDtos;

namespace SignalRWebUI.ViewComponents.DefaultComponents
{
    public class _DefaultSliderComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public _DefaultSliderComponentPartial(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task< IViewComponentResult> InvokeAsync()
        {
            var client =  _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:SliderApi"];

            var responseMessage = await client.GetAsync(apiAdresi);
            if(responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<List<ResultSliderDto>>(jsonData);
                return View(value);
            }

            return View();
        }
    }
}
