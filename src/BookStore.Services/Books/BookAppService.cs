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



    }
}
