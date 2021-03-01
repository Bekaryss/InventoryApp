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
        private readonly string getEmployeesFromDb =
            $"SELECT A.Id, A.Name, " +
            $"B.Id AS DepartmentId, B.Name AS DepartmentName, " +
            $"C.Id AS BusinessUnitId, C.Name AS BusinessUnitName " +
            $"FROM OrganizationalStructure A,  OrganizationalStructure B, OrganizationalStructure C " +
            $"WHERE A.Department IS NOT NULL " +
            $"AND A.Department = B.Id " +
            $"AND B.BusinessUnit IS NOT NULL " +
            $"AND B.BusinessUnit = C.Id";

        public InventoryService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetEmployeesAsync()
        {
            var viewModel = await _context.Set<EmployeeViewModel>().FromSqlRaw(getEmployeesFromDb).ToListAsync();
            return viewModel;
        }

        public async Task<OrganizationalStructure> GetOrganizationalStructure(int? id)
        {
            var viewModel = await _context.OrganizationalStructures.Where(p => p.Id == id).FirstOrDefaultAsync();
            return viewModel;
        }

        public async Task<InventoriesViewModel> GetFurnituresAsync(int? id, Filter filter)
        {
            var viewModel = new InventoriesViewModel();
            var organizationalStructures = await _context.OrganizationalStructures.ToArrayAsync();

            viewModel.Employee = organizationalStructures
                .Where(p => p.Department != null)
                .ToList();

            viewModel.Departments = organizationalStructures
                .Where(p => p.BusinessUnit != null)
                .ToList();

            viewModel.BusinessUnits = organizationalStructures
                .Where(p => p.BusinessUnit == null && p.Department == null)
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
                    var furnitures = from inventory in _context.Inventories
                                     join furniture in _context.Furnitures on inventory.ObjectId equals furniture.Id
                                     join employee in _context.OrganizationalStructures on inventory.EmployeeId equals employee.Id
                                     join department in _context.OrganizationalStructures on employee.Department equals department.Id
                                     where department.BusinessUnit == id
                                     select new Furniture
                                     {
                                         Id = furniture.Id,
                                         Name = furniture.Name
                                     };
                    viewModel.Furnitures = await furnitures.ToListAsync();
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
