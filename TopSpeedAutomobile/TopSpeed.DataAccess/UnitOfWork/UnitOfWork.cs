using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.DataAccess.Common;
using TopSpeed.DataAccess.Repository;

namespace TopSpeed.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            VehicleType = new VehicleTypeRepository(_dbContext);
            Brand = new BrandRepository(_dbContext);
        }

        public IVehicleTypeRepository VehicleType { get; private set; }

        public IBrandRepository Brand { get; private set; }


        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
