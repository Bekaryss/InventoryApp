using Ikea.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ikea.Models
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationContext _context;
        private readonly string getEmployeesFromDb = $"SELECT * FROM OrganizationalStructure";

        public InventoryService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrganizationalStructure>> GetEmployeesAsync()
        {
            var viewModel = await _context.OrganizationalStructures
                .FromSqlRaw(getEmployeesFromDb)
                .ToListAsync();
            return viewModel;
        }

        public async Task<InventoriesViewModel> GetFurnituresAsync(int? id, Filter filter)
        {
            var viewModel = new InventoriesViewModel();

            viewModel.Employee = await _context.OrganizationalStructures
                .ToListAsync();
            viewModel.Departments = viewModel.Employee
                .Where(p => p.Department != null)
                .Select(p => p.Department)
                .Distinct()
                .ToList();
            viewModel.BusinessUnits = viewModel.Employee
                .Where(p => p.BusinessUnit != null)
                .Select(p => p.BusinessUnit)
                .Distinct()
                .ToList();

            if (id != null)
            {
                viewModel.Id = id;
                viewModel.FilterType = filter;
                if (filter == Filter.Employee)
                {
                    viewModel.Furnitures = await _context.Inventories
                        .Include(i => i.Furniture)
                        .Where(p => p.EmployeeId == id)
                        .Select(p => p.Furniture)
                        .ToListAsync();
                }
                else if (filter == Filter.Department)
                {
                    viewModel.Furnitures = await _context.Inventories
                        .Include(i => i.Furniture)
                        .Include(i => i.Employee)
                        .Where(p => p.Employee.Department == id)
                        .Select(p => p.Furniture)
                        .ToListAsync();
                }
                else
                {
                    viewModel.Furnitures = await _context.Inventories
                        .Include(i => i.Furniture)
                        .Include(i => i.Employee)
                        .Where(p => p.Employee.BusinessUnit == id)
                        .Select(p => p.Furniture)
                        .ToListAsync();
                }
            }
            else
            {
                viewModel.Id = null;
                viewModel.Furnitures = await _context.Furnitures.ToListAsync();
            }

            return viewModel;
        }
    }
}
