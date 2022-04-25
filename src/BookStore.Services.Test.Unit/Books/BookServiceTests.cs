using BookService.Test;
using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Infrastructure.Test;
using BookStore.Persistence.EF;
using BookStore.Persistence.EF.Books;
using BookStore.Services.Books;
using BookStore.Services.Books.Contracts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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

        [Fact]
        public void Add_add_book_to_category_properly()
        {
            CategoryFactory categoryfactory = new CategoryFactory();
            Category category = categoryfactory.CreateCategory();
            _dataContext.Manipulate(x => x.Categories.Add(category));
            AddBookDto dto = new AddBookDto
            {
                Author = "a",
                Description = "d",
                Pages = 12,
                Title = "t",
                CategoryId = category.Id
            };

            _sut.Add(dto);
            var expected = _dataContext.books.Include(x=>x.Category).FirstOrDefault();
            expected.Title.Should().Be(dto.Title);
            expected.Author.Should().Be(dto.Author);
            expected.Description.Should().Be(dto.Description);
            expected.Pages.Should().Be(dto.Pages);

        }



    }

}
