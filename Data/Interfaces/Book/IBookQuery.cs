using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.DTOs;

namespace LearnLinQWeb.Data.Interfaces.Book;

public interface IBookQuery
{
    IQueryable<Domain.Entities.Book> Query();
    //IQueryable<User> Query();

    //IQueryable<LoginRequest> Query();

}
