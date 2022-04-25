using BookStore.Entities;
using BookStore.Services.Books.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Persistence.EF.Books
{
    public class EFBookRepository : BookRepository
    {
        private readonly EFDataContext _dataContext;
        public EFBookRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Book book)
        {
            _dataContext.books.Add(book);
        }

        public void Delete(Book book)
        {
            _dataContext.books.Remove(book);
        }

        public Book Find(int id)
        {
            return _dataContext.books.Find(id);
        }

        public IList<GetBookDto> GetAll()
        {
            return _dataContext.books.Select(x => new GetBookDto
            {
                Author = x.Author,
                CategoryId = x.CategoryId,
                Description = x.Description,
                Pages = x.Pages,
                Title = x.Title,
            }).ToList();
        }
    }
}
