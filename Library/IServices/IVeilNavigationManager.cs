
namespace Library.IServices;

public interface IVeilNavigationManager
{
    event Func<int> OnPreNavigation;
    event Action OnAfterNavigation;
    Task NavigateToAsync(string uri, bool forceLoad = false);
    void Refresh();
}
