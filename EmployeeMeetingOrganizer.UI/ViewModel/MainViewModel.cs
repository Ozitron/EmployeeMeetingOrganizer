using System;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private readonly Func<IEmployeeDetailViewModel> _employeeDetailViewModelCreator;
        private IEmployeeDetailViewModel _employeeDetailViewModel;
        private readonly IMessageDialogService _messageDialogService;

        public MainViewModel(INavigationViewModel navigationViewModel,
            Func<IEmployeeDetailViewModel> employeeDetailViewModelCreator, 
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _employeeDetailViewModelCreator = employeeDetailViewModelCreator;
            _messageDialogService = messageDialogService;

            _eventAggregator.GetEvent<OpenEmployeeDetailViewEvent>()
                .Subscribe(OnOpenEmployeeDetailView);
            _eventAggregator.GetEvent<AfterEmployeeDeletedEvent>()
                .Subscribe(AfterEmployeeDeleted);


            CreateNewEmployeeCommand = new DelegateCommand(OnCreateNewEmployeeExecute);

            NavigationViewModel = navigationViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public ICommand CreateNewEmployeeCommand { get; }

        public INavigationViewModel NavigationViewModel { get; }

        public IEmployeeDetailViewModel EmployeeDetailViewModel
        {
            get => _employeeDetailViewModel;
            private set
            {
                _employeeDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        private async void OnOpenEmployeeDetailView(int? employeeId)
        {
            if (EmployeeDetailViewModel != null && EmployeeDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You have made changes. Do you want to navigate away?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            EmployeeDetailViewModel = _employeeDetailViewModelCreator();
            await EmployeeDetailViewModel.LoadAsync(employeeId);
        }

        private void OnCreateNewEmployeeExecute()
        {
            OnOpenEmployeeDetailView(null);
        }

        private void AfterEmployeeDeleted(int employeeId)
        {
            EmployeeDetailViewModel = null;
        }
    }
}
