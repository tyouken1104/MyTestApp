using Avalonia;
using Avalonia.Styling;

namespace MyTestApp.Services
{
    public class ThemeService : IThemeService
    {
        public void SetDarkTheme()
        {
            if (Application.Current != null)
            {
                Application.Current.RequestedThemeVariant = ThemeVariant.Dark;
            }
        }

        public void SetLightTheme()
        {
            if (Application.Current != null)
            {
                Application.Current.RequestedThemeVariant = ThemeVariant.Light;
            }
        }

        public void SetSystemTheme()
        {
            if (Application.Current != null)
            {
                Application.Current.RequestedThemeVariant = ThemeVariant.Default;
            }
        }

        public bool IsDarkTheme()
        {
            return Application.Current?.RequestedThemeVariant == ThemeVariant.Dark;
        }

        public bool IsSystemTheme()
        {
            return Application.Current?.RequestedThemeVariant == ThemeVariant.Default;
        }
    }
}