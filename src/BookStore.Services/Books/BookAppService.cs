using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Services.Books.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Books
{
    public class BookAppService: BookkService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly BookRepository _repository;
        public BookAppService(UnitOfWork unitOfWork, BookRepository repository)
        {
            _unitOfWork = unitOfWork;   
            _repository = repository;   
        }
        public void Add (AddBookDto dto)
        {
            var book = new Book
            {
                Author = dto.Author,
                CategoryId = dto.CategoryId,
                Description = dto.Description,
                Pages = dto.Pages,
                Title = dto.Title,
            };

            _repository.Add(book);

            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var book = _repository.Find(id);
            if (book==null)
            {
                throw new BookForDeleteNotFoundException();
            }
            else
            {
                _repository.Delete(book);
                _unitOfWork.Commit();
            }
        }

        public IList<GetBookDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(UpdateBookDto dto, int id)
        {
            var book = _repository.Find(id);
            if (book==null)
            {
                throw new BookForUpdateNotFoundException();
            }
            else
            {
                book.Author = dto.Author;
                book.CategoryId = dto.CategoryId;
                book.Description=   dto.Description;
                book.Pages= dto.Pages;
                book.Title = dto.Title; 
                _unitOfWork.Commit();
            }
        }
    }
}
