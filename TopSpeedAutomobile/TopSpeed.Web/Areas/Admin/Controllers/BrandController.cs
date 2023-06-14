using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.Application.Services.Interface;
using TopSpeed.Domain.Model;

namespace TopSpeed.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserNameService _userName;

        public BrandController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IUserNameService userName)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _userName = userName;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Brand> brands = await _unitOfWork.Brand.GetAllAsync();

            return View(brands);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Brand brand = await _unitOfWork.Brand.GetByIdAsync(id);

            brand.CreatedBy = await _userName.GetUserName(brand.CreatedBy);

            brand.ModifiedBy = await _userName.GetUserName(brand.ModifiedBy);

            return View(brand);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Create(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webRootPath, @"images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }

                brand.BrandLogo = @"\images\brand\" + newFileName + extension;
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.Brand.Create(brand);

                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Brand brand = await _unitOfWork.Brand.GetByIdAsync(id);

            return View(brand);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                var newFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webRootPath, @"images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                //delete old image
                var objFromDb = await _unitOfWork.Brand.GetByIdAsync(brand.Id);

                var oldImagePath = Path.Combine(webRootPath, objFromDb.BrandLogo.Trim('\\'));

                if(System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }

                brand.BrandLogo = @"\images\brand\" + newFileName + extension;
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.Brand.Update(brand);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Brand brand = await _unitOfWork.Brand.GetByIdAsync(id);

            return View(brand);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            if (!string.IsNullOrEmpty(brand.BrandLogo))
            {

                //delete old image
                var objFromDb = await _unitOfWork.Brand.GetByIdAsync(brand.Id);

                var oldImagePath = Path.Combine(webRootPath, objFromDb.BrandLogo.Trim('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            await _unitOfWork.Brand.Delete(brand);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
