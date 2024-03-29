﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.Application.ExtensionsMethods;
using TopSpeed.Application.Services.Interface;
using TopSpeed.DataAccess.Common;
using TopSpeed.Domain.Model;

namespace TopSpeed.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VehicleTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserNameService _userName;

        public VehicleTypeController(IUnitOfWork unitOfWork, IUserNameService userName)
        {
            _unitOfWork = unitOfWork;
            _userName = userName;
        }

        public async Task<IActionResult> Index()
        {
            List<VehicleType> vehicleTypes = await _unitOfWork.VehicleType.GetAllAsync();

            return View(vehicleTypes);
        }

        public async Task<IActionResult> Details(int id)
        {
            VehicleType vehicleType = await _unitOfWork.VehicleType.GetByIdAsync(id);

            vehicleType.CreatedBy = await _userName.GetUserName(vehicleType.CreatedBy);

            vehicleType.ModifiedBy = await _userName.GetUserName(vehicleType.ModifiedBy);

            return View(vehicleType);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.VehicleType.Create(vehicleType);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicleType = await _unitOfWork.VehicleType.GetByIdAsync(id);

            return View(vehicleType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.VehicleType.Update(vehicleType);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicleType = await _unitOfWork.VehicleType.GetByIdAsync(id);

            return View(vehicleType);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(VehicleType vehicleType)
        {
            await _unitOfWork.VehicleType.Delete(vehicleType);
            return RedirectToAction(nameof(Index));
        }
    }
}
