using EmployeeMeetingOrganizer.Model;
using EmployeeMeetingOrganizer.UI.Wrapper.Base;

namespace EmployeeMeetingOrganizer.UI.Wrapper
{
    internal class PhoneNumberWrapper : ModelWrapper<EmployeePhone>
    {
        public PhoneNumberWrapper(EmployeePhone model) : base(model)
        {
        }

        public string Number
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}
