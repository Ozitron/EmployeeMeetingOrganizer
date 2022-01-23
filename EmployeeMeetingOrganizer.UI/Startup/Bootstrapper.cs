using Autofac;
using EmployeeMeetingOrganizer.DataAccess;
using EmployeeMeetingOrganizer.UI.Data;
using EmployeeMeetingOrganizer.UI.Data.Interface;
using EmployeeMeetingOrganizer.UI.ViewModel;
using EmployeeMeetingOrganizer.UI.ViewModel.Interface;
using Prism.Events;

namespace EmployeeMeetingOrganizer.UI.Startup
{
    internal class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<OrganizerContext>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<EmployeeDetailViewModel>().As<IEmployeeDetailViewModel>();
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<EmployeeDataService>().As<IEmployeeDataService>();

            return builder.Build();
        }
    }
}
