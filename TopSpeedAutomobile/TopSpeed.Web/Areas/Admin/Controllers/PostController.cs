using System.Collections.Generic;
using System.Threading.Tasks;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.Domain.ModelAggregate.Post;
using TopSpeed.Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using TopSpeed.Domain.ApplicationEnums;

namespace TopSpeed.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            List<Post> posts = await _unitOfWork.Post.GetAllPost();

            return View(posts);
        }

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
                .Select(x=> new SelectListItem
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
    }
}
