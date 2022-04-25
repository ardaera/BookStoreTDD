using BookService.Test;
using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Infrastructure.Test;
using BookStore.Persistence.EF;
using BookStore.Persistence.EF.Categories;
using BookStore.Services.Categories;
using BookStore.Services.Categories.Contracts;
using BookStore.Services.Categories.Exceptions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Services.Test.Unit.Categories
{
    public class CategoryServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;

        public CategoryServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_category_properly()
        {
            AddCategoryDto dto = GenerateAddCategoryDto();

            _sut.Add(dto);

            _dataContext.Categories.Should()
                .Contain(_ => _.Title == dto.Title);
        }

        [Fact]
        public void GetAll_returns_all_categories()
        {
            CreateCategoriesInDataBase();

            var expected = _sut.GetAll();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ => _.Title == "dummy1");
            expected.Should().Contain(_ => _.Title == "dummy2");
            expected.Should().Contain(_ => _.Title == "dummy3");
        }

        [Fact]
        public void Update_updates_category_properly()
        {
            Category category = CreateCategoryWithCategoryFactory();
            _sut.Update(new UpdateCategoryDto { Title = "UpdateDummy" }, category.Id);
            _dataContext.Categories.Should()
                .Contain(_ => _.Title == "UpdateDummy");
        }

        [Fact]
        public void Update_CategoryForUpdateNotFoundException_properly()
        {
            var dummycategoryId = 1000;
            var dto = CreateCategoryWithCategoryFactory();
            Action expected = () => 
            _sut.Update(new UpdateCategoryDto { Title = "UpdateDummy" }, dummycategoryId);
            expected.Should().ThrowExactly<CategoryForUpdateNotFoundException>();
        }

        [Fact]
        public void Delete_delete_one_category_properly()
        {
            Category category = CreateCategoryWithCategoryFactory();
            _sut.Delete(category.Id);
            _dataContext.Categories.Should()
                .NotContain(_ => _.Title == "dummy1");
        }

        [Fact]
        public void Delete_CategoryForDeleteNotFoundException_properly()
        {
            var dummycategoryId = 1000;
            var dto = CreateCategoryWithCategoryFactory();
            Action expected = () =>
            _sut.Delete(dummycategoryId);
            expected.Should().ThrowExactly<CategoryForDeleteNotFoundException>();
        }

        private Category CreateCategoryWithCategoryFactory()
        {
            CategoryFactory categoryFactory = new CategoryFactory();
            Category category = categoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            return category;
        }

        private void CreateCategoriesInDataBase()
        {
            var categories = new List<Category>
            {
                new Category { Title = "dummy1"},
                new Category { Title = "dummy2"},
                new Category { Title = "dummy3"}
            };
            _dataContext.Manipulate(_ =>
            _.Categories.AddRange(categories));
        }

        private static AddCategoryDto GenerateAddCategoryDto()
        {
            return new AddCategoryDto
            {
                Title = "dummy"
            };
        }

    }
}
