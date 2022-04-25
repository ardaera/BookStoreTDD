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
            var expected = _dataContext.books.Include(x => x.Category).FirstOrDefault();
            expected.Title.Should().Be(dto.Title);
            expected.Author.Should().Be(dto.Author);
            expected.Description.Should().Be(dto.Description);
            expected.Pages.Should().Be(dto.Pages);
        }

        [Fact]
        public void GetAll_returns_all_books_properly()
        {
            List<Book> books = CreateBooksInDataBase();

            var expected = _sut.GetAll();

            expected.Should().HaveCount(books.Count);
        }

        [Fact]
        public void Update_updates_book_properly()
        {
            CategoryFactory categoryfactory = new CategoryFactory();
            Category category = categoryfactory.CreateCategory();
            _dataContext.Manipulate(x => x.Categories.Add(category));
            AddBookDto dto = new AddBookDto
            {
                Author = "aaa",
                Description = "ddd",
                Pages = 132,
                Title = "tiii",
                CategoryId = category.Id
            };

            _sut.Add(dto);
            var book = _dataContext.books.Include(x => x.Category).FirstOrDefault();
            _sut.Update(new UpdateBookDto
            {
                Author = "upa",
                Title = "upt",
                Description = "upd",
                Pages = 1111,
                CategoryId = category.Id
            }, book.Id);
            _dataContext.books.Should().Contain(_ => _.Author == "upa");
            _dataContext.books.Should().Contain(_ => _.Title == "upt");
            _dataContext.books.Should().Contain(_ => _.Description == "upd");
            _dataContext.books.Should().Contain(_ => _.Pages == 1111);
            _dataContext.books.Should().Contain(_ => _.CategoryId == category.Id);
        }

        //[Fact]
        //public void Update_CategoryForUpdateNotFoundException_properly()
        //{
        //    var dummycategoryId = 1000;
        //    var dto = CreateCategoryWithCategoryFactory();
        //    Action expected = () =>
        //    _sut.Update(new UpdateCategoryDto { Title = "UpdateDummy" }, dummycategoryId);
        //    expected.Should().ThrowExactly<CategoryForUpdateNotFoundException>();
        //}

        //[Fact]
        //public void Delete_delete_one_category_properly()
        //{
        //    Category category = CreateCategoryWithCategoryFactory();
        //    _sut.Delete(category.Id);
        //    _dataContext.Categories.Should()
        //        .NotContain(_ => _.Title == "dummy1");
        //}

        //[Fact]
        //public void Delete_CategoryForDeleteNotFoundException_properly()
        //{
        //    var dummycategoryId = 1000;
        //    var dto = CreateCategoryWithCategoryFactory();
        //    Action expected = () =>
        //    _sut.Delete(dummycategoryId);
        //    expected.Should().ThrowExactly<CategoryForDeleteNotFoundException>();
        //}

        private Category CreateCategoryWithCategoryFactory()
        {
            CategoryFactory categoryFactory = new CategoryFactory();
            Category category = categoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            return category;
        }

        private List<Book> CreateBooksInDataBase()
        {
            CategoryFactory categoryfactory = new CategoryFactory();
            Category category = categoryfactory.CreateCategory();
            _dataContext.Manipulate(x => x.Categories.Add(category));
            var books = new List<Book>
            {
                new Book { Author="az",Description="des",
                    Pages=100,Title="tit",CategoryId=category.Id},
                new Book { Author="abz",Description="debs",
                    Pages=1010,Title="titb",CategoryId=category.Id}
            };
            _dataContext.Manipulate(_ =>
            _.books.AddRange(books));
            return books;
        }

        //private static AddCategoryDto GenerateAddCategoryDto()
        //{
        //    return new AddCategoryDto
        //    {
        //        Title = "dummy"
        //    };
        //}


    }

}
