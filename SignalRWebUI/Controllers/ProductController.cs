﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.CategoryDtos;
using SignalRWebUI.Dtos.ProductDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory; 
        private readonly IConfiguration _configuration;

        public ProductController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task< IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:ProductApi"];
            var responseMessage = await client.GetAsync($"{apiAdresi}/ProductListWithCategory");
            if(responseMessage.IsSuccessStatusCode)
            { 
                var jsonData=await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
                return View(values);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:CategoryApi"];
            var responseMessage = await client.GetAsync(apiAdresi);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();   
            var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
            List<SelectListItem> values2 = (from x in values
                                            select new SelectListItem
                                            {
                                                Text=x.CategoryName,
                                                Value=x.CategoryID.ToString(),
                                            }).ToList();
            ViewBag.v=values2;
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            createProductDto.ProductStatus = true;
            var apiAdresi = _configuration["ApiAdresleri:ProductApi"];
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createProductDto);   
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8,"application/json");
            var responseMessage = await client.PostAsync(apiAdresi, stringContent);
            if(responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");   
            }
            return View();
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:ProductApi"];
            var responseMessage = await client.DeleteAsync($"{apiAdresi}/{id}");
            if(responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {

            // drownliste için 
            var client1 = _httpClientFactory.CreateClient();
            var apiAdresi1 = _configuration["ApiAdresleri:CategoryApi"];
            var responseMessage1 = await client1.GetAsync(apiAdresi1);
            var jsonData1 = await responseMessage1.Content.ReadAsStringAsync();
            var values1 = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData1);
            List<SelectListItem> values2 = (from x in values1
                                            select new SelectListItem
                                            {
                                                Text = x.CategoryName,
                                                Value = x.CategoryID.ToString(),
                                            }).ToList();
            ViewBag.v = values2;

            // 
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:ProductApi"];
            var responseMessage = await client.GetAsync($"{apiAdresi}/{id}");
            if(responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();   
                var values = JsonConvert.DeserializeObject<UpdateProductDto>(jsonData); 
                return View(values);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            updateProductDto.ProductStatus = true;
            var client = _httpClientFactory.CreateClient();
            var apiAdresi = _configuration["ApiAdresleri:ProductApi"];
            var jsonData= JsonConvert.SerializeObject(updateProductDto);
            StringContent stringContent = new StringContent(jsonData,Encoding.UTF8,"application/json");
            var responseMessage= await client.PostAsync(apiAdresi,stringContent);   
            if(responseMessage.IsSuccessStatusCode)
            {
				return RedirectToAction("Index");
			}
			return View();
		}


    }
}
