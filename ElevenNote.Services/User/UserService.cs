using ElevenNote.Data;
using ElevenNote.Data.Entities;
using ElevenNote.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElevenNote.Services.User
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> RegisterUserAsync(UserRegister model)
        {
            if (await GetUserByEmailAsync(model.Email) != null || await GetUserByUsername(model.Username) != null)
            {
                return false;
            }

            var entity = new UserEntity
            {
                Email = model.Email,
                Username = model.Username,
                Password = model.Password,
                DateCreated = DateTime.Now
            };

            var passwordHasher = new PasswordHasher<UserEntity>();

            entity.Password = passwordHasher.HashPassword(entity, model.Password);

            _context.Users.Add(entity);

            var numberOfChanges = await _context.SaveChangesAsync();

            return numberOfChanges == 1;
        }


        public async Task<UserDetail> GetUserByIDAsync(int userID)
        {
            var entity = await _context.Users.FindAsync(userID);

            if (entity is null)
            {
                return null;
            }

            var userDetail = new UserDetail
            {
                ID = entity.ID,
                Email = entity.Email,
                Username = entity.Username,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                DateCreated = entity.DateCreated
            };
            return userDetail;
        }
        private async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower());
        }

        private async Task<UserEntity> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Username.ToLower() == username.ToLower());
        }
    }
}