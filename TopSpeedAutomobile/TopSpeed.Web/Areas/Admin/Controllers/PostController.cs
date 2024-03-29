﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.Domain.ModelAggregate.Post;
using TopSpeed.Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using TopSpeed.Domain.ApplicationEnums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using TopSpeed.Domain.Model;
using System.Drawing.Drawing2D;
using TopSpeed.Application.Services.Interface;

namespace TopSpeed.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserNameService _userName;

        public PostController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IUserNameService userName)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _userName = userName;
        }

        public async Task<IActionResult> Index()
        {
            List<Post> posts = await _unitOfWork.Post.GetAllPost();

            return View(posts);
        }


        public async Task<IActionResult> Details(int id)
        {
            var post = await _unitOfWork.Post.GetPostById(id);

            post.CreatedBy = await _userName.GetUserName(post.CreatedBy);

            post.ModifiedBy = await _userName.GetUserName(post.ModifiedBy);

            return View(post);
        }

        [HttpGet]

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> brandList = _unitOfWork.Brand.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()
            });

            IEnumerable<SelectListItem> vehicleTypeList = _unitOfWork.VehicleType.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()
            });

            IEnumerable<SelectListItem> engineAndFuelType = Enum.GetValues(typeof(EngineAndFuelType))
                .Cast<EngineAndFuelType>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            IEnumerable<SelectListItem> transmission = Enum.GetValues(typeof(Transmission))
               .Cast<Transmission>()
               .Select(x => new SelectListItem
               {
                   Text = x.ToString().ToUpper(),
                   Value = ((int)x).ToString()
               });

            PostVM postVM = new PostVM
            {
                Post = new Post(),
                BrandList = brandList,
                VehicleTypeList = vehicleTypeList,
                EngineAndFuelTypeList = engineAndFuelType,
                TransmissionList = transmission
            };

            return View(postVM);
        }


        [HttpPost]
        public async Task<IActionResult> Create(PostVM postVM)
        {
            var webRootPath = _webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webRootPath + @"\images\post");

                var extension = Path.GetExtension(file[0].FileName);

                using (var fileSteam = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileSteam);
                }

                postVM.Post.VehicleImage = @"\images\post\" + newFileName + extension;
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.Post.Create(postVM.Post);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }


        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {

            var post = await _unitOfWork.Post.GetByIdAsync(id);

            IEnumerable<SelectListItem> brandList = _unitOfWork.Brand.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()
            });

            IEnumerable<SelectListItem> vehicleTypeList = _unitOfWork.VehicleType.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()
            });

            IEnumerable<SelectListItem> engineAndFuelType = Enum.GetValues(typeof(EngineAndFuelType))
                .Cast<EngineAndFuelType>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            IEnumerable<SelectListItem> transmission = Enum.GetValues(typeof(Transmission))
               .Cast<Transmission>()
               .Select(x => new SelectListItem
               {
                   Text = x.ToString().ToUpper(),
                   Value = ((int)x).ToString()
               });

            PostVM postVM = new PostVM
            {
                Post = post,
                BrandList = brandList,
                VehicleTypeList = vehicleTypeList,
                EngineAndFuelTypeList = engineAndFuelType,
                TransmissionList = transmission
            };

            return View(postVM);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(PostVM postVM)
        {
            var webRootPath = _webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webRootPath + @"\images\post");

                var extension = Path.GetExtension(file[0].FileName);

                //delete old image
                var objFromDb = await _unitOfWork.Post.GetByIdAsync(postVM.Post.Id);

                var oldImagePath = Path.Combine(webRootPath, objFromDb.VehicleImage.Trim('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                using (var fileSteam = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileSteam);
                }

                postVM.Post.VehicleImage = @"\images\post\" + newFileName + extension;
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.Post.Update(postVM.Post);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }


        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {

            var post = await _unitOfWork.Post.GetByIdAsync(id);

            IEnumerable<SelectListItem> brandList = _unitOfWork.Brand.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()
            });

            IEnumerable<SelectListItem> vehicleTypeList = _unitOfWork.VehicleType.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()
            });

            IEnumerable<SelectListItem> engineAndFuelType = Enum.GetValues(typeof(EngineAndFuelType))
                .Cast<EngineAndFuelType>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            IEnumerable<SelectListItem> transmission = Enum.GetValues(typeof(Transmission))
               .Cast<Transmission>()
               .Select(x => new SelectListItem
               {
                   Text = x.ToString().ToUpper(),
                   Value = ((int)x).ToString()
               });

            PostVM postVM = new PostVM
            {
                Post = post,
                BrandList = brandList,
                VehicleTypeList = vehicleTypeList,
                EngineAndFuelTypeList = engineAndFuelType,
                TransmissionList = transmission
            };

            return View(postVM);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(PostVM postVM)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            if (!string.IsNullOrEmpty(postVM.Post.VehicleImage))
            {

                //delete old image
                var objFromDb = await _unitOfWork.Post.GetByIdAsync(postVM.Post.Id);

                var oldImagePath = Path.Combine(webRootPath, objFromDb.VehicleImage.Trim('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            await _unitOfWork.Post.Delete(postVM.Post);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
