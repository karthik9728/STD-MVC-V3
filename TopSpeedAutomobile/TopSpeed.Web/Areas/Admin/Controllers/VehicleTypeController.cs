using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.DataAccess.Common;
using TopSpeed.Domain.Models;

namespace TopSpeed.Web.Areas.Admin.Controllers
{
    public class VehicleTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            List<VehicleType> vehicleTypes = await _unitOfWork.VehicleType.GetAllAsync();

            return View(vehicleTypes);
        }

        public async Task<IActionResult> Details(int id)
        {
            VehicleType vehicleType = await _unitOfWork.VehicleType.GetByIdAsync(id);
            return View(vehicleType);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            VehicleType vehicleType = new VehicleType
            {
                Name = name
            };

            await _unitOfWork.VehicleType.Create(vehicleType);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
