using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSpeed.Application.Contracts.Presistence
{
    public interface IUnitOfWork 
    {
        public IVehicleTypeRepository VehicleType { get; }

        Task SaveAsync();
    }
}
