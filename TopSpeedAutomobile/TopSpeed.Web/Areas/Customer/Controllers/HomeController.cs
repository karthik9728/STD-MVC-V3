using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.Domain.ModelAggregate.Post;

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

        public async Task<IActionResult> Index(int? page)
        {
            List<Post> posts = await _unitOfWork.Post.GetAllPost();

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

            return View(pagedPosts);
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