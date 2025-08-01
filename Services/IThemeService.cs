namespace MyTestApp.Services
{
    public interface IThemeService
    {
        void SetDarkTheme();
        void SetLightTheme();
        void SetSystemTheme();
        bool IsDarkTheme();
        bool IsSystemTheme();
    }
}