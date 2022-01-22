using Autofac;
using EmployeeMeetingOrganizer.DataAccess;
using EmployeeMeetingOrganizer.UI.Data;
using EmployeeMeetingOrganizer.UI.ViewModel;

namespace EmployeeMeetingOrganizer.UI.Startup
{
    internal class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<OrganizerContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<EmployeeDataService>().As<IEmployeeDataService>();

            return builder.Build();
        }
    }
}
