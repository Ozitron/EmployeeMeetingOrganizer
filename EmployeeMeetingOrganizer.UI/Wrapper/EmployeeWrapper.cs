﻿using System;
using System.Collections.Generic;
using EmployeeMeetingOrganizer.Model;
using EmployeeMeetingOrganizer.UI.Wrapper.Base;

namespace EmployeeMeetingOrganizer.UI.Wrapper
{
    internal class EmployeeWrapper : ModelWrapper<Employee>
    {
        public EmployeeWrapper(Employee model) : base(model)
        {
        }

        public int Id => Model.Id;

        public string FirstName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string LastName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, string.Empty, StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "First name cannot be empty.";
                    }

                    break;
            }
        }
    }
}
