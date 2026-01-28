using LearnLinQWeb.Application.Interfaces;
using LearnLinQWeb.Data;
using LearnLinQWeb.Data.Interfaces;
using LearnLinQWeb.Data.Interfaces.Auth;
using LearnLinQWeb.Data.Interfaces.User;
using LearnLinQWeb.Domain.Entities;
using LearnLinQWeb.DTOs.Auth;
using LearnLinQWeb.Services.Interfaces;

namespace LearnLinQWeb.Services
{
    public class AuthService : IAuthService
    {

        private readonly IAuthQuery _query;
        private readonly IUserService _service;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtService _jwt;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthService(IAuthQuery query, IUserService service, IPasswordHasher hasher, IJwtService jwt, IRefreshTokenService refreshTokenService)
        {
            _query = query;
            _hasher = hasher;
            _jwt = jwt;
            _service = service;
            _refreshTokenService = refreshTokenService;
        }

        //public User? Login(string username,string password)
        //{

        //    return _unitOfWork.AuthQuery.Query().FirstOrDefault(u => u.Username == username && u.Password == password);
        //}

        public async Task<string> LoginAsync(string username, string password)
        {
            //var user = await _userQuery.GetUserByUsernameAsync(username);
            
            var user = await _query.GetByUsernameAsync(username);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            if (!_hasher.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException();
            }

            return _jwt.GenerateToken(user);
        }

        public async Task<RegisterResponse> RegisterAsync(string name, string username, string password)
        {
            var existingUser = await _query.GetByUsernameAsync(username);
            if (existingUser != null)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "Username already exists"
                };
            }

            var user = new User
            {
                Name = name,
                Username = username,
                Role = "User"
            };

            try
            {
                var result = await _service.AddUserAsync(user, password);

                if (!result)
                {
                    return new RegisterResponse
                    {
                        Success = false,
                        Message = "Register failed"
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }

            return new RegisterResponse
            {
                Success = true,
                Message = "Register successful"
            };


        }
    }
}
