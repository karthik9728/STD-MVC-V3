using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Domain.Model;

namespace TopSpeed.Application.Contracts.Presistence
{
    public interface IVehicleTypeRepository : IGenericRepository<VehicleType>
    {
        Task Update(VehicleType vehicleType);
    }
}
