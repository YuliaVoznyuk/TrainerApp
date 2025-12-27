using Microsoft.JSInterop;

namespace TrainerApp.Web.Services;

public class JsInterop : IAsyncDisposable
{
    private readonly IJSRuntime _js;

    public JsInterop(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<bool> Confirm(string message)
    {
        return await _js.InvokeAsync<bool>("JS.Confirm", message);
    }

    public async Task Alert(string message)
    {
        await _js.InvokeAsync<object>("JS.Alert", message);
    }

    public async ValueTask DisposeAsync()
    {
        // Якщо потрібно — звільнити ресурси
    }
}