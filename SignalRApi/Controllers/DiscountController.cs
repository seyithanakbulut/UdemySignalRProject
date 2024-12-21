using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.DiscountDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;

        public DiscountController(IDiscountService discountService, IMapper mapper)
        {
            _discountService = discountService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult DiscountList()
        {
            var values = _mapper.Map<List<ResultDiscountDto>>(_discountService.TGetListAll());
            return Ok(values);  
        }

        [HttpPost]
        public IActionResult CreateDiscount(CreateDiscountDto createDiscount)
        {
            var value = _mapper.Map<Discount>(createDiscount);
            _discountService.TAdd(value);
            return Ok(value);    

        }

		[HttpDelete("{id}")]
		public IActionResult DeleteDiscount(int id)
        {
            var values = _discountService.TGetByID(id);
            _discountService.TDelete(values);
            return Ok("Silme işlemi gerçekleştirilmiştir");
        }

		[HttpGet("{id}")]
		public IActionResult GetDiscount(int id) 
        {
            var values = _discountService.TGetByID(id);
            return Ok(_mapper.Map<GetDiscountDto>(values));
        }

        [HttpPut]
        public IActionResult UpdateDiscount(UpdateDiscountDto updateDiscount)
        {
            var value = _mapper.Map<Discount>(updateDiscount);
            
            _discountService.TUpdate(value);
            return Ok("Güncelleme işlemi başarıyla tamamlandı");
        }

		[HttpGet("ChangeStatusToTrue/{id}")]
		public IActionResult ChangeStatusToTrue(int id)
		{
			_discountService.TChangeStatusToTrue(id);
			return Ok("Ürün İndirimi Aktif Hale Getirildi");
		}

		[HttpGet("ChangeStatusToFalse/{id}")]
		public IActionResult ChangeStatusToFalse(int id)
		{
			_discountService.TChangeStatusToFalse(id);
			return Ok("Ürün İndirimi Pasif Hale Getirildi");
		}

        [HttpGet("GetListByStatusTrue")]
        public IActionResult GetListByStatusTrue()
        {
            return Ok(_discountService.TGetListByStatusTrue());
        }
    }
}
