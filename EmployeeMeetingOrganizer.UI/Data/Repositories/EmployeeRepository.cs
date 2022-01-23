﻿using System;
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
            return await _context.Employees.SingleAsync(e => e.Id == employeeId);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}