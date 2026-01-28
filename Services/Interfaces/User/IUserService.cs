using System.Collections.Generic;
using System.Globalization;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.DTOs.Common;

namespace LearnLinQWeb.Services.Interfaces;

public interface IUserService
{

    PagedResult<User> GetAllUsers(int page, int limit, string? search, string? sortBy, string? order);
    User? GetUserById(int id);
    Task<User?> GetUserByIdAsync(int id);

    Task<User?> GetUserByUsernameAsync(string username);
    //bool AddUser(User user);
    //bool UpdateUser(User user);
    //bool DeleteUser(int id);

    Task<bool> AddUserAsync(User user, string plainPassword);
    Task<bool> UpdateUserAsync(User user);
}

