using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Domain.ModelAggregate.Post;

namespace TopSpeed.Domain.ViewModel
{
    public class HomePostVM
    {
        public List<Post> Posts { get; set; }

        public string? searchBox { get; set; } = string.Empty;  

        public int? BrandId { get; set; }

        public int? VehicleTypeId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> VehicleTypeList { get; set; }
    }
}
