using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopSpeed.DataAccess.Common;
using TopSpeed.Domain.Models;

namespace TopSpeed.Web.Areas.Admin.Controllers
{
    public class VehicleTypeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public VehicleTypeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            List<VehicleType> vehicleTypes = await _dbContext.VehicleType.ToListAsync();

            return View(vehicleTypes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            VehicleType vehicleType = await _dbContext.VehicleType.FirstOrDefaultAsync(x=>x.Id == id);

            return View(vehicleType);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            VehicleType vehicleType = new VehicleType
            {
                Name = name
            };

            _dbContext.VehicleType.Add(vehicleType);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
