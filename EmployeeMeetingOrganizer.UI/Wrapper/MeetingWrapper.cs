﻿using System;
using EmployeeMeetingOrganizer.Model;
using EmployeeMeetingOrganizer.UI.Wrapper.Base;

namespace EmployeeMeetingOrganizer.UI.Wrapper
{
    internal class MeetingWrapper : ModelWrapper<Meeting>
    {
        public MeetingWrapper(Meeting model) : base(model)
        {
        }
        
        public int Id => Model.Id;

        public string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public DateTime DateFrom
        {
            get => GetValue<DateTime>();
            set
            {
                SetValue(value);
                if (DateTo < DateFrom)
                {
                    DateTo = DateFrom;
                }
            }
        }

        public DateTime DateTo
        {
            get => GetValue<DateTime>();
            set
            {
                SetValue(value);
                if (DateTo < DateFrom)
                {
                    DateFrom = DateTo;
                }
            }
        }
    }
}
