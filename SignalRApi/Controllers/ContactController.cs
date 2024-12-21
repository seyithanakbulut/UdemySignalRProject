﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.ContactDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public ContactController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult ContactList()
        {


            var values = _mapper.Map<List<ResultContactDto>>(_contactService.TGetListAll());
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateContact(CreateContactDto createContactDto)
        {
           var value = _mapper.Map<Contact>(createContactDto);
            _contactService.TAdd(value);  
            return Ok("Ekleme yapıldı"); 
        }

		[HttpDelete("{id}")]
		public IActionResult DeleteContact(int id)
        {
            var value= _contactService.TGetByID(id);    
            _contactService.TDelete(value);
            return Ok("Silme işlemi gerçekleştirildi");
        }

		[HttpGet("{id}")]
		public IActionResult GetContact(int id)
        {

            var values = _contactService.TGetByID(id);
            return Ok(_mapper.Map<GetContactDto>(values));  
        }

        [HttpPut]
        public IActionResult UpdateContact(UpdateContactDto updateContactDto)
        {
            var value = _mapper.Map<Contact>(updateContactDto);
            _contactService.TUpdate(value);
            return Ok("Güncelleme işlemi başarıyla tamamlandı");
        }
    }
}
