using Microsoft.JSInterop;
using MudBlazor;

namespace BlazorPerformance.Client.Shared;

public partial class MainLayout
{
    private bool _drawerOpen = true;
    private bool _isDarkTheme = false;
    
    private static readonly MudTheme DefaultTheme = new()
    {
        Palette = new Palette
        {
            Black = "#272c34",
            AppbarBackground = "#ffffff",
            AppbarText = "#ff584f",
            DrawerBackground = "#ff584f",
            DrawerText = "ffffff",
            DrawerIcon = "ffffff",
            Primary = "#ff584f",
            Secondary = "#3d6fb4"
        }, PaletteDark = new Palette
        {
            AppbarBackground = "#27272f",
            AppbarText = "#ff584f",
            DrawerBackground = "#ff584f",
            DrawerText = "ffffff",
            DrawerIcon = "ffffff",
            Primary = "#ff584f",
            Secondary = "#3d6fb4",
            Black = "#27272f",
            Background = "#32333d",
            BackgroundGrey = "#27272f",
            Surface = "#373740",
            TextPrimary = "rgba(255,255,255, 0.70)",
            TextSecondary = "rgba(255,255,255, 0.50)",
            ActionDefault = "#adadb1",
            ActionDisabled = "rgba(255,255,255, 0.26)",
            ActionDisabledBackground = "rgba(255,255,255, 0.12)",
            Divider = "rgba(255,255,255, 0.12)",
            DividerLight = "rgba(255,255,255, 0.06)",
            TableLines = "rgba(255,255,255, 0.12)",
            LinesDefault = "rgba(255,255,255, 0.12)",
            LinesInputs = "rgba(255,255,255, 0.3)",
            TextDisabled = "rgba(255,255,255, 0.2)",
            Info = "#3299ff",
            Success = "#0bba83",
            Warning = "#ffa800",
            Error = "#f64e62",
            Dark = "#27272f",
        }
    };
    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void ToggleTheme()
    {
        _isDarkTheme = !_isDarkTheme;
        
    }
}


