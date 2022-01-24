using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeMeetingOrganizer.Model;
using EmployeeMeetingOrganizer.UI.Data.Repositories;
using EmployeeMeetingOrganizer.UI.View.Services;
using EmployeeMeetingOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace EmployeeMeetingOrganizer.UI.ViewModel
{
    internal class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private readonly IMeetingRepository _meetingRepository;
        private MeetingWrapper _meeting;
        private readonly IMessageDialogService _messageDialogService;
        private Employee _selectedAvailableEmployee;
        private Employee _selectedAddedEmployee;
        private List<Employee> _allEmployees;

        public MeetingDetailViewModel(IEventAggregator eventAggregator,
          IMessageDialogService messageDialogService,
          IMeetingRepository meetingRepository) : base(eventAggregator)
        {
            _meetingRepository = meetingRepository;
            _messageDialogService = messageDialogService;

            AddedEmployees = new ObservableCollection<Employee>();
            AvailableEmployees = new ObservableCollection<Employee>();
            AddEmployeeCommand = new DelegateCommand(OnAddEmployeeExecute, OnAddEmployeeCanExecute);
            RemoveEmployeeCommand = new DelegateCommand(OnRemoveEmployeeExecute, OnRemoveEmployeeCanExecute);
        }

        public ICommand AddEmployeeCommand { get; }

        public ICommand RemoveEmployeeCommand { get; }

        public ObservableCollection<Employee> AddedEmployees { get; }

        public ObservableCollection<Employee> AvailableEmployees { get; }

        public Employee SelectedAvailableEmployee
        {
            get => _selectedAvailableEmployee;
            set
            {
                _selectedAvailableEmployee = value;
                OnPropertyChanged();
                ((DelegateCommand)AddEmployeeCommand).RaiseCanExecuteChanged();
            }
        }

        public Employee SelectedAddedEmployee
        {
            get => _selectedAddedEmployee;
            set
            {
                _selectedAddedEmployee = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveEmployeeCommand).RaiseCanExecuteChanged();
            }
        }

        public MeetingWrapper Meeting
        {
            get => _meeting;
            private set
            {
                _meeting = value;
                OnPropertyChanged();
            }
        }

        public override async Task LoadAsync(int? meetingId)
        {
            var meeting = meetingId.HasValue
              ? await _meetingRepository.GetByIdAsync(meetingId.Value)
              : CreateNewMeeting();

            InitializeMeeting(meeting);

            _allEmployees = await _meetingRepository.GetAllEmployeesAsync();

            SetupPersonnelList();
        }

        private void SetupPersonnelList()
        {
            var meetingEmployeeIds = Meeting.Model.Employees.Select(f => f.Id).ToList();
            var addedEmployees = _allEmployees.Where(f => meetingEmployeeIds.Contains(f.Id)).OrderBy(f => f.FirstName);
            var availableEmployees = _allEmployees.Except(addedEmployees).OrderBy(f => f.FirstName);

            AddedEmployees.Clear();
            AvailableEmployees.Clear();
            foreach (var addedEmployee in addedEmployees)
            {
                AddedEmployees.Add(addedEmployee);
            }
            foreach (var availableEmployee in availableEmployees)
            {
                AvailableEmployees.Add(availableEmployee);
            }
        }

        protected override void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete the meeting {Meeting.Title}?", "Question");
            if (result == MessageDialogResult.OK)
            {
                _meetingRepository.Remove(Meeting.Model);
                _meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(Meeting.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _meetingRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Meeting.Id == 0)
            {
                Meeting.Title = "";
            }
        }

        private void OnRemoveEmployeeExecute()
        {
            var EmployeeToRemove = SelectedAddedEmployee;

            Meeting.Model.Employees.Remove(EmployeeToRemove);
            AddedEmployees.Remove(EmployeeToRemove);
            AvailableEmployees.Add(EmployeeToRemove);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveEmployeeCanExecute()
        {
            return SelectedAddedEmployee != null;
        }

        private bool OnAddEmployeeCanExecute()
        {
            return SelectedAvailableEmployee != null;
        }

        private void OnAddEmployeeExecute()
        {
            var employeeToAdd = SelectedAvailableEmployee;

            Meeting.Model.Employees.Add(employeeToAdd);
            AddedEmployees.Add(employeeToAdd);
            AvailableEmployees.Remove(employeeToAdd);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }
    }
}
