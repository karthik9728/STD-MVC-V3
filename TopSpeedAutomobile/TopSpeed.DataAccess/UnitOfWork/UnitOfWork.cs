using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.DataAccess.Common;
using TopSpeed.DataAccess.Repository;
using TopSpeed.Domain.ModelAggregate.User;

namespace TopSpeed.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnitOfWork(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            VehicleType = new VehicleTypeRepository(_dbContext);
            Brand = new BrandRepository(_dbContext);
            Post = new PostRepository(_dbContext);
        }

        public IVehicleTypeRepository VehicleType { get; private set; }

        public IBrandRepository Brand { get; private set; }

        public IPostRepository Post { get; private set; }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task SaveAsync()
        {
            _dbContext.SaveCommonFields(_userManager, _httpContextAccessor);
            await _dbContext.SaveChangesAsync();
        }
    }
}
