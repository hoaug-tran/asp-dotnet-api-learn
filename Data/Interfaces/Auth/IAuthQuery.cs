using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Data.Interfaces.Auth;

public interface IAuthQuery
{
    IQueryable<Domain.Entities.User> Query();
}
