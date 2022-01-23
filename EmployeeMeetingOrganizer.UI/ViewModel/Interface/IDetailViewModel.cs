using System.Threading.Tasks;

namespace EmployeeMeetingOrganizer.UI.ViewModel.Interface
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? id);

        bool HasChanges { get; }
    }
}
