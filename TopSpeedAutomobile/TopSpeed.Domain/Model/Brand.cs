using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Domain.Common;

namespace TopSpeed.Domain.Model
{
    public class Brand : BaseModel
    {
        [Required]
        public string Name { get; set; }


        [Display(Name = "Established Year")]
        public int? EstablishedYear { get; set; }


        [ValidateNever]
        [Display(Name = "Brand Logo")]
        public string BrandLogo { get; set; } = string.Empty;
    }
}
