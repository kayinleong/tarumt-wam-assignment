using Microsoft.EntityFrameworkCore;
using Tarumt.WAM.Assignment.Infrastructure.Context;
using Tarumt.WAM.Assignment.Infrastructure.Models;

namespace Tarumt.WAM.Assignment.Infrastructure.Services
{
    public class UserLogService(DatabaseContext context)
    {
        public PagedList<UserLog> GetAll(int pageNumber, int pageSize)
        {
            return PagedList<UserLog>.ToPagedList(
                context.Set<UserLog>()
                    .OrderByDescending(m => m.CreatedAt),
                pageNumber, pageSize);
        }

        public PagedList<UserLog> GetAllByUserId(string userId, int pageNumber, int pageSize)
        {
            return PagedList<UserLog>.ToPagedList(
                context.Set<UserLog>()
                    .Include(m => m.User)
                    .Where(m => m.User.Id == userId)
                    .OrderByDescending(m => m.CreatedAt),
                pageNumber, pageSize);
        }

        public async Task<UserLog> GetByIdAsync(string id)
        {
            return await context.UserLogs!
                .Include(m => m.User)
                .FirstAsync(m => m.Id == id) ?? throw new InvalidOperationException("UserLog not found");
        }

        public async Task CreateAsync(UserLog userLog)
        {
            try
            {
                await context.UserLogs!.AddAsync(userLog);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to create UserLog");
            }
        }
    }
}
