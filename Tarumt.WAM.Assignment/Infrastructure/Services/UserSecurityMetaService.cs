using Microsoft.EntityFrameworkCore;
using Tarumt.WAM.Assignment.Infrastructure.Context;
using Tarumt.WAM.Assignment.Infrastructure.Models;

namespace Tarumt.WAM.Assignment.Infrastructure.Services
{
    public class UserSecurityMetaService(DatabaseContext context)
    {
        public async Task<UserSecurityMeta> GetByIdAsync(string id)
        {
            return await context.UserSecurityMetas!
                .FirstOrDefaultAsync(m => m.Id == id) ?? throw new InvalidOperationException("UserSecurityMeta not found");
        }

        public async Task CreateAsync(UserSecurityMeta userSecurityMeta)
        {
            try
            {
                await context.UserSecurityMetas!.AddAsync(userSecurityMeta);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to create UserSecurityMeta");
            }
        }

        public async Task UpdateByIdAsync(UserSecurityMeta userSecurityMeta)
        {
            var existingUserSecurityMeta = await GetByIdAsync(userSecurityMeta.Id);
            existingUserSecurityMeta.Type = userSecurityMeta.Type;

            try
            {
                context.UserSecurityMetas!.Update(existingUserSecurityMeta);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Failed to create UserSecurityMeta");
            }
        }
    }
}
