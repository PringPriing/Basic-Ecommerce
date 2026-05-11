namespace Ecommerce.Client.Services;

public enum ToastType { Success, Error, Info }

public record ToastMessage(Guid Id, string Message, ToastType Type);

public class ToastService
{
    public event Action? OnChange;
    public IReadOnlyList<ToastMessage> Toasts => _toasts;

    private readonly List<ToastMessage> _toasts = [];

    public void Show(string message, ToastType type = ToastType.Success)
    {
        var toast = new ToastMessage(Guid.NewGuid(), message, type);
        _toasts.Add(toast);
        OnChange?.Invoke();
        _ = RemoveAfterDelayAsync(toast.Id);
    }

    public void Dismiss(Guid id)
    {
        _toasts.RemoveAll(t => t.Id == id);
        OnChange?.Invoke();
    }

    private async Task RemoveAfterDelayAsync(Guid id)
    {
        await Task.Delay(3500);
        Dismiss(id);
    }
}
