using ClosedXML.Excel;
using Ikea.Models;
using Ikea.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Ikea.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInventoryService _inventoryService;

        public HomeController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _inventoryService.GetEmployeesAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> GetInventory(int? id, Filter filter = Filter.Employee)
        {
            var viewModel = await _inventoryService.GetFurnituresAsync(id, filter);
            return View(viewModel);
        }

        public async Task<IActionResult> ExportInventoryExcel(int? id, Filter filter)
        {
            var viewModel = await _inventoryService.GetFurnituresAsync(id, filter);
            var organizationalStructure = await _inventoryService.GetOrganizationalStructure(id);
            using (var workbook = new XLWorkbook())
            {
                var name = "";
                switch (filter)
                {
                    case Filter.Employee:
                        name = "Employees";
                        break;
                    case Filter.Department:
                        name = "Department";
                        break;
                    default:
                        name = "BusinessUnit";
                        break;
                }
                var worksheet = workbook.Worksheets.Add(name);
                var currentRow = 1;

                if (id != null)
                {
                    worksheet.Cell(currentRow, 1).Value = filter.ToString();
                    worksheet.Cell(currentRow, 2).Value = organizationalStructure.Name;
                    currentRow++;
                }

                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";

                foreach (var furniture in viewModel.Furnitures)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = furniture.Id;
                    worksheet.Cell(currentRow, 2).Value = furniture.Name;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return await Task.FromResult(
                        File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Furnitures.xlsx"
                        ));
                }
            }
        }

        public async Task<IActionResult> ExportExcel()
        {
            var employees = await _inventoryService.GetEmployeesAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Employees");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "DepartmentId";
                worksheet.Cell(currentRow, 4).Value = "DepartmentName";
                worksheet.Cell(currentRow, 5).Value = "BusinessUnitId";
                worksheet.Cell(currentRow, 6).Value = "BusinessUnitName";

                foreach (var employee in employees)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = employee.Id;
                    worksheet.Cell(currentRow, 2).Value = employee.Name;
                    worksheet.Cell(currentRow, 3).Value = employee.DepartmentId;
                    worksheet.Cell(currentRow, 4).Value = employee.DepartmentName;
                    worksheet.Cell(currentRow, 5).Value = employee.BusinessUnitId;
                    worksheet.Cell(currentRow, 6).Value = employee.BusinessUnitName;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return await Task.FromResult(
                        File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Employees.xlsx"
                        ));
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

