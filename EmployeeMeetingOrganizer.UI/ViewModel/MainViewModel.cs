﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac.Features.Indexed;
using EmployeeMeetingOrganizer.UI.Event;
using EmployeeMeetingOrganizer.UI.View.Services;
using EmployeeMeetingOrganizer.UI.ViewModel.Base;
using EmployeeMeetingOrganizer.UI.ViewModel.Interface;
using Prism.Commands;
using Prism.Events;

namespace EmployeeMeetingOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private IDetailViewModel _detailViewModel;
        private readonly IMessageDialogService _messageDialogService;
        private IIndex<string, IDetailViewModel> _detailViewModelCreator;

        public MainViewModel(INavigationViewModel navigationViewModel,
            IIndex<string, IDetailViewModel> detailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _detailViewModelCreator = detailViewModelCreator;
            _messageDialogService = messageDialogService;

            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailView);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Subscribe(AfterDetailDeleted);


            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);

            NavigationViewModel = navigationViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public ICommand CreateNewDetailCommand { get; }

        public INavigationViewModel NavigationViewModel { get; }

        public IDetailViewModel DetailViewModel
        {
            get => _detailViewModel;
            private set
            {
                _detailViewModel = value;
                OnPropertyChanged();
            }
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You have made changes. Do you want to navigate away?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            DetailViewModel = _detailViewModelCreator[args.ViewModelName];
            await DetailViewModel.LoadAsync(args.Id);
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = viewModelType.Name });
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            DetailViewModel = null;
        }
    }
}
