﻿using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.BookintDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBookingDto> _validator;

        public BookingController(IBookingService bookingService, IMapper mapper, IValidator<CreateBookingDto> validator)
        {
            _bookingService = bookingService;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public IActionResult BookingList()
        {
            var values = _bookingService.TGetListAll();
            return Ok(_mapper.Map<List<ResultBookingDto>>(values));  
        }

        [HttpPost]
        public IActionResult CreateBooking(CreateBookingDto createBookingDto)
        {

            var validationResult = _validator.Validate(createBookingDto);
            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            else
            {
                var value = _mapper.Map<Booking>(createBookingDto);

                _bookingService.TAdd(value);
                return Ok("Rezervasyon kısmı başarılı bir şekilde eklenmiştir.");
            }
          
        }

		[HttpDelete("{id}")]
		public IActionResult DeleteBooking(int id)
        {
            var values= _bookingService.TGetByID(id);
            _bookingService.TDelete(values);
            return Ok("Silme işlemi gerçekleştirilmiştir");
        }

        [HttpPut]
        public IActionResult UpdateBooking(UpdateBookingDto updateBookingDto)
        {
           var value=_mapper.Map<Booking>(updateBookingDto);

            _bookingService.TUpdate(value);
            return Ok("Güncelleme işlemi başarılı bir şekilde gerçekleştirilmiştir.");

        }

		[HttpGet("{id}")]
		public IActionResult GetBooking(int id)
        {
            var values = _bookingService.TGetByID(id);
            return Ok(_mapper.Map<GetBookingDto>(values));
        }

		[HttpGet("BookingStatusApproved/{id}")]
		public IActionResult BookingStatusApproved(int id)
		{
			_bookingService.TBookingStatusApproved(id);
			return Ok("Rezervasyon Açıklaması Değiştirildi");
		}
		[HttpGet("BookingStatusCancelled/{id}")]
		public IActionResult BookingStatusCancelled(int id)
		{
			_bookingService.TBookingStatusCancelled(id);
			return Ok("Rezervasyon Açıklaması Değiştirildi");
		}
	}
}
