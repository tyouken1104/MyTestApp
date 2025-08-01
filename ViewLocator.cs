using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MyTestApp.ViewModels;
using System;

namespace MyTestApp;
public class ViewLocator : IDataTemplate
{
    Control? ITemplate<object?, Control?>.Build(object? param)
    {
        if (param == null)
            return new TextBlock { Text = "Data is null" };

        var name = param.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase || data is MainWindowViewModel;
    }
}
