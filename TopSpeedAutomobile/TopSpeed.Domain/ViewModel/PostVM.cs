using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TopSpeed.Domain.ModelAggregate.Post;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TopSpeed.Domain.ViewModel
{
    public class PostVM
    {
        public Post Post { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> VehicleTypeList { get; set; }

        public IEnumerable<SelectListItem> EngineAndFuelTypeList { get; set; }

        public IEnumerable<SelectListItem> TransmissionList { get; set; }
    }
}
