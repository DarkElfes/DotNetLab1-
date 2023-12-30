
using Microsoft.AspNetCore.Components;
using Library.IServices;

namespace MyMauiApplication.Services;

public class VeilNavigationManager(
    NavigationManager navigationManager) : IVeilNavigationManager
{
    private readonly NavigationManager _navigationManager = navigationManager;

    public event Func<int>? OnPreNavigation;
    public event Action OnAfterNavigation;

    public async Task NavigateToAsync(string uri, bool forceLoad = false)
    {
        int delay = OnPreNavigation?.Invoke() ?? 0;
        await Task.Delay(delay);

        _navigationManager.NavigateTo(uri, forceLoad);
        OnAfterNavigation?.Invoke();
    }

    public void Refresh()
        => _navigationManager.Refresh();
}
