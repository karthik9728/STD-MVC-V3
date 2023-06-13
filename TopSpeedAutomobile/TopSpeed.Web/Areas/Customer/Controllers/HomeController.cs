using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.Application.ExtensionsMethods;
using TopSpeed.Domain.ModelAggregate.Post;
using TopSpeed.Domain.ViewModel;

namespace TopSpeed.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(int? page, bool resetFilter = false)
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

            //List<Post> posts = await _unitOfWork.Post.GetAllPost();
            List<Post> posts;

            // Check if the resetFilter flag is true
            if (resetFilter)
            {
                // Clear the TempData values to reset the filter
                TempData.Remove("FilteredPosts");
                TempData.Remove("SelectedBrandId");
                TempData.Remove("SelectedVehicleTypeId");
            }

            if (TempData.ContainsKey("FilteredPosts"))
            {
                posts = TempData.Get<List<Post>>("FilteredPosts");
                TempData.Keep("FilteredPosts"); // Keep the TempData for the next request
            }
            else
            {
                posts = await _unitOfWork.Post.GetAllPost();
            }

            //Set the page size (number of items per page)
            int pageSize = 3;

            //Set current page
            int pageNumber = page ?? 1;

            //Calculate the total number of pages
            int totalItems = posts.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            //Pass the total number of pages and current page number to the View

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageNumber;

            var pagedPosts = posts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            HttpContext.Session.SetString("PreviousUrl", HttpContext.Request.Path);

            HomePostVM homePostVM = new HomePostVM
            {
                Posts = pagedPosts,
                BrandList =brandList,
                VehicleTypeList = vehicleTypeList,
                BrandId = (int?)TempData["SelectedBrandId"],
                VehicleTypeId = (int?)TempData["SelectedVehicleTypeId"]
            };

            return View(homePostVM);
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomePostVM homePostVM)
        {
            var posts  = await _unitOfWork.Post.GetAllPost(homePostVM.BrandId,homePostVM.VehicleTypeId);

            TempData.Put("FilteredPosts", posts);
            TempData["SelectedBrandId"] = homePostVM.BrandId;
            TempData["SelectedVehicleTypeId"] = homePostVM.VehicleTypeId;

            return RedirectToAction("Index", new { page = 1, resetFilter = false });
        }

        [Authorize]
        public async Task<IActionResult> Details(int id, int? page)
        {
            var post = await _unitOfWork.Post.GetPostById(id);

            ViewBag.CurrentPage = page;

            return View(post);
        }

        public IActionResult GoBack(int? page)
        {
            string? previousUrl = HttpContext.Session.GetString("PreviousUrl");

            if (!string.IsNullOrEmpty(previousUrl))
            {
                // Append the page number to the previous URL if it exists
                if (page.HasValue)
                {
                    previousUrl = QueryHelpers.AddQueryString(previousUrl, "page", page.Value.ToString());
                }

                HttpContext.Session.Remove("PreviousUrl"); // Remove the session variable

                return Redirect(previousUrl);
            }
            else
            {
                // Handle the case when there is no previous URL stored in the session
                // You can redirect to a default page or take some other action
                return RedirectToAction("Index");
            }
        }

    }
}