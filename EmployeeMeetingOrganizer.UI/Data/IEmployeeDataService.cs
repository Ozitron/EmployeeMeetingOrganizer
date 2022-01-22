﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeMeetingOrganizer.Model;

namespace EmployeeMeetingOrganizer.UI.Data
{
    public interface IEmployeeDataService
    {
        Task<List<Employee>> GetAllAsync();
    }
}
