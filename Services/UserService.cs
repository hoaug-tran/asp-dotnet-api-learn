using LearnLinQWeb.Application.Interfaces;
using LearnLinQWeb.Data;
using LearnLinQWeb.Data.Interfaces;
using LearnLinQWeb.Data.Interfaces.User;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.DTOs.Common;
using LearnLinQWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearnLinQWeb.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public PagedResult<User> GetAllUsers(int page, int limit, string? search, string? sortBy, string? order)
        {

            var query = _unitOfWork.UserQuery.Query();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(u => u.Name.Contains(search) || u.Username.Contains(search) || u.Email.Contains(search) || u.Phone.Contains(search));
            }

            int totalItems = query.Count();


            string sort = sortBy?.ToLowerInvariant() ?? string.Empty;
            string ord = order?.ToLowerInvariant() ?? string.Empty;
            bool isDesc = ord == "desc";

            query = sort switch
            {

                "name" => isDesc ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name),
                "username" => isDesc ? query.OrderByDescending(u => u.Username) : query.OrderBy(u => u.Username),
                "email" => isDesc ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                _ => query.OrderBy(b => b.Id)
            };

            page = page < 1 ? 1 : page;
            limit = limit < 1 ? 10 : limit;

            int totalPages = (int)Math.Round(totalItems / (double)limit);
            if (page > totalPages && totalPages > 0)
            {
                page = totalPages;
            }

            var items = query.Skip((page - 1) * limit).Take(limit).ToList();

            return new PagedResult<User>
            {
                Items = items,
                Page = page,
                Limit = limit,
                TotalItems = totalItems,
                TotalPages = totalPages,
                HasNext = page < totalPages,
                HasPrevious = page > 1
            };


        }

        public User? GetUserById(int id)
        {
            return _unitOfWork.UserQuery.Query().FirstOrDefault(u => u.Id == id);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await Task.FromResult(_unitOfWork.UserQuery.Query().FirstOrDefault(u => u.Id == id));
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return _unitOfWork.UserQuery.Query().FirstOrDefault(u => u.Username == username);
        }

        public async Task<bool> AddUserAsync(User user, string plainPassword)
        {
            user.PasswordHash = _passwordHasher.Hash(plainPassword);
            _unitOfWork.UserCommand.AddUser(user);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                _unitOfWork.UserCommand.UpdateUser(user);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
 
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
        }

        //public bool UpdateUser(User user)
        //{
        //    _unitOfWork.UserCommand.Update(user);
        //    return _unitOfWork.SaveChanges() > 0;
        //}

        //public bool DeleteUser(int id)
        //{
        //    var user = GetUserById(id);
        //    if (user != null)
        //    {
        //        _unitOfWork.UserCommand.Delete(user);
        //        return _unitOfWork.SaveChanges() > 0;
        //    }
        //    return false;
        //}
    }
}
