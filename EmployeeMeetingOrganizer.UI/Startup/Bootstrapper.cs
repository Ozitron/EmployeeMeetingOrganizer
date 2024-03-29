﻿using Autofac;
using EmployeeMeetingOrganizer.DataAccess;
using EmployeeMeetingOrganizer.UI.Data;
using EmployeeMeetingOrganizer.UI.Data.Repositories;
using EmployeeMeetingOrganizer.UI.View.Services;
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
            builder.RegisterType<OrganizerDbContext>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();

            builder.RegisterType<EmployeeDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(EmployeeDetailViewModel));
            builder.RegisterType<MeetingDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(MeetingDetailViewModel));

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>();
            builder.RegisterType<MeetingRepository>().As<IMeetingRepository>();

            return builder.Build();
        }
    }
}
