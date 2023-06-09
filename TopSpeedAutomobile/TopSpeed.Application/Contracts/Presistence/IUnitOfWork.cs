using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSpeed.Application.Contracts.Presistence
{
    public interface IUnitOfWork : IDisposable
    {
        public IVehicleTypeRepository VehicleType { get; }

        public IBrandRepository Brand { get; }

        Task SaveAsync();
    }
}
