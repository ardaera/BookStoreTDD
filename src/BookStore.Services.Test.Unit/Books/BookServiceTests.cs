using BookStore.Infrastructure.Application;
using BookStore.Infrastructure.Test;
using BookStore.Persistence.EF;
using BookStore.Persistence.EF.Books;
using BookStore.Services.Books;
using BookStore.Services.Books.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Test.Unit.Books
{
    public class BookServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly BookkService _sut;
        private readonly BookRepository _repository;

        public BookServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFBookRepository(_dataContext);
            _sut = new BookAppService(_unitOfWork, _repository);
        }

        



    }
}
