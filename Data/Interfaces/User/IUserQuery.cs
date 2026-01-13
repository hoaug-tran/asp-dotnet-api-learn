using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Data.Interfaces.User;

public interface IUserQuery
{
    IQueryable<Domain.Entities.User> Query();
}
