using HeatExchangeCalculator.Data;
using HeatExchangeCalculator.Models;
using HeatExchangeCalculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeatExchangeCalculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HeatExchangeService _heatExchangeService;

        public HomeController(ApplicationDbContext context, HeatExchangeService heatExchangeService)
        {
            _context = context;
            _heatExchangeService = heatExchangeService;
        }

        // Главная страница - список расчетов
        public async Task<IActionResult> Index()
        {
            var calculations = await _context.Calculations
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return View(calculations);
        }

        // Создание/редактирование расчета
        [HttpGet]
        public IActionResult Create(int id = 0)
        {
            if (id == 0)
            {
                return View(new Calculation());
            }

            var calculation = _context.Calculations.Find(id);
            if (calculation == null)
            {
                return NotFound();
            }

            return View(calculation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Calculation calculation)
        {
            if (ModelState.IsValid)
            {
                if (calculation.Id == 0)
                {
                    // Новый расчет
                    calculation.CreatedDate = DateTime.Now;
                    _context.Calculations.Add(calculation);
                }
                else
                {
                    // Редактирование существующего
                    var existingCalculation = await _context.Calculations.FindAsync(calculation.Id);
                    if (existingCalculation == null)
                    {
                        return NotFound();
                    }

                    // Обновляем поля
                    existingCalculation.Name = calculation.Name;
                    existingCalculation.LayerHeight = calculation.LayerHeight;
                    existingCalculation.MaterialInitialTemp = calculation.MaterialInitialTemp;
                    existingCalculation.GasInitialTemp = calculation.GasInitialTemp;
                    existingCalculation.GasVelocity = calculation.GasVelocity;
                    existingCalculation.GasHeatCapacity = calculation.GasHeatCapacity;
                    existingCalculation.MaterialFlowRate = calculation.MaterialFlowRate;
                    existingCalculation.MaterialHeatCapacity = calculation.MaterialHeatCapacity;
                    existingCalculation.HeatTransferCoefficient = calculation.HeatTransferCoefficient;
                    existingCalculation.ApparatusDiameter = calculation.ApparatusDiameter;
                    existingCalculation.ModifiedDate = DateTime.Now;

                    calculation = existingCalculation;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Results", new { id = calculation.Id });
            }

            return View(calculation);
        }

        // Просмотр результатов расчета
        public IActionResult Results(int id)
        {
            var calculation = _context.Calculations.Find(id);
            if (calculation == null)
            {
                return NotFound();
            }

            var result = _heatExchangeService.Calculate(calculation);
            return View(result);
        }

        // Отдельная страница с графиками
        public IActionResult Chart(int id)
        {
            var calculation = _context.Calculations.Find(id);
            if (calculation == null)
            {
                return NotFound();
            }

            var result = _heatExchangeService.Calculate(calculation);
            return View(result);
        }

        // Экспорт в CSV
        public IActionResult ExportCsv(int id)
        {
            var calculation = _context.Calculations.Find(id);
            if (calculation == null)
            {
                return NotFound();
            }

            var result = _heatExchangeService.Calculate(calculation);

            var csv = "Высота (м);Отн. высота;Темп. материала (°C);Темп. газа (°C);Разность (°C)\n";
            foreach (var point in result.Points)
            {
                csv += $"{point.Height};{point.RelativeHeight};{point.MaterialTemperature};{point.GasTemperature};{point.TemperatureDifference}\n";
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/csv", $"Расчет_{calculation.Name.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmm}.csv");
        }

        // Удаление расчета
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var calculation = await _context.Calculations.FindAsync(id);
            if (calculation == null)
            {
                return NotFound();
            }

            _context.Calculations.Remove(calculation);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = $"Расчет «{calculation.Name}» удален.";
            return RedirectToAction("Index");
        }
    }
}