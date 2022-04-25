using BookStore.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Books.Contracts
{
    public interface BookkService: Service
    {
        void Add(AddBookDto dto);
        IList<GetBookDto> GetAll();
        void Update(UpdateBookDto dto, int id);
        void Delete(int id);
    }
}
