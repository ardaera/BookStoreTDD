using BookStore.Entities;
using BookStore.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Books.Contracts
{
    public interface BookRepository: Repository
    {
        void Add(Book book);
        IList<GetBookDto> GetAll();
        Book Find(int id);
        void Delete(Book book); 
    }
}
