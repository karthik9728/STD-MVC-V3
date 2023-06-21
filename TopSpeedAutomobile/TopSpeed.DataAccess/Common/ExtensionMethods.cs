using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Domain.Common;
using TopSpeed.Domain.ModelAggregate.User;

namespace TopSpeed.DataAccess.Common
{
    public static class ExtensionMethods
    {
        public static async Task<string> GetCurrentUserId(UserManager<IdentityUser> _userManager, IHttpContextAccessor _httpContextAccessor)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                userId = user?.Id;
            }

            return userId;
        }

        public static async void SaveCommonFields(this ApplicationDbContext dbContext,UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {

            var userId = await GetCurrentUserId(userManager, httpContextAccessor);

            IEnumerable<BaseModel> insertEntites = dbContext.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity)
                .OfType<BaseModel>();

            IEnumerable<BaseModel> updateEntites = dbContext.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .OfType<BaseModel>();

            foreach (var item in insertEntites)
            {
                item.CreatedOn = DateTime.UtcNow;
                item.CreatedBy = userId;
                item.ModifiedOn = DateTime.UtcNow;
            }

            foreach (var item in updateEntites)
            {
                item.ModifiedOn = DateTime.UtcNow;
                item.ModifiedBy = userId;
            }
        }
    }
}
