using AvaloniaUI.Ribbon.Desktop;
using System;
using System.Linq;
using Avalonia.VisualTree;

namespace AvaloniaUI.Ribbon.Demo.Views;

public partial class MainWindow : RibbonWindow
{
    public MainWindow()
    {
        InitializeComponent();
        this.Opened += MainWindow_Opened;
    }

    private void MainWindow_Opened(object? sender, EventArgs e)
    {
        try
        {
            Console.WriteLine("[MainWindow] Opened");
            Console.WriteLine($"[MainWindow] Ribbon property is {(this.Ribbon == null ? "null" : "not null")}");
            if (this.Ribbon != null)
            {
                Console.WriteLine($"[MainWindow] Ribbon type: {this.Ribbon.GetType().FullName}");
                try
                {
                    Console.WriteLine($"[MainWindow] Ribbon.IsVisible: {this.Ribbon.IsVisible}");
                    Console.WriteLine($"[MainWindow] Ribbon.Bounds: {this.Ribbon.Bounds}");
                    Console.WriteLine($"[MainWindow] Ribbon.Parent: {this.Ribbon.Parent?.GetType().FullName ?? "(null)"}");
                    try
                    {
                        var gv = this.Ribbon.GetType().GetMethod("GetVisualParent");
                        var vp = gv?.Invoke(this.Ribbon, null);
                        Console.WriteLine($"[MainWindow] Ribbon.VisualParent: {vp?.GetType().FullName ?? "(null)"}");
                    }
                    catch { }
                    try
                    {
                        var prop = this.Ribbon.GetType().GetProperty("IsAttachedToVisualTree");
                        var val = prop?.GetValue(this.Ribbon);
                        Console.WriteLine($"[MainWindow] Ribbon.IsAttachedToVisualTree: {val ?? "(unknown)"}");
                    }
                    catch { }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[MainWindow] Ribbon diagnostics error: " + ex);
                }
                try
                {
                    var dr = this.GetVisualDescendants().FirstOrDefault(v => v.GetType().Name.IndexOf("DesktopRibbon", StringComparison.OrdinalIgnoreCase) >= 0);
                    Console.WriteLine($"[MainWindow] DesktopRibbon found in VisualTree: {(dr != null ? "yes" : "no")}");
                    if (dr != null)
                    {
                        var tabsProp = dr.GetType().GetProperty("Tabs");
                        if (tabsProp != null)
                        {
                            var tabs = tabsProp.GetValue(dr) as System.Collections.ICollection;
                            Console.WriteLine($"[MainWindow] DesktopRibbon Tabs count: {tabs?.Count ?? -1}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[MainWindow] VisualTree diagnostics error: " + ex);
                }
            }

            var all = this.GetVisualDescendants().Take(20).Select(v => v.GetType().FullName).ToArray();
            Console.WriteLine($"[MainWindow] First visual descendants (max 20): {string.Join(", ", all)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[MainWindow] Opened handler error: " + ex);
        }
    }
}