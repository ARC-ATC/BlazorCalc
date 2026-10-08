using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorWasm.Shared;

public partial class MainLayout
{
    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    private bool isDarkTheme;
    private string radzenTheme = "material";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        var theme = await JS.InvokeAsync<string>("examCalc.getTheme");
        isDarkTheme = theme == "dark";
        radzenTheme = isDarkTheme ? "material-dark" : "material";

        await InvokeAsync(StateHasChanged);
    }

    private async Task ToggleTheme()
    {
        var theme = await JS.InvokeAsync<string>("examCalc.toggleTheme");
        isDarkTheme = theme == "dark";
        radzenTheme = isDarkTheme ? "material-dark" : "material";
    }
}