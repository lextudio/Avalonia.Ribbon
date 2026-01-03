using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
// ThemeManager removed for local build compatibility

using AvaloniaUI.Ribbon.Demo.ViewModels;
using AvaloniaUI.Ribbon.Demo.Views;

namespace AvaloniaUI.Ribbon.Demo;

public class App : Application
{
    public override void Initialize()
    {
        // ThemeManager initialization removed for local build compatibility
        AvaloniaXamlLoader.Load(this);
        try
        {
            Console.WriteLine("[App] Avalonia Application initialized");
            Console.WriteLine($"[App] Styles count: {this.Styles?.Count ?? 0}");
            var idx = 0;
            var stylesEnum = this.Styles as System.Collections.IEnumerable ?? System.Array.Empty<object>();
            foreach (var s in stylesEnum)
            {
                var typeName = s?.GetType().FullName ?? "(null)";
                var source = "(n/a)";
                try
                {
                    var prop = s?.GetType().GetProperty("Source");
                    if (prop != null)
                    {
                        var val = prop.GetValue(s);
                        source = val?.ToString() ?? "(n/a)";
                    }
                }
                catch { }
                Console.WriteLine($"[App] Style[{idx++}]: {typeName} Source={source}");
            }

            Console.WriteLine($"[App] Resources type: {this.Resources?.GetType().FullName ?? "(null)"}");
            try
            {
                var mdProp = this.Resources?.GetType().GetProperty("MergedDictionaries");
                if (mdProp != null)
                {
                    var md = mdProp.GetValue(this.Resources) as System.Collections.IEnumerable;
                    var count = 0;
                    if (md != null)
                    {
                        foreach (var m in md)
                        {
                            Console.WriteLine($"[App] MergedDict: {m?.GetType().FullName ?? "(null)"}");
                            count++;
                        }
                    }
                    Console.WriteLine($"[App] Resources merged count: {count}");
                }
            }
            catch { }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[App] Diagnostics error: " + ex);
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };

        base.OnFrameworkInitializationCompleted();
    }
}