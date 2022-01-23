using System;
using System.Threading.Tasks;
using EmployeeMeetingOrganizer.DataAccess;
using EmployeeMeetingOrganizer.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMeetingOrganizer.UI.Data.Repositories
{
    internal class EmployeeRepository : IEmployeeRepository
    {
        private readonly OrganizerContext _context;

        public EmployeeRepository(OrganizerContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.PhoneNumbers)
                .SingleAsync(e => e.Id == employeeId);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
        }
        public void Remove(Employee model)
        {
            _context.Employees.Remove(model);
        }
        public void RemovePhoneNumber(EmployeePhone model)
        {
            _context.PhoneNumbers.Remove(model);
        }
    }
}
