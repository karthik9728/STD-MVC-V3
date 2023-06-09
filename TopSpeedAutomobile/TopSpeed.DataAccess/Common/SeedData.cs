﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Domain.Models;

namespace TopSpeed.DataAccess.Common
{
    public class SeedData
    {
        public static async Task SeedRole(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new List<IdentityRole>
            {
                new IdentityRole {Name="MASTERADMIN",NormalizedName="MASTERADMIN"},
                new IdentityRole {Name="ADMIN",NormalizedName="ADMIN"},
                new IdentityRole {Name="CUSTOMER",NormalizedName="CUSTOMER"}
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }

        public static async Task SeedDataAsync(ApplicationDbContext _dbContext)
        {
            if (!_dbContext.VehicleType.Any())
            {
                await _dbContext.VehicleType.AddRangeAsync(
                    new VehicleType 
                    { 
                        Name = "Motorcycle"
                    },
                    new VehicleType
                    {
                        Name = "Car"
                    },
                    new VehicleType
                    {
                        Name = "SUV"
                    },
                    new VehicleType
                    {
                        Name = "Van"
                    },
                    new VehicleType
                    {
                        Name = "Sedan"
                    },
                    new VehicleType
                    {
                        Name = "Truck"
                    });

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
