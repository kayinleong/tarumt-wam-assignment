using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tarumt.WAM.Assignment.Infrastructure.Context;
using Tarumt.WAM.Assignment.Infrastructure.Models;

namespace Tarumt.WAM.Assignment.Infrastructure.Services
{
    public class UserService(DatabaseContext context, IPasswordHasher<User> passwordHasher)
    {
        public PagedList<User> GetAllAsync(int pageNumber, int pageSize, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<User>.ToPagedList(
                    context.Set<User>()
                        .Include(m => m.SecurityMeta)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<User>.ToPagedList(
                    context.Set<User>()
                        .Where(m => m.Username.Contains(keyword))
                        .Include(m => m.SecurityMeta)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await context.Users!
                .Include(m => m.SecurityMeta)
                .Include(m => m.Tickets)
                .FirstAsync(m => m.Id == id) ?? throw new InvalidOperationException("User not found");
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await context.Users!
                .Include(m => m.SecurityMeta)
                .Include(m => m.Tickets)
                .FirstAsync(m => m.Username == username) ?? throw new InvalidOperationException("User not found");
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await context.Users!
                .Include(m => m.SecurityMeta)
                .Include(m => m.Tickets)
                .FirstAsync(m => m.Email == email) ?? throw new InvalidOperationException("User not found");
        }

        public async Task CreateAsync(User user)
        {
            if (context.Users!.Any(m => m.Username == user.Username))
            {
                throw new InvalidOperationException("Username already exists");
            }

            if (context.Users!.Any(m => m.Email == user.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            user.Password = passwordHasher.HashPassword(user, user.Password);

            try
            {
                await context.Users!.AddAsync(user);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to create User");
            }
        }

        public async Task UpdateByIdAsync(User user)
        {
            if (context.Users!.Any(m => m.Username == user.Username && m.Id != user.Id))
            {
                throw new InvalidOperationException("Username already exists");
            }

            if (context.Users!.Any(m => m.Email == user.Email && m.Id != user.Id))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var existingUser = await GetByIdAsync(user.Id);
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.SecurityMeta = user.SecurityMeta;
            existingUser.Password = passwordHasher.HashPassword(user, user.Password);
            existingUser.UpdatedAt = DateTime.Today;

            try
            {
                context.Users!.Update(existingUser);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to update User");
            }
        }

        public async Task DeleteByIdAsync(User user)
        {
            var existingUser = await GetByIdAsync(user.Id);

            try
            {
                context.Users!.Remove(existingUser);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to delete User");
            }
        }

        public void ValidatePassword(string rawPassword, User user)
        {
            if (passwordHasher.VerifyHashedPassword(user, user.Password, rawPassword) == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException("Invalid login credentials");
            }
        }
    }
}
