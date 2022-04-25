using BookStore.Services.Books.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookStore.RestAPI.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookkService _service;
        public BooksController(BookkService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddBookDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public IList<GetBookDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPut("{id}")]
        public void Update(UpdateBookDto dto, int id)
        {
            _service.Update(dto, id);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.Delete(id);
        }
    }
}
