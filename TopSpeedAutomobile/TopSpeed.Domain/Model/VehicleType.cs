﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Domain.Common;

namespace TopSpeed.Domain.Model
{
    public class VehicleType : BaseModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
